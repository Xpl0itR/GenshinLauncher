// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GenshinLauncher.FileParsers;
using GenshinLauncher.MiHoYoApi;
using GenshinLauncher.MiHoYoRegistry;
using GenshinLauncher.Ui.Common;

namespace GenshinLauncher
{
    public static class Program
    {
        private static readonly string          BgDirectory;
        private static readonly string          LauncherIniPath;
        private static readonly LauncherIni     LauncherIni;
        private static readonly IUserInterface  Ui;
        private static readonly MiHoYoApiClient ApiClient;

        private static bool            _borderlessMode;
        private static bool            _exitOnLaunch;
        private static string?         _gameRoot;
        private static Version?        _gameVersion;
        private static MiHoYoGameName? _gameName;

        static Program()
        {
            BgDirectory     = Path.Combine(AppContext.BaseDirectory, "bg");
            LauncherIniPath = Path.Combine(AppContext.BaseDirectory, "config.ini");
            LauncherIni     = new LauncherIni(File.Exists(LauncherIniPath) ? LauncherIniPath : null);
            Ui              = new Ui.WinForms.UserInterface();
            ApiClient       = new MiHoYoApiClient(new HttpClient
            {
                DefaultRequestHeaders =
                {
                    // ReSharper disable once StringLiteralTypo
                    { "User-Agent", "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) QtWebEngine/5.12.5 Chrome/69.0.3497.128 Safari/537.36" }
                }
            });
        }

        public static bool CloseToTray { get; private set; }

        [STAThread]
        public static void Main()
        {
            Ui.MainWindow.ButtonAcceptClick   += ButtonAccept_Click;
            Ui.MainWindow.ButtonSettingsClick += ButtonSettings_Click;

            _borderlessMode = ToBoolean(LauncherIni.BorderlessMode) ?? false;
            _exitOnLaunch   = ToBoolean(LauncherIni.ExitOnLaunch)   ?? false;
            _gameRoot       = LauncherIni.GameInstallPath           ?? string.Empty;
            CloseToTray     = LauncherIni.ExitType == "1";

            ReloadGame();

            Ui.RunMainWindow();
        }

        private static void ReloadGame()
        {
            if (string.IsNullOrWhiteSpace(_gameRoot))
            {
                Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonLaunch & ~Components.ButtonUpdate & ~Components.ButtonPreload & ~Components.ButtonDownload;
                return;
            }

            string  gameIniPath = Path.Combine(_gameRoot, "config.ini");
            GameIni gameIni;

            if (!File.Exists(gameIniPath) || (gameIni = new GameIni(gameIniPath)).GameVersion == null)
            {
                Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonLaunch & ~Components.ButtonUpdate & ~Components.ButtonPreload | Components.ButtonDownload;
                return;
            }

            _gameVersion = Version.Parse(gameIni.GameVersion);
            _gameName    = MiHoYoGameNameFromAppInfo(Path.Join(_gameRoot, LauncherIni.GameStartName switch
            {
                "GenshinImpact.exe" => "GenshinImpact_Data",
                "YuanShen.exe"      => "YuanShen_Data",
                "BH3.exe"           => "BH3_Data",
                _                   => throw new ArgumentException()
            }, "app.info"));

            Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonDownload & ~Components.ButtonUpdate & ~Components.ButtonPreload | Components.ButtonLaunch;
            string backgroundPath    = Path.Join(BgDirectory, LauncherIni.GameDynamicBgName);

            if (File.Exists(backgroundPath))
            {
                Ui.MainWindow.BackgroundImage = Image.FromFile(backgroundPath);
            }

            //todo: come up with a better way to do this
            _ = LoadAdditionalContentAsync();
            _ = CheckForUpdates();
        }

        private static async Task LoadAdditionalContentAsync()
        {
            DataJsonContent dataJsonContent = await ApiClient.GetContent(_gameName!.Value, Language.FromCulture(CultureInfo.CurrentUICulture));

            string bgName = Path.GetFileName(dataJsonContent.Adv.Background);
            string bgPath = Path.Combine(BgDirectory, bgName);

            if (!File.Exists(bgPath))
            {
                await using Stream bgStream = new FileStream(bgPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, Utils.DefaultFileStreamBufferSize, true);
                await ApiClient.Download(dataJsonContent.Adv.Background, bgStream);
                Ui.MainWindow.BackgroundImage = Image.FromStream(bgStream);
            }

            LauncherIni.GameDynamicBgName = bgName;
            LauncherIni.GameDynamicBgMd5  = dataJsonContent.Adv.BgChecksum;
            LauncherIni.WriteFile(LauncherIniPath);

            //TODO: implement banner and post viewers into UI
        }

        private static async Task CheckForUpdates()
        {
            Ui.MainWindow.Components |= Components.CheckingForUpdate;

            DataJsonResource resource = await ApiClient.GetResource(_gameName!.Value);

            if (Version.Parse(resource.Game.Latest.Version) > _gameVersion)
            {
                Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonLaunch | Components.ButtonUpdate;
            }

            if (resource.PreDownloadGame != null)
            {
                Ui.MainWindow.Components |= Components.ButtonPreload;
            }

            Ui.MainWindow.Components &= ~Components.CheckingForUpdate;
        }

        private static void OpenSettingsWindow()
        {
            ISettingsWindow settingsWindow = Ui.NewSettingsWindow();

            //TODO: support honkai

            if (_gameName.HasValue)
            {
                using GenshinRegistry registry = new GenshinRegistry(_gameName.Value, false);

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
                    ReloadGame();
                }

                if (_gameName.HasValue)
                {
                    using GenshinRegistry registry = new GenshinRegistry(_gameName.Value, true);

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

        private static async Task Launch()
        {
            if (Process.GetProcesses().Any(process => process.ProcessName == Path.GetFileNameWithoutExtension(LauncherIni.GameStartName)))
            {
                Ui.ShowErrorDialog("GenshinLauncher", "Another instance of Genshin Impact is already running"); //TODO: localize
                return;
            }

            Process process = Process.Start(Path.Join(_gameRoot, LauncherIni.GameStartName));

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

        public static MiHoYoGameName MiHoYoGameNameFromAppInfo(string path)
        {
            using StreamReader reader = new StreamReader(path);
            reader.ReadLine();

            return MiHoYoGameName.Parse(reader.ReadLine());
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