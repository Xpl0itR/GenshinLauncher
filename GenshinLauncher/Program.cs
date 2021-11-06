// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using GenshinLauncher.FileParsers;
using GenshinLauncher.MiHoYoCdn;
using GenshinLauncher.MiHoYoRegistry;
using GenshinLauncher.Ui.Common;
using GenshinLauncher.Ui.WinForms;

namespace GenshinLauncher;

public static class Program
{
    private static readonly string          BgDirectory;
    private static readonly string          LauncherIniPath;
    private static readonly LauncherIni     LauncherIni;
    private static readonly IUserInterface  Ui;
    private static readonly HttpClient      HttpClient;
    private static readonly MiHoYoCdnClient CdnClient;

    private static CancellationTokenSource? _ctsLoadGameContent;
    private static CancellationTokenSource? _ctsUpdateCheck;
    private static CancellationTokenSource? _ctsMain;

    private static bool            _borderlessMode;
    private static bool            _exitOnLaunch;
    private static string?         _gameRoot;
    private static Version?        _gameVersion;
    private static MiHoYoGameName? _gameName;

    static Program()
    {
        HttpClient = new HttpClient
        {
            DefaultRequestHeaders =
            {
                // ReSharper disable once StringLiteralTypo
                { "User-Agent", "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) QtWebEngine/5.12.5 Chrome/69.0.3497.128 Safari/537.36" }
            }
        };

        BgDirectory     = Path.Combine(AppContext.BaseDirectory, "bg");
        LauncherIniPath = Path.Combine(AppContext.BaseDirectory, "config.ini");
        LauncherIni     = new LauncherIni(File.Exists(LauncherIniPath) ? LauncherIniPath : null);
        Ui              = new UserInterface();
        CdnClient       = new MiHoYoCdnClient(HttpClient);
    }

    public static bool CloseToTray { get; private set; }

    [STAThread]
    public static void Main()
    {
        Ui.MainWindow.ButtonAcceptClick          += ButtonAccept_Click;
        Ui.MainWindow.ButtonDownloadPreloadClick += ButtonDownloadPreload_Click;
        Ui.MainWindow.ButtonSettingsClick        += ButtonSettings_Click;
        Ui.MainWindow.ButtonStopClick            += ButtonStopDownload_Click;
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
            Ui.MainWindow.Components = (Ui.MainWindow.Components & ~Components.ButtonLaunch & ~Components.ButtonUpdate & ~Components.ButtonPreload) | Components.ButtonDownload;
            return;
        }

        _gameVersion = Version.Parse(gameIni.GameVersion);
        _gameName    = MiHoYoGameNameFromAppInfo(Path.Join(gameDataPath, "app.info"));

        if (!isFirstLoad)
        {
            LauncherIni.GameStartName = $"{Path.GetFileName(gameDataPath)[..^5]}.exe";
        }

        Ui.MainWindow.Components = (Ui.MainWindow.Components & ~Components.ButtonDownload & ~Components.ButtonUpdate & ~Components.ButtonPreload) | Components.ButtonLaunch;
        string backgroundPath = Path.Join(BgDirectory, LauncherIni.GameDynamicBgName);

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

        DataJsonContent dataJsonContent = await CdnClient.GetContent(_gameName!.Value, Language.FromCulture(CultureInfo.CurrentUICulture), _ctsLoadGameContent.Token);

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
            await using Stream dlStream = await HttpClient.GetDownloadStream(dataJsonContent.Adv.Background, _ctsLoadGameContent.Token);
            await dlStream.CopyToAsync(bgStream, _ctsLoadGameContent.Token);

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
            DataJsonResource resource = await CdnClient.GetResource(_gameName!.Value, _ctsUpdateCheck.Token);

