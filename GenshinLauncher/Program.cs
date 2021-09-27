// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using GenshinLauncher.FileParsers;
using GenshinLauncher.MiHoYoApi;
using GenshinLauncher.Ui.Common;

namespace GenshinLauncher
{
    public static class Program
    {
        private const string ExeNameGlobal = "GenshinImpact.exe";
        private const string ExeNameChina  = "YuanShen.exe";

        private static readonly IUserInterface  Ui;
        private static readonly IMainWindow     MainWindow;
        private static readonly MiHoYoApiClient ApiClient;
        private static readonly string          BgDirectory;
        private static readonly string          LauncherIniPath;
        private static readonly SemaphoreSlim   InstallerSemaphore;

        private static CancellationTokenSource? _cts;

        public static string EntryPointPath  => Path.Combine(GameInstallDir, EntryPoint);
        public static string BackgroundPath  => Path.Combine(BgDirectory, BackgroundFileName);
        public static bool   ValidEntryPoint => EntryPoint is ExeNameGlobal or ExeNameChina;

        public static bool   CloseToTray        { get; set; }
        public static bool   BorderlessMode     { get; set; }
        public static bool   ExitOnLaunch       { get; set; }
        public static string GameInstallDir     { get; set; }
        public static string BackgroundFileName { get; set; }
        public static string BackgroundMd5      { get; set; }
        public static string EntryPoint         { get; set; }

        public static Setting<int?>  ResolutionWidth  { get; }
        public static Setting<int?>  ResolutionHeight { get; }
        public static Setting<bool?> FullscreenMode   { get; }
        public static Setting<int?>  MonitorIndex     { get; }

        static Program()
        {
            Ui                 = new Ui.WinForms.UserInterface();
            MainWindow         = new Ui.WinForms.MainWindow();
            ApiClient          = new MiHoYoApiClient();
            BgDirectory        = Path.Combine(AppContext.BaseDirectory, "bg");
            LauncherIniPath    = Path.Combine(AppContext.BaseDirectory, "config.ini");
            InstallerSemaphore = new SemaphoreSlim(1);

            LauncherIni ini = File.Exists(LauncherIniPath)
                ? new LauncherIni(LauncherIniPath)
                : new LauncherIni();

            GameInstallDir     = ini.GameInstallPath                     ?? string.Empty;
            BackgroundFileName = ini.GameDynamicBgName                   ?? string.Empty;
            BackgroundMd5      = ini.GameDynamicBgMd5                    ?? string.Empty;
            EntryPoint         = ini.GameStartName                       ?? string.Empty;
            CloseToTray        = NullableStringEquals(ini.ExitType, "1") ?? false;
            BorderlessMode     = ToBoolean(ini.BorderlessMode)           ?? false;
            ExitOnLaunch       = ToBoolean(ini.ExitOnLaunch)             ?? false;

            using GenshinRegistry? genshinRegistry = ValidEntryPoint
                ? new GenshinRegistry(false, EntryPoint != ExeNameChina)
                : null;

            ResolutionWidth  = new Setting<int?>(genshinRegistry?.ResolutionWidth, 0);
            ResolutionHeight = new Setting<int?>(genshinRegistry?.ResolutionHeight, 0);
            FullscreenMode   = new Setting<bool?>(genshinRegistry?.FullscreenMode, true);
            MonitorIndex     = new Setting<int?>(genshinRegistry?.MonitorIndex, 0);
        }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            MainWindow.Components                =  Components.ButtonDirectX;
            MainWindow.ButtonAcceptClick         += ButtonAccept_Click;
            MainWindow.ButtonSettingsClick       += ButtonSettings_Click;
            MainWindow.ButtonInstallDirectXClick += ButtonInstallDirectX_Click;
            MainWindow.ButtonStopDownloadClick   += (_, _) => _cts?.Cancel();

            if (File.Exists(EntryPointPath))
            {
                MainWindow.Components |= Components.ButtonLaunch;
            }
            else
            {
                MainWindow.Components |= Components.ButtonDownload;
            }

            if (File.Exists(BackgroundPath))
            {
                MainWindow.BackgroundImage = Image.FromFile(BackgroundPath);
            }

            LoadAdditionalContentAsync();

            Ui.RunMainWindow(MainWindow);
        }

