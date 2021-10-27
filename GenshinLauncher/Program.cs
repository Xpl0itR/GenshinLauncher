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
using System.Threading;
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

        private static CancellationTokenSource? _ctsLoadGameContent;
        private static CancellationTokenSource? _ctsUpdateCheck;

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
            Ui.MainWindow.ButtonAcceptClick          += ButtonAccept_Click;
            Ui.MainWindow.ButtonDownloadPreloadClick += ButtonDownloadPreload_Click;
            Ui.MainWindow.ButtonSettingsClick        += ButtonSettings_Click;
            Ui.MainWindow.ButtonStopDownloadClick    += ButtonStopDownload_Click;
            Ui.MainWindow.ButtonInstallDirectXClick  += ButtonInstallDirectX_Click;

            _borderlessMode = ToBoolean(LauncherIni.BorderlessMode) ?? false;
            _exitOnLaunch   = ToBoolean(LauncherIni.ExitOnLaunch)   ?? false;
            _gameRoot       = LauncherIni.GameInstallPath           ?? string.Empty;
            CloseToTray     = LauncherIni.ExitType == "1";

            ReloadGame(true);

            Ui.RunMainWindow();
        }

        private static void ReloadGame(bool isFirstLoad)
        {
            if (string.IsNullOrWhiteSpace(_gameRoot))
            {
                Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonLaunch & ~Components.ButtonUpdate & ~Components.ButtonPreload & ~Components.ButtonDownload;
                return;
            }

            string? gameDataPath = Directory.EnumerateFileSystemEntries(_gameRoot, "*_Data").FirstOrDefault();
            string  gameIniPath  = Path.Combine(_gameRoot, "config.ini");
            GameIni gameIni;

            if (string.IsNullOrWhiteSpace(gameDataPath) || !File.Exists(gameIniPath) || (gameIni = new GameIni(gameIniPath)).GameVersion == null)
            {
                Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonLaunch & ~Components.ButtonUpdate & ~Components.ButtonPreload | Components.ButtonDownload;
                return;
            }

            _gameVersion = Version.Parse(gameIni.GameVersion);
            _gameName    = MiHoYoGameNameFromAppInfo(Path.Join(gameDataPath, "app.info"));

            if (!isFirstLoad)
            {
                LauncherIni.GameStartName = $"{Path.GetFileName(gameDataPath)[..^5]}.exe";
            }

            Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonDownload & ~Components.ButtonUpdate & ~Components.ButtonPreload | Components.ButtonLaunch;
            string backgroundPath    = Path.Join(BgDirectory, LauncherIni.GameDynamicBgName);

            if (File.Exists(backgroundPath))
            {
                Ui.MainWindow.BackgroundImage = Image.FromFile(backgroundPath);
            }

            //TODO: come up with a better way to do this
            LoadGameContentAsync().HandleAsyncExceptions();
            CheckForUpdates().HandleAsyncExceptions();
        }

        private static async Task LoadGameContentAsync()
        {
            _ctsLoadGameContent?.Cancel();
            _ctsLoadGameContent?.Dispose();
            _ctsLoadGameContent = new CancellationTokenSource();

            DataJsonContent dataJsonContent = await ApiClient.GetContent(_gameName!.Value, Language.FromCulture(CultureInfo.CurrentUICulture), _ctsLoadGameContent.Token);

            string backgroundName = Path.GetFileName(dataJsonContent.Adv.Background);
            string backgroundPath = Path.Combine(BgDirectory, backgroundName);

            if (File.Exists(backgroundPath))
            {
                if (LauncherIni.GameDynamicBgName != backgroundName)
                {
                    Ui.MainWindow.BackgroundImage = Image.FromFile(backgroundPath);
                }
            }
            else
            {
                Directory.CreateDirectory(BgDirectory);
                await using Stream bgStream = new FileStream(backgroundPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, Utils.DefaultFileStreamBufferSize, true);
                await ApiClient.Download(dataJsonContent.Adv.Background, bgStream, cancellationToken: _ctsLoadGameContent.Token);
                Ui.MainWindow.BackgroundImage = Image.FromStream(bgStream);
            }

            LauncherIni.GameDynamicBgName = backgroundName;
            LauncherIni.GameDynamicBgMd5  = dataJsonContent.Adv.BgChecksum;
            LauncherIni.WriteFile(LauncherIniPath);

            //TODO: implement banner and post viewers into UI
        }

        private static async Task CheckForUpdates()
        {
            Ui.MainWindow.Components &= ~(Components.ButtonUpdate | Components.ButtonPreload);

            _ctsUpdateCheck?.Cancel();
            _ctsUpdateCheck?.Dispose();
            _ctsUpdateCheck = new CancellationTokenSource();

            Ui.MainWindow.Components |= Components.CheckingForUpdate;

            try
            {
                DataJsonResource resource = await ApiClient.GetResource(_gameName!.Value, _ctsUpdateCheck.Token);

                if (Version.Parse(resource.Game.Latest.Version) > _gameVersion)
                {
                    Ui.MainWindow.Components = Ui.MainWindow.Components & ~Components.ButtonLaunch | Components.ButtonUpdate;
                }

                if (resource.PreDownloadGame != null)
                {
                    Ui.MainWindow.Components |= Components.ButtonPreload;
                }
            }
            finally
            {
                Ui.MainWindow.Components &= ~Components.CheckingForUpdate;
            }
        }

        private static void OpenSettingsWindow()
        {
            ISettingsWindow settingsWindow = Ui.NewSettingsWindow();

            settingsWindow.CheckBoxCloseToTrayChecked  = CloseToTray;
            settingsWindow.CheckBoxExitOnLaunchChecked = _exitOnLaunch;
            settingsWindow.TextBoxGameDirText          = _gameRoot ?? string.Empty;
            settingsWindow.NumericMonitorIndexMaximum  = WinApi.GetSystemMetrics(WinApi.SM_CMONITORS) - 1;

            if (_gameName.HasValue)
            {
                using MiHoYoRegistry.MiHoYoRegistry registry = MiHoYoRegistry.MiHoYoRegistry.New(_gameName.Value, false);

                if (_borderlessMode)
                {
                    settingsWindow.RadioButtonBorderlessChecked = true;
                }
                else if (registry.TryGetFullscreenMode(out bool fullscreenMode) && fullscreenMode)
                {
                    settingsWindow.RadioButtonFullscreenChecked = true;
                }
                else
                {
                    settingsWindow.RadioButtonWindowedChecked = true;
                }

                if (registry.TryGetResolutionHeight(out int height))
                {
                    settingsWindow.NumericWindowHeightValue = height;
                }

                if (registry.TryGetResolutionWidth(out int width))
                {
                    settingsWindow.NumericWindowWidthValue = width;
                }

                if (registry.TryGetMonitorIndex(out int index))
                {
                    settingsWindow.NumericMonitorIndexValue = index;
                }
            }

            settingsWindow.ButtonSaveClick += (_, _) =>
            {
                if (_gameRoot != settingsWindow.TextBoxGameDirText)
                {
                    _gameRoot = settingsWindow.TextBoxGameDirText;
                    ReloadGame(false);
                }

                if (_gameName.HasValue)
                {
                    using MiHoYoRegistry.MiHoYoRegistry registry = MiHoYoRegistry.MiHoYoRegistry.New(_gameName.Value, true);

                    registry.SetResolutionHeight(settingsWindow.NumericWindowHeightValue);
                    registry.SetResolutionWidth(settingsWindow.NumericWindowWidthValue);
                    registry.SetMonitorIndex(settingsWindow.NumericMonitorIndexValue);

                    if (settingsWindow.RadioButtonBorderlessChecked)
                    {
                        _borderlessMode = true;
                        registry.SetFullscreenMode(false);
                    }
                    else
                    {
                        _borderlessMode = false;
                        registry.SetFullscreenMode(settingsWindow.RadioButtonFullscreenChecked);
                    }

                    if (registry is HonkaiRegistry honkaiRegistry)
                    {
                        if (honkaiRegistry.TryGetScreenSetting(out JsonSettingScreen screenSetting))
                        {
                            honkaiRegistry.SetScreenSetting(screenSetting with
                            {
                                Width        = settingsWindow.NumericWindowWidthValue,
                                Height       = settingsWindow.NumericWindowHeightValue,
                                IsFullscreen = !_borderlessMode && settingsWindow.RadioButtonFullscreenChecked
                            });
                        }
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
                Ui.ShowErrorDialog("GenshinLauncher", $"Another instance of {_gameName} is already running"); //TODO: localize
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

        private static async Task<IntPtr> GetMainWindowHandle(this Process process) //TODO: improve this
        {
            IntPtr handle = IntPtr.Zero;

            for (int i = 0; i < 25 && (handle = process.MainWindowHandle) == IntPtr.Zero; i++)
            {
                await Task.Delay(200);
            }
            
            // honkai moment
            if (handle == IntPtr.Zero)
            {
                for (int j = 0; j < 25; j++)
                {
                    Process? childProcess = Process.GetProcessesByName(process.ProcessName).FirstOrDefault(p => p.StartTime > process.StartTime && p.MainWindowHandle != IntPtr.Zero);

                    if (childProcess != null)
                    {
                        handle = childProcess.MainWindowHandle;
                        break;
                    }
                }

                await Task.Delay(500);
            }

            if (handle == IntPtr.Zero)
            {
                throw new TimeoutException("Main window handle was not found within the time limit");
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

        private static void HandleAsyncExceptions(this Task task) =>
            task.ContinueWith(t => { if (t.Exception != null) Ui.ShowThreadExceptionDialog(t.Exception); });

        private static bool? ToBoolean(string? value) =>
            (value as IConvertible)?.ToBoolean(null);

        private static void ButtonSettings_Click(object? sender, EventArgs args) =>
            OpenSettingsWindow();

        private static void ButtonAccept_Click(object? sender, EventArgs args)
        {
            if (Ui.MainWindow.Components.HasFlag(Components.ButtonLaunch))
            {
                Launch().HandleAsyncExceptions();
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

        private static void ButtonDownloadPreload_Click(object? sender, EventArgs args) =>
            throw new NotImplementedException();

        private static void ButtonStopDownload_Click(object? sender, EventArgs args) =>
            throw new NotImplementedException();

        private static void ButtonInstallDirectX_Click(object? sender, EventArgs args) =>
            throw new NotImplementedException();
    }
}