            if (Version.Parse(resource.Game.Latest.Version) > _gameVersion)
            {
                Ui.MainWindow.Components = (Ui.MainWindow.Components & ~Components.ButtonLaunch) | Components.ButtonUpdate;
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
                        honkaiRegistry.SetScreenSetting(screenSetting with { Width = settingsWindow.NumericWindowWidthValue, Height = settingsWindow.NumericWindowHeightValue, IsFullscreen = !_borderlessMode && settingsWindow.RadioButtonFullscreenChecked });
                    }
                }
            }

            LauncherIni.GameInstallPath = _gameRoot;
            LauncherIni.BorderlessMode  = _borderlessMode ? "true" : "false";
            // ReSharper disable AssignmentInConditionalExpression
            LauncherIni.ExitType     = (CloseToTray   = settingsWindow.CheckBoxCloseToTrayChecked) ? "1" : "2";
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
            Ui.ShowErrorDialog("GenshinLauncher", string.Format(LocalizedStrings.AnotherInstanceIsRunning, _gameName)); //TODO: store program name in one const
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

    private static async Task<Stream> GetDownloadStream(this HttpClient httpClient, string url, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();

        Stream contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
        return new LengthStream(contentStream, response.Content.Headers.ContentLength);
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

                await Task.Delay(200);
            }
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

    private static MiHoYoGameName MiHoYoGameNameFromAppInfo(string path)
    {
        using StreamReader reader = new(path);
        reader.ReadLine();

        return MiHoYoGameName.Parse(reader.ReadLine());
    }

    private static void HandleAsyncExceptions(this Task task) =>
        task.ContinueWith
        (
            t =>
            {
                if (t.Exception != null)
                    Ui.ShowThreadExceptionDialog(t.Exception);
            }
        );

    private static bool? ToBoolean(string? value) =>
        (value as IConvertible)?.ToBoolean(null);

    private static void ButtonSettings_Click(object? sender, EventArgs args) =>
        OpenSettingsWindow();

    private static void ButtonStopDownload_Click(object? sender, EventArgs args) =>
        _ctsMain?.Cancel();

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

    private static async void ButtonInstallDirectX_Click(object? sender, EventArgs args)
    {
        Ui.MainWindow.Components               = (Ui.MainWindow.Components & ~Components.ProgressBarBlocks) | Components.ProgressBarMarquee | Components.DisableDownloading;
        Ui.MainWindow.LabelProgressBarTextLeft = LocalizedStrings.FetchingManifest;

        // Honkai's manifest doesn't include DirectX
        MiHoYoGameName gameName = _gameName == MiHoYoGameName.BengHuai || _gameName == MiHoYoGameName.YuanShen
            ? MiHoYoGameName.YuanShen
            : MiHoYoGameName.Genshin;

        _ctsMain = new CancellationTokenSource();

        try
        {
            DataJsonResource resource   = await CdnClient.GetResource(gameName, _ctsMain.Token);
            Package          directXPkg = resource.Plugin.Plugins.First(package => package.Name == "DXSETUP.zip");

            Ui.MainWindow.Components = (Ui.MainWindow.Components & ~Components.ProgressBarMarquee) | Components.ProgressBarBlocks;

            string fileName = Path.GetFileName(directXPkg.Path);
            string filePath = Path.Combine(AppContext.BaseDirectory, fileName + "_tmp");

            // TODO: make resumable
            await using (FileStream fileStream = new(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, Utils.DefaultFileStreamBufferSize, true))
            await using (Stream downloadStream = await HttpClient.GetDownloadStream(directXPkg.Path, _ctsMain.Token))
            using (MD5 md5Alg = MD5.Create())
            {
                byte[] buffer1 = ArrayPool<byte>.Shared.Rent(Utils.DefaultFileStreamBufferSize);

                try
                {
                    TransferProgressTracker transferProgressTracker = new(downloadStream.Length, 1000);
                    transferProgressTracker.ProgressChanged += Progress_Changed;

                    int bytesRead;

                    while ((bytesRead = await downloadStream.ReadAsync(buffer1, _ctsMain.Token)) > 0)
                    {
                        md5Alg.TransformBlock(buffer1, 0, bytesRead, null, 0);
                        await fileStream.WriteAsync(buffer1, 0, bytesRead, _ctsMain.Token);

                        transferProgressTracker.Report(bytesRead);
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer1);
                    md5Alg.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
                }

                if (!Convert.ToHexString(md5Alg.Hash!).Equals(directXPkg.Md5, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new Exception("Hash mismatch!");
                }

                using ZipArchive zipArchive = new(fileStream, ZipArchiveMode.Read);

                double decompressedLength = 0;
                long   uncompressedLength = zipArchive.Entries.Aggregate(0L, (current, entry) => current + entry.Length);
                string destinationPath    = AppContext.BaseDirectory; //Directory.CreateDirectory(destinationPath).FullName;

                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    _ctsMain.Token.ThrowIfCancellationRequested();

                    string entryPath = Path.GetFullPath(Path.Combine(destinationPath, entry.FullName));

                    if (!entryPath.StartsWith(destinationPath, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new IOException("Zip entry path is outside the destination directory");
                    }

                    if (entry.Name == string.Empty)
                    {
                        if (entry.Length > 0)
                        {
                            throw new IOException("Zip entry is supposed to be a folder but contains data");
                        }

                        Directory.CreateDirectory(entryPath);
                    }
                    else
                    {
                        await using (Stream outStream = new FileStream(entryPath, FileMode.Create, FileAccess.Write, FileShare.None, Utils.DefaultFileStreamBufferSize, true))
                        await using (Stream entryStream = entry.Open())
                        {
                            byte[] buffer2 = ArrayPool<byte>.Shared.Rent(Utils.DefaultFileStreamBufferSize);

                            try
                            {
                                int bytesRead;

                                while ((bytesRead = await entryStream.ReadAsync(buffer2, _ctsMain.Token)) > 0)
                                {
                                    await outStream.WriteAsync(buffer2, 0, bytesRead, _ctsMain.Token);

                                    decompressedLength += bytesRead;
                                    double decimalPercentage = decompressedLength / uncompressedLength;

                                    Ui.MainWindow.ProgressBarValue         = (int)(decimalPercentage * int.MaxValue);
                                    Ui.MainWindow.LabelProgressBarTextLeft = string.Format(LocalizedStrings.Extracting, decimalPercentage * 100, $"{decompressedLength}B", $"{uncompressedLength}B");
                                }
                            }
                            finally
                            {
                                ArrayPool<byte>.Shared.Return(buffer2);
                            }
                        }

                        File.SetLastWriteTime(entryPath, entry.LastWriteTime.DateTime);
                    }
                }
            }

            File.Delete(filePath);
            Process.Start(Path.Combine(AppContext.BaseDirectory, "DXSETUP", "DXSETUP.exe"));
        }
        catch (TaskCanceledException) { }
        finally
        {
            _ctsMain.Dispose();
            Ui.MainWindow.ProgressBarValue           = 0;
            Ui.MainWindow.LabelProgressBarTextLeft   = string.Empty;
            Ui.MainWindow.LabelProgressBarTextRight  = string.Empty;
            Ui.MainWindow.LabelProgressBarTextBottom = string.Empty;
            Ui.MainWindow.Components                 = Ui.MainWindow.Components & ~Components.DisableDownloading & ~Components.ProgressBar;
        }
    }

    private static void Progress_Changed(object? sender, TransferProgress transferProgress)
    {
        Ui.MainWindow.ProgressBarValue           = (int)(transferProgress.FractionCompleted * int.MaxValue);
        Ui.MainWindow.LabelProgressBarTextLeft   = string.Format(LocalizedStrings.Downloading,   transferProgress.FractionCompleted * 100, transferProgress.TotalBytesTransferredString, transferProgress.TotalBytesString);
        Ui.MainWindow.LabelProgressBarTextRight  = string.Format(LocalizedStrings.TimeRemaining, transferProgress.EstimatedTimeRemaining);
        Ui.MainWindow.LabelProgressBarTextBottom = string.Format(LocalizedStrings.Speed,         transferProgress.AverageBytesPerSecondString);
    }
}