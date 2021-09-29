// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

        private static bool          _exitOnLaunch;
        private static string?       _gameRoot;
        private static string?       _entryPoint;
        private static Version?      _gameVersion;
        private static ResourceJson? _resource;

        static Program()
        {
            Ui              = new Ui.WinForms.UserInterface();
            MainWindow      = new Ui.WinForms.MainWindow();
            ApiClient       = new MiHoYoApiClient();
            BgDirectory     = Path.Combine(AppContext.BaseDirectory, "bg");
            LauncherIniPath = Path.Combine(AppContext.BaseDirectory, "config.ini");
        }

        public static bool CloseToTray { get; private set; }

        [STAThread]
        public static void Main()
        {
            MainWindow.ButtonAcceptClick += ButtonAccept_Click;

            LauncherIni launcherIni;
            if (File.Exists(LauncherIniPath))
            {
                launcherIni   = new LauncherIni(LauncherIniPath);
                CloseToTray   = NullableStringEquals(launcherIni.ExitType, "1") ?? false;
                _exitOnLaunch = ToBoolean(launcherIni.ExitOnLaunch)             ?? false;
                _gameRoot     = launcherIni.GameInstallPath;
                _entryPoint   = launcherIni.GameStartName;

                string backgroundPath = Path.Join(BgDirectory, launcherIni.GameDynamicBgName);
                if (File.Exists(backgroundPath))
                {
                    MainWindow.BackgroundImage = Image.FromFile(backgroundPath);
                }

                GameRootUpdated();
            }
            else
            {
                launcherIni = new LauncherIni();
            }

            async void LoadAdditionalContentAsync()
            {
                ContentJson contentJson = _entryPoint == ExeNameChina
                    ? await ApiClient.GetContent(false, Language.Chinese)
                    : await ApiClient.GetContent(true, Language.English); //TODO: load this from CultureInfo

                string bgName = Path.GetFileName(contentJson.Adv.Background);
                string bgPath = Path.Combine(BgDirectory, bgName);

                if (!File.Exists(bgPath))
                {
                    await using Stream bgStream = new FileStream(bgPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, Utils.DefaultFileStreamBufferSize, true);
                    await ApiClient.Download(contentJson.Adv.Background, bgStream);
                    MainWindow.BackgroundImage = Image.FromStream(bgStream);
                }

                launcherIni.GameDynamicBgName = bgName;
                launcherIni.GameDynamicBgMd5  = contentJson.Adv.BgChecksum;
                launcherIni.WriteFile(LauncherIniPath);

                //TODO: implement banner and post viewers into UI
            }
            LoadAdditionalContentAsync();

            Ui.RunMainWindow(MainWindow);
        }

        private static void GameRootUpdated()
        {
            string gameIniPath = Path.Join(_gameRoot, "config.ini");

            if (File.Exists(gameIniPath))
            {
                GameIni gameIni = new GameIni(gameIniPath);

                if (gameIni.GameVersion != null)
                {
                    MainWindow.Components = MainWindow.Components & ~Components.ButtonDownload & ~Components.ButtonUpdate & ~Components.ButtonPreload | Components.ButtonLaunch;
                    _gameVersion          = Version.Parse(gameIni.GameVersion);

                    CheckForUpdates().ContinueWith(t => { if (t.Exception != null) throw t.Exception; });
                    return;
                }
            }

            MainWindow.Components = MainWindow.Components & ~Components.ButtonLaunch & ~Components.ButtonUpdate & ~Components.ButtonPreload | Components.ButtonDownload;
        }

        private static async Task CheckForUpdates()
        {
            MainWindow.Components |= Components.CheckingForUpdate;

            _resource = await ApiClient.GetResource(_entryPoint! == ExeNameGlobal);

            if (_gameVersion == null || Version.Parse(_resource.Game.Latest.Version) > _gameVersion)
            {
                MainWindow.Components = MainWindow.Components & ~Components.ButtonLaunch | Components.ButtonUpdate;
            }

            if (_resource.PreDownloadGame != null)
            {
                MainWindow.Components |= Components.ButtonPreload;
            }

            MainWindow.Components &= ~Components.CheckingForUpdate;
        }

        private static async Task<IntPtr> GetMainWindowHandle(this Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            while (handle == IntPtr.Zero)
            {
                await Task.Delay(300);
                handle = process.MainWindowHandle;
            }

            return handle;
        }

        private static void RemoveWindowTitlebar(IntPtr hWnd)
        {
            long style = WinApi.GetWindowLong(hWnd, WinApi.GWL_STYLE)
                       & ~WinApi.WS_CAPTION
                       & ~WinApi.WS_THICKFRAME;

            WinApi.SetWindowLong(hWnd, WinApi.GWL_STYLE, style);
        }

        private static void ResizeWindowToFillScreen(IntPtr hWnd)
        {
            IntPtr    hMonitor = WinApi.MonitorFromWindow(hWnd, WinApi.MONITOR_DEFAULTTONEAREST);
            Rectangle bounds   = WinApi.GetMonitorInfo(hMonitor).rcMonitor;

            WinApi.SetWindowPos(hWnd, IntPtr.Zero, bounds.X, bounds.Y, bounds.Width, bounds.Height, WinApi.SWP_FRAMECHANGED);
        }

        private static bool? NullableStringEquals(string? value, string equals) =>
            value == null ? null : value == equals;

        private static bool? ToBoolean(string? value) =>
            (value as IConvertible)?.ToBoolean(null);

        private static void ButtonAccept_Click(object? sender, EventArgs args)
        {
            if (MainWindow.Components.HasFlag(Components.ButtonLaunch))
            {
                //TODO: implement this

                if (_exitOnLaunch)
                {
                    Ui.Exit();
                }
            }
            else if (MainWindow.Components.HasFlag(Components.ButtonDownload))
            {
                //TODO: implement this
            }
            else if (MainWindow.Components.HasFlag(Components.ButtonUpdate))
            {
                //TODO: implement this
            }
        }
    }
}