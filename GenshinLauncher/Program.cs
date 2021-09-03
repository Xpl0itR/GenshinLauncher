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
            Ui              = new Ui.WinForms.UserInterface();
            MainWindow      = new Ui.WinForms.MainWindow();
            ApiClient       = new MiHoYoApiClient();
            BgDirectory     = Path.Combine(AppContext.BaseDirectory, "bg");
            LauncherIniPath = Path.Combine(AppContext.BaseDirectory, "config.ini");

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
            MainWindow.CheckBoxCloseToTrayChecked  = CloseToTray;
            MainWindow.CheckBoxExitOnLaunchChecked = ExitOnLaunch;
            MainWindow.TextBoxGameDirText          = GameInstallDir;
            MainWindow.NumericMonitorIndexValue    = (int)MonitorIndex;

            if (BorderlessMode)
            {
                MainWindow.RadioButtonBorderlessChecked = true;
            }
            else if ((bool)FullscreenMode)
            {
                MainWindow.RadioButtonFullscreenChecked = true;
            }
            else
            {
                MainWindow.RadioButtonWindowedChecked = true;
            }

            MainWindow.NumericWindowWidthValueChanged  += NumericWindowWidth_ValueChanged;
            MainWindow.NumericWindowHeightValueChanged += NumericWindowHeight_ValueChanged;

            if (ResolutionHeight == 0 || ResolutionWidth == 0)
            {
                Rectangle bounds = MainWindow.GetCurrentScreenBounds();
                MainWindow.NumericWindowWidthValue  = bounds.Width;
                MainWindow.NumericWindowHeightValue = bounds.Height;
            }
            else
            {
                MainWindow.NumericWindowWidthValue  = (int)ResolutionWidth;
                MainWindow.NumericWindowHeightValue = (int)ResolutionHeight;
            }

            MainWindow.GameDirectoryUpdate                += GameDirectoryUpdated;
            MainWindow.ButtonLaunchClick                  += ButtonLaunch_Click;
            MainWindow.ButtonDownloadClick                += ButtonDownload_Click;
            MainWindow.ButtonUseScreenResolutionClick     += ButtonUseScreenResolution_Click;
            MainWindow.WindowModeCheckedChanged           += WindowMode_CheckedChanged;
            MainWindow.NumericMonitorIndexValueChanged    += NumericMonitorIndex_ValueChanged;
            MainWindow.CheckBoxCloseToTrayCheckedChanged  += CheckBoxCloseToTray_CheckedChanged;
            MainWindow.CheckBoxExitOnLaunchCheckedChanged += CheckBoxExitOnLaunch_CheckedChanged;
            MainWindow.ButtonStopDownloadClick            += (_, _) => _cts?.Cancel();

            if (!File.Exists(EntryPointPath))
            {
                MainWindow.GroupBoxSettingsEnabled = false;
                MainWindow.ShowButtonDownload();
            }

            if (File.Exists(BackgroundPath))
            {
                MainWindow.BackgroundImage = Image.FromFile(BackgroundPath);
            }

            LoadAdditionalContentAsync();

            Ui.Run(MainWindow);
        }

        public static void SaveLauncherConfig()
        {
            LauncherIni ini = File.Exists(LauncherIniPath)
                ? new LauncherIni(LauncherIniPath)
                : new LauncherIni();

            ini.GameInstallPath   = GameInstallDir;
            ini.GameDynamicBgName = BackgroundFileName;
            ini.GameDynamicBgMd5  = BackgroundMd5;
            ini.GameStartName     = EntryPoint;
            ini.ExitType          = CloseToTray    ? "1" : "2";
            ini.BorderlessMode    = BorderlessMode ? "true" : "false";
            ini.ExitOnLaunch      = ExitOnLaunch   ? "true" : "false";
            ini.WriteFile(LauncherIniPath);

            if (ValidEntryPoint)
            {
                using GenshinRegistry genshinRegistry = new GenshinRegistry(true, EntryPoint != ExeNameChina);

                if (ResolutionWidth.Updated)
                {
                    genshinRegistry.ResolutionWidth = ResolutionWidth;
                }

                if (ResolutionHeight.Updated)
                {
                    genshinRegistry.ResolutionHeight = ResolutionHeight;
                }

                if (FullscreenMode.Updated)
                {
                    genshinRegistry.FullscreenMode = FullscreenMode;
                }

                if (MonitorIndex.Updated)
                {
                    genshinRegistry.MonitorIndex = MonitorIndex;
                }
            }
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

            BackgroundMd5 = contentJson.Adv.BgChecksum;
            BackgroundFileName = bgName;

            if (bgStream != null)
            {
                MainWindow.BackgroundImage = Image.FromStream(bgStream);
            }

            //TODO: implement banner and post viewers into UI

            SaveLauncherConfig();
        }

        private static async Task InstallPackage(Package package, string installDir, IProgress<double>? progress, CancellationToken cancellationToken)
        {
            string fileName = Path.GetFileName(package.Path);
            string filePath = Path.Combine(GameInstallDir, fileName + "_tmp");

            await using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, Utils.DefaultFileStreamBufferSize, true))
            {
                using MD5 md5Alg = MD5.Create();

                if (fileStream.Length > 0)
                {
                    MainWindow.LabelProgressBarDownloadTitleText = "Hashing existing parts...";

                    await md5Alg.HashStreamAsync(fileStream, progress, cancellationToken, doFinal: false);
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
                    await ApiClient.Download(package.Path, fileStream, range, md5Alg, progress, cancellationToken);
                    hash = md5Alg.Hash;
                }

                while (!Convert.ToHexString(hash!).Equals(package.Md5, StringComparison.InvariantCultureIgnoreCase))
                {
                    MainWindow.LabelProgressBarDownloadTitleText = "Hash mismatch! Re-downloading...";
                    fileStream.SetLength(0);

                    await ApiClient.Download(package.Path, fileStream, null, md5Alg, progress, cancellationToken);
                    hash = md5Alg.Hash;
                }

                MainWindow.LabelProgressBarDownloadTitleText = "Extracting...";
                using ZipArchive zip = new ZipArchive(fileStream, ZipArchiveMode.Read);
                await zip.ExtractToDirectory(installDir, progress, cancellationToken);
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
                EntryPoint = ExeNameGlobal;
                MainWindow.GroupBoxSettingsEnabled = true;
                MainWindow.ShowButtonLaunch();
            }
            else if (File.Exists(Path.Combine(GameInstallDir, ExeNameChina)))
            {
                EntryPoint = ExeNameChina;
                MainWindow.GroupBoxSettingsEnabled = true;
                MainWindow.ShowButtonLaunch();
            }
            else
            {
                EntryPoint = string.Empty;
                MainWindow.GroupBoxSettingsEnabled = false;
                MainWindow.ShowButtonDownload();
            }
        }

        private static async void ButtonLaunch_Click(object? sender, EventArgs args)
        {
            SaveLauncherConfig();

            string genshinProcessName = Path.GetFileNameWithoutExtension(EntryPoint);
            if (Process.GetProcesses().Any(process => process.ProcessName == genshinProcessName))
            {
                MainWindow.ShowErrorProcessAlreadyRunning();
                return;
            }

            Process process = new Process{ StartInfo = { FileName = EntryPointPath } };
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

        private static async void ButtonDownload_Click(object? sender, EventArgs args)
        {
            SaveLauncherConfig();

            MainWindow.ShowProgressBar();
            MainWindow.SetProgressBarDownloadStyleMarquee();
            MainWindow.LabelProgressBarDownloadTitleText = "Fetching manifest...";
            ResourceJson resourceJson = await ApiClient.GetResource(MainWindow.RadioButtonGlobalVersionChecked);
            Package      latest       = resourceJson.Game.Latest;

            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            Progress<double> progress = new Progress<double>(Progress_Changed);

            try
            {
                MainWindow.SetProgressBarDownloadStyleBlock();
                await InstallPackage(latest, GameInstallDir, progress, _cts.Token);

                string gameIniPath = Path.Combine(GameInstallDir, "config.ini");
                GameIni ini = File.Exists(gameIniPath)
                    ? new GameIni(gameIniPath)
                    : new GameIni();

                ini.GameVersion = latest.Version;
                ini.WriteFile(gameIniPath);

                MainWindow.ShowButtonLaunch();
            }
            catch (TaskCanceledException) { }

            MainWindow.ProgressBarDownloadValue  = 0;
            MainWindow.LabelProgressBarDownloadTitleText = string.Empty;
            MainWindow.LabelProgressBarDownloadText      = string.Empty;
            MainWindow.ShowInstallPath();
        }

        private static void ButtonUseScreenResolution_Click(object? sender, EventArgs args)
        {
            Rectangle bounds = MainWindow.GetCurrentScreenBounds();

            MainWindow.NumericWindowWidthValue = bounds.Width;
            ResolutionWidth.SetValue(bounds.Width);

            MainWindow.NumericWindowHeightValue = bounds.Height;
            ResolutionHeight.SetValue(bounds.Height);
        }

        private static void WindowMode_CheckedChanged(object? sender, EventArgs args)
        {
            if (MainWindow.RadioButtonBorderlessChecked)
            {
                BorderlessMode = true;
                FullscreenMode.SetValue(false);
            }
            else
            {
                BorderlessMode = false;
                FullscreenMode.SetValue(MainWindow.RadioButtonFullscreenChecked);
            }
        }

        private static void NumericWindowWidth_ValueChanged(object? sender, EventArgs args)
        {
            ResolutionWidth.SetValue(MainWindow.NumericWindowWidthValue);
        }

        private static void NumericWindowHeight_ValueChanged(object? sender, EventArgs args)
        {
            ResolutionHeight.SetValue(MainWindow.NumericWindowHeightValue);
        }

        private static void NumericMonitorIndex_ValueChanged(object? sender, EventArgs args)
        {
            MonitorIndex.SetValue(MainWindow.NumericMonitorIndexValue);
        }

        private static void CheckBoxCloseToTray_CheckedChanged(object? sender, EventArgs args)
        {
            CloseToTray = MainWindow.CheckBoxCloseToTrayChecked;
        }

        private static void CheckBoxExitOnLaunch_CheckedChanged(object? sender, EventArgs args)
        {
            ExitOnLaunch = MainWindow.CheckBoxExitOnLaunchChecked;
        }

        private static void Progress_Changed(double decimalPercentage)
        {
            MainWindow.ProgressBarDownloadValue     = (int)(decimalPercentage * int.MaxValue);
            MainWindow.LabelProgressBarDownloadText = $"{(int)(decimalPercentage * 100)}%";
        }
    }
}