        private static async void LoadAdditionalContentAsync()
        {
            ContentJson contentJson = await ApiClient.GetContent(EntryPoint != ExeNameChina, "en-us"); //TODO: load this from CultureInfo
            string  bgName   = Path.GetFileName(contentJson.Adv.Background);
            string  bgPath   = Path.Combine(BgDirectory, bgName);
            Stream? bgStream = null;

            if (!File.Exists(bgPath))
            {
                bgStream = new FileStream(bgPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, Utils.DefaultFileStreamBufferSize, true);
                await ApiClient.Download(contentJson.Adv.Background, bgStream);
            }

            LauncherIni ini = File.Exists(LauncherIniPath)
                ? new LauncherIni(LauncherIniPath)
                : new LauncherIni();

            ini.GameDynamicBgMd5  = contentJson.Adv.BgChecksum;
            ini.GameDynamicBgName = bgName;
            ini.WriteFile(LauncherIniPath);

            if (bgStream != null)
            {
                MainWindow.BackgroundImage = Image.FromStream(bgStream);
            }

            //TODO: implement banner and post viewers into UI
        }

        private static async void Launch()
        {
            string genshinProcessName = Path.GetFileNameWithoutExtension(EntryPoint);
            if (Process.GetProcesses().Any(process => process.ProcessName == genshinProcessName))
            {
                Ui.ShowErrorDialog("GenshinLauncher", "Another instance of Genshin Impact is already running");
                return;
            }

            Process process = new Process { StartInfo = { FileName = EntryPointPath } };
            process.Start();

            if (BorderlessMode)
            {
                IntPtr hWnd = await process.GetMainWindowHandle();

                Utils.RemoveWindowTitlebar(hWnd);
                Utils.ResizeWindowToFillScreen(hWnd);
            }

            if (ExitOnLaunch)
            {
                Ui.Exit();
            }
        }

        private static async void DownloadLatest()
        {
            MainWindow.Components &= ~Components.ButtonDownload;
            await InstallerSemaphore.WaitAsync();
            try
            {
                MainWindow.Components = MainWindow.Components & ~Components.ProgressBarBlocks | Components.ProgressBarMarquee;
                MainWindow.LabelProgressBarDownloadTitleText = "Fetching manifest...";
                ResourceJson resourceJson = await ApiClient.GetResource(MainWindow.RadioButtonGlobalVersionChecked);
                Package latest = resourceJson.Game.Latest;

                try
                {
                    MainWindow.Components = MainWindow.Components & ~Components.ProgressBarMarquee | Components.ProgressBarBlocks;
                    await InstallPackage(latest, GameInstallDir);

                    string gameIniPath = Path.Combine(GameInstallDir, "config.ini");
                    GameIni ini = File.Exists(gameIniPath)
                        ? new GameIni(gameIniPath)
                        : new GameIni();

                    ini.GameVersion = latest.Version;
                    ini.WriteFile(gameIniPath);

                    MainWindow.Components |= Components.ButtonLaunch;
                }
                catch (TaskCanceledException)
                {
                    MainWindow.Components |= Components.ButtonDownload;
                }

                MainWindow.ProgressBarDownloadValue          =  0;
                MainWindow.LabelProgressBarDownloadTitleText =  string.Empty;
                MainWindow.LabelProgressBarDownloadText      =  string.Empty;
                MainWindow.Components                        &= ~Components.ProgressBar;
            }
            finally
            {
                InstallerSemaphore.Release(1);
            }
        }

