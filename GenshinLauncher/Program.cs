// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private static readonly MiHoYoApiClient ApiClient;
        private static readonly LauncherIni     LauncherIni;
        private static readonly string          LauncherIniPath;
        private static readonly string          BgDirectory;

        private static bool          _borderlessMode;
        private static bool          _exitOnLaunch;
        private static string?       _gameRoot;
        private static string?       _entryPoint;
        private static Version?      _gameVersion;
        private static ResourceJson? _resource;

        static Program()
        {
            Ui              = new Ui.WinForms.UserInterface();
            ApiClient       = new MiHoYoApiClient();
            BgDirectory     = Path.Combine(AppContext.BaseDirectory, "bg");
            LauncherIniPath = Path.Combine(AppContext.BaseDirectory, "config.ini");
            LauncherIni     = new LauncherIni(File.Exists(LauncherIniPath) ? LauncherIniPath : null);
        }

        public static bool CloseToTray { get; private set; }

        [STAThread]
        public static void Main()
        {
            Ui.MainWindow.ButtonAcceptClick   += ButtonAccept_Click;
            Ui.MainWindow.ButtonSettingsClick += ButtonSettings_Click;

            if (File.Exists(LauncherIniPath))
            {
                _gameRoot       = LauncherIni.GameInstallPath;
                _entryPoint     = LauncherIni.GameStartName;
                _borderlessMode = ToBoolean(LauncherIni.BorderlessMode) ?? false;
                _exitOnLaunch   = ToBoolean(LauncherIni.ExitOnLaunch)   ?? false;
                CloseToTray     = LauncherIni.ExitType == "1";

                if (string.IsNullOrWhiteSpace(_gameRoot))
                {
                    OpenSettingsWindow();
                }
                else
                {
                    GameRootUpdated();
                }

                string backgroundPath = Path.Join(BgDirectory, LauncherIni.GameDynamicBgName);
                if (File.Exists(backgroundPath))
                {
                    Ui.MainWindow.BackgroundImage = Image.FromFile(backgroundPath);
                }
            }

            async Task LoadAdditionalContentAsync()
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
                    Ui.MainWindow.BackgroundImage = Image.FromStream(bgStream);
                }

                LauncherIni.GameDynamicBgName = bgName;
                LauncherIni.GameDynamicBgMd5  = contentJson.Adv.BgChecksum;
                LauncherIni.WriteFile(LauncherIniPath);

                //TODO: implement banner and post viewers into UI
            }
            _ = LoadAdditionalContentAsync();

            Ui.RunMainWindow();
        }

        private static void GameRootUpdated()
        {
            string gameIniPath = Path.Join(_gameRoot, "config.ini");

            if (File.Exists(gameIniPath))
            {
                GameIni gameIni = new GameIni(gameIniPath);

                if (gameIni.GameVersion != null)
                {
                    Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonDownload & ~Components.ButtonUpdate & ~Components.ButtonPreload | Components.ButtonLaunch;
                    _gameVersion             = Version.Parse(gameIni.GameVersion);

                    _ = CheckForUpdates(); //TODO: fixme. when this method throws the exception doesn't leave the GameRootUpdated method
                    return;
                }
            }

            Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonLaunch & ~Components.ButtonUpdate & ~Components.ButtonPreload | Components.ButtonDownload;
        }

        private static async Task CheckForUpdates()
        {
            Ui.MainWindow.Components |= Components.CheckingForUpdate;

            _resource = await ApiClient.GetResource(_entryPoint! == ExeNameGlobal);

            if (_gameVersion == null || Version.Parse(_resource.Game.Latest.Version) > _gameVersion)
            {
                Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonLaunch | Components.ButtonUpdate;
            }

            if (_resource.PreDownloadGame != null)
            {
                Ui.MainWindow.Components |= Components.ButtonPreload;
            }

            Ui.MainWindow.Components &= ~Components.CheckingForUpdate;
        }

        private static async Task Launch()
        {
            if (Process.GetProcesses().Any(process => process.ProcessName == Path.GetFileNameWithoutExtension(_entryPoint!)))
            {
                Ui.ShowErrorDialog("GenshinLauncher", "Another instance of Genshin Impact is already running"); //TODO: localize
                return;
            }

            Process process = Process.Start(Path.Join(_gameRoot, _entryPoint));

            if (_borderlessMode)
            {
                IntPtr hWnd = await process.GetMainWindowHandle();

                RemoveWindowTitlebar(hWnd);
                ResizeWindowToFillScreen(hWnd);
            }

            if (_exitOnLaunch)
            {
                Ui.Exit();
            }
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

        private static void OpenSettingsWindow()
        {
            ISettingsWindow settingsWindow = Ui.NewSettingsWindow();

            using (GenshinRegistry registry = new GenshinRegistry(false, _entryPoint != ExeNameChina))
            {
                settingsWindow.CheckBoxCloseToTrayChecked  = CloseToTray;
                settingsWindow.CheckBoxExitOnLaunchChecked = _exitOnLaunch;
                settingsWindow.TextBoxGameDirText          = _gameRoot             ?? string.Empty;
                settingsWindow.NumericMonitorIndexValue    = registry.MonitorIndex ?? 0;
                settingsWindow.NumericMonitorIndexMaximum  = WinApi.GetSystemMetrics(WinApi.SM_CMONITORS) - 1;

                if (_borderlessMode)
                {
                    settingsWindow.RadioButtonBorderlessChecked = true;
                }
                else if (registry.FullscreenMode ?? true)
                {
                    settingsWindow.RadioButtonFullscreenChecked = true;
                }
                else
                {
                    settingsWindow.RadioButtonWindowedChecked = true;
                }

                if (registry.ResolutionHeight != null)
                {
                    settingsWindow.NumericWindowHeightValue = (int)registry.ResolutionHeight;
                }

                if (registry.ResolutionWidth != null)
                {
                    settingsWindow.NumericWindowWidthValue = (int)registry.ResolutionWidth;
                }
            }

            settingsWindow.ButtonSaveClick += (_, _) =>
            {
                if (_gameRoot != settingsWindow.TextBoxGameDirText)
                {
                    _gameRoot = settingsWindow.TextBoxGameDirText;
                    GameRootUpdated();
                }

                using (GenshinRegistry registry = new GenshinRegistry(true, _entryPoint != ExeNameChina))
                {
                    registry.ResolutionHeight = settingsWindow.NumericWindowHeightValue;
                    registry.ResolutionWidth  = settingsWindow.NumericWindowWidthValue;
                    registry.MonitorIndex     = settingsWindow.NumericMonitorIndexValue;

                    if (settingsWindow.RadioButtonBorderlessChecked)
                    {
                        _borderlessMode         = true;
                        registry.FullscreenMode = false;
                    }
                    else
                    {
                        _borderlessMode         = false;
                        registry.FullscreenMode = settingsWindow.RadioButtonFullscreenChecked;
                    }
                }

                LauncherIni.GameInstallPath = _gameRoot;
                LauncherIni.BorderlessMode  = _borderlessMode ? "true" : "false";
                // ReSharper disable AssignmentInConditionalExpression
                LauncherIni.ExitType     = (CloseToTray   = settingsWindow.CheckBoxCloseToTrayChecked)  ? "1"    : "2";
                LauncherIni.ExitOnLaunch = (_exitOnLaunch = settingsWindow.CheckBoxExitOnLaunchChecked) ? "true" : "false";
                // ReSharper restore AssignmentInConditionalExpression
                LauncherIni.WriteFile(LauncherIniPath);

                settingsWindow.Close();
            };

            Ui.RunSettingsWindow(settingsWindow);
        }

        private static bool? ToBoolean(string? value) =>
            (value as IConvertible)?.ToBoolean(null);

        private static void ButtonSettings_Click(object? sender, EventArgs args) =>
            OpenSettingsWindow();

        private static void ButtonAccept_Click(object? sender, EventArgs args)
        {
            if (Ui.MainWindow.Components.HasFlag(Components.ButtonLaunch))
            {
                _ = Launch();
            }
            else if (Ui.MainWindow.Components.HasFlag(Components.ButtonDownload))
            {
                throw new NotImplementedException();
            }
            else if (Ui.MainWindow.Components.HasFlag(Components.ButtonUpdate))
            {
                throw new NotImplementedException();
            }
        }
    }
}