        private static async Task InstallPackage(Package package, string installDir)
        {
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            Progress<double> progress = new Progress<double>(Progress_Changed);

            string fileName = Path.GetFileName(package.Path);
            string filePath = Path.Combine(installDir, fileName + "_tmp");

            await using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, Utils.DefaultFileStreamBufferSize, true))
            {
                using MD5 md5Alg = MD5.Create();

                if (fileStream.Length > 0)
                {
                    MainWindow.LabelProgressBarDownloadTitleText = "Hashing existing parts...";

                    await md5Alg.HashStreamAsync(fileStream, progress, _cts.Token, doFinal: false);
                }

                byte[]? hash;
                if (fileStream.Length == long.Parse(package.Size))
                {
                    md5Alg.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
                    hash = md5Alg.Hash;
                }
                else
                {
                    MainWindow.LabelProgressBarDownloadTitleText = "Downloading...";

                    RangeHeaderValue range = new RangeHeaderValue(fileStream.Position, null);
                    await ApiClient.Download(package.Path, fileStream, range, md5Alg, progress, _cts.Token);
                    hash = md5Alg.Hash;
                }

                while (!Convert.ToHexString(hash!).Equals(package.Md5, StringComparison.InvariantCultureIgnoreCase))
                {
                    MainWindow.LabelProgressBarDownloadTitleText = "Hash mismatch! Re-downloading...";
                    fileStream.SetLength(0);

                    await ApiClient.Download(package.Path, fileStream, null, md5Alg, progress, _cts.Token);
                    hash = md5Alg.Hash;
                }

                MainWindow.LabelProgressBarDownloadTitleText = "Extracting...";
                using ZipArchive zip = new ZipArchive(fileStream, ZipArchiveMode.Read);
                await zip.ExtractToDirectory(installDir, progress, _cts.Token);
            }

            File.Delete(filePath);
        }

        private static bool? NullableStringEquals(string? value, string equals) =>
            value == null ? null : value == equals;

        private static bool? ToBoolean(string? value) =>
            (value as IConvertible)?.ToBoolean(null);

        private static void GameDirectoryUpdated(object? sender, string newPath)
        {
            GameInstallDir = newPath.Replace(@"\", "/");

            if (File.Exists(Path.Combine(GameInstallDir, ExeNameGlobal)))
            {
                EntryPoint            = ExeNameGlobal;
                MainWindow.Components = MainWindow.Components & ~Components.ButtonDownload | Components.ButtonLaunch;
            }
            else if (File.Exists(Path.Combine(GameInstallDir, ExeNameChina)))
            {
                EntryPoint            = ExeNameChina;
                MainWindow.Components = MainWindow.Components & ~Components.ButtonDownload | Components.ButtonLaunch;
            }
            else
            {
                EntryPoint            = string.Empty;
                MainWindow.Components = MainWindow.Components & ~Components.ButtonLaunch | Components.ButtonDownload;
            }
        }

        private static void ButtonAccept_Click(object? sender, EventArgs args)
        {
            if (MainWindow.Components.HasFlag(Components.ButtonLaunch))
            {
                Launch();
            }
            else if(MainWindow.Components.HasFlag(Components.ButtonDownload))
            {
                DownloadLatest();
            }
        }

        private static void ButtonSettings_Click(object? sender, EventArgs args)
        {
            ISettingsWindow settingsWindow = new Ui.WinForms.SettingsWindow();

            settingsWindow.CheckBoxCloseToTrayChecked  = CloseToTray;
            settingsWindow.CheckBoxExitOnLaunchChecked = ExitOnLaunch;
            settingsWindow.TextBoxGameDirText          = GameInstallDir;
            settingsWindow.NumericMonitorIndexValue    = (int)MonitorIndex;
            settingsWindow.NumericMonitorIndexMaximum  = WinApi.GetSystemMetrics(WinApi.SM_CMONITORS) - 1;
            
            if (BorderlessMode)
            {
                settingsWindow.RadioButtonBorderlessChecked = true;
            }
            else if ((bool)FullscreenMode)
            {
                settingsWindow.RadioButtonFullscreenChecked = true;
            }
            else
            {
                settingsWindow.RadioButtonWindowedChecked = true;
            }

            if (ResolutionHeight == 0 || ResolutionWidth == 0)
            {
                Rectangle bounds = MainWindow.GetCurrentScreenBounds();
                settingsWindow.NumericWindowWidthValue  = bounds.Width;
                settingsWindow.NumericWindowHeightValue = bounds.Height;
            }
            else
            {
                settingsWindow.NumericWindowWidthValue  = (int)ResolutionWidth;
                settingsWindow.NumericWindowHeightValue = (int)ResolutionHeight;
            }

            settingsWindow.ShowDialog(MainWindow);
        }

        private static async void ButtonInstallDirectX_Click(object? sender, EventArgs args)
        {
            MainWindow.Components &= ~Components.ButtonDirectX;
            await InstallerSemaphore.WaitAsync();

            try
            {
                MainWindow.Components = MainWindow.Components & ~Components.ProgressBarBlocks | Components.ProgressBarMarquee;
                MainWindow.LabelProgressBarDownloadTitleText = "Fetching manifest...";
                ResourceJson resourceJson = await ApiClient.GetResource(MainWindow.RadioButtonGlobalVersionChecked);
                Package      directX      = resourceJson.Plugin.Plugins.First(package => package.Name == "DXSETUP.zip");

                try
                {
                    MainWindow.Components = MainWindow.Components & ~Components.ProgressBarMarquee | Components.ProgressBarBlocks;
                    await InstallPackage(directX, AppContext.BaseDirectory);

                    Process.Start(Path.Combine(AppContext.BaseDirectory, "DXSETUP", "DXSETUP.exe"));
                }
                catch (TaskCanceledException) { }

                MainWindow.ProgressBarDownloadValue          =  0;
                MainWindow.LabelProgressBarDownloadTitleText =  string.Empty;
                MainWindow.LabelProgressBarDownloadText      =  string.Empty;
                MainWindow.Components                        &= ~Components.ProgressBar;
            }
            finally
            {
                InstallerSemaphore.Release(1);
                MainWindow.Components |= Components.ButtonDirectX;
            }
        }

        private static void Progress_Changed(double decimalPercentage)
        {
            MainWindow.ProgressBarDownloadValue     = (int)(decimalPercentage * int.MaxValue);
            MainWindow.LabelProgressBarDownloadText = $"{(int)(decimalPercentage * 100)}%";
        }
    }
}