// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GenshinLauncher
{
    public class Launcher
    {
        public const string ExeNameGlobal = "GenshinImpact.exe";
        public const string ExeNameChina  = "YuanShen.exe";

        private readonly GenshinHttpClient _httpClient;
        private readonly Process           _genshinProcess;
        private readonly string            _bgDirectory;
        private readonly string            _launcherIniPath;

        public bool   CloseToTray        { get; set; }
        public bool   BorderlessMode     { get; set; }
        public bool   ExitOnLaunch       { get; set; }
        public string GameInstallDir     { get; set; }
        public string BackgroundFileName { get; set; }
        public string BackgroundMd5      { get; set; }
        public string EntryPoint         { get; set; }

        public RegistrySetting<int?>  ResolutionWidth  { get; }
        public RegistrySetting<int?>  ResolutionHeight { get; }
        public RegistrySetting<bool?> FullscreenMode   { get; }
        public RegistrySetting<int?>  MonitorIndex     { get; }

        public string EntryPointPath  => Path.Combine(GameInstallDir, EntryPoint);
        public string BackgroundPath  => Path.Combine(_bgDirectory, BackgroundFileName);
        public bool   ValidEntryPoint => EntryPoint is ExeNameGlobal or ExeNameChina;

        public Launcher()
        {
            _httpClient      = new GenshinHttpClient();
            _genshinProcess  = new Process();
            _bgDirectory     = Path.Combine(AppContext.BaseDirectory, "bg");
            _launcherIniPath = Path.Combine(AppContext.BaseDirectory, "config.ini");

            LauncherConfig ini = File.Exists(_launcherIniPath)
                ? new LauncherConfig(_launcherIniPath)
                : new LauncherConfig();

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

            ResolutionWidth  = new RegistrySetting<int?>(genshinRegistry?.ResolutionWidth, 0);
            ResolutionHeight = new RegistrySetting<int?>(genshinRegistry?.ResolutionHeight, 0);
            FullscreenMode   = new RegistrySetting<bool?>(genshinRegistry?.FullscreenMode, true);
            MonitorIndex     = new RegistrySetting<int?>(genshinRegistry?.MonitorIndex, 0);
        }

        public void SaveLauncherConfig()
        {
            LauncherConfig ini = File.Exists(_launcherIniPath)
                ? new LauncherConfig(_launcherIniPath)
                : new LauncherConfig();

            ini.GameInstallPath   = GameInstallDir;
            ini.GameDynamicBgName = BackgroundFileName;
            ini.GameDynamicBgMd5  = BackgroundMd5;
            ini.GameStartName     = EntryPoint;
            ini.ExitType          = CloseToTray    ? "1"    : "2";
            ini.BorderlessMode    = BorderlessMode ? "true" : "false";
            ini.ExitOnLaunch      = ExitOnLaunch   ? "true" : "false";
            ini.WriteFile(_launcherIniPath);

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

        public async Task<(string bgName, string bgMd5, Banner[] banners, Post[] posts, Stream? bgStream)> GetAdditionalContent(string language)
        {
            Stream? stream  = null;
            Content content = await _httpClient.GetContent(EntryPoint != ExeNameChina, language);

            string bgName = Path.GetFileName(content.Adv.Background.AbsolutePath);
            string bgPath = Path.Combine(_bgDirectory, bgName);

            if (!File.Exists(bgPath))
            {
                stream = new FileStream(bgPath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 4096, true);
                await _httpClient.Download(content.Adv.Background, stream);
            }

            return (bgName, content.Adv.BgChecksum, content.Banner, content.Post, stream);
        }

        public async Task DownloadLatestVersion(bool globalVersion)
        {
            Resource resource = await _httpClient.GetResource(globalVersion);
            Package  latest   = resource.Game.Latest;

            await InstallPackage(latest);

            string gameIniPath = Path.Combine(GameInstallDir, "config.ini");
            GameConfig ini = File.Exists(gameIniPath)
                ? new GameConfig(gameIniPath)
                : new GameConfig();

            ini.GameVersion = latest.Version;
            ini.WriteFile(gameIniPath);
        }

        public void StartGame()
        {
            string genshinProcessName = Path.GetFileNameWithoutExtension(EntryPoint);
            if (Process.GetProcesses().Any(process => process.ProcessName == genshinProcessName))
            {
                throw new InvalidOperationException();
            }

            _genshinProcess.StartInfo.FileName = EntryPointPath;
            _genshinProcess.Start();
        }

        public void RemoveGameTitlebar()
        {
            IntPtr hWnd  = GetMainWindowHandle();
            long   style = WinApi.GetWindowLong(hWnd, WinApi.GWL_STYLE);

            WinApi.SetWindowLong(hWnd, WinApi.GWL_STYLE, style & ~WinApi.WS_CAPTION & ~WinApi.WS_THICKFRAME);
        }

        public void ResizeGameToFillBounds()
        {
            IntPtr    hWnd   = GetMainWindowHandle();
            Rectangle bounds = Screen.FromHandle(hWnd).Bounds;

            WinApi.SetWindowPos(hWnd, IntPtr.Zero, bounds.X, bounds.Y, bounds.Width, bounds.Height, WinApi.SWP_FRAMECHANGED);
        }

        private IntPtr GetMainWindowHandle()
        {
            IntPtr handle = _genshinProcess!.MainWindowHandle;
            while (handle == IntPtr.Zero)
            {
                handle = _genshinProcess.MainWindowHandle;
            }

            return handle;
        }

        private async Task InstallPackage(Package package)
        {
            string fileName   = Path.GetFileName(package.Path.AbsolutePath);
            string filePath   = Path.Combine(GameInstallDir, fileName) + "_tmp";

            await using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None, 4096, true))
            {
                while (!VerifyMd5(stream, package.Md5))
                {
                    stream.SetLength(0);
                    await _httpClient.Download(package.Path, stream);
                }

                using ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Read);

                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    entry.ExtractRelativeToDirectory(GameInstallDir, true);
                }
            }

            File.Delete(filePath);
        }

        private async Task DownloadPackage(Package package)
        {
            string fileName = Path.GetFileName(package.Path.AbsolutePath);
            string filePath = Path.Combine(GameInstallDir, fileName) + "_tmp";

            await using Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read, 4096, true);
            await _httpClient.Download(package.Path, stream);
        }

        public async void VerifyInstall()
        {
            using StreamReader reader = new StreamReader(Path.Combine(GameInstallDir, "pkg_version"), Encoding.UTF8);
            using MD5 md5Alg = MD5.Create();

            List<PkgVersionEntry> failedList = new List<PkgVersionEntry>();

            foreach (PkgVersionEntry entry in new PkgVersion(reader))
            {
                string path = Path.Combine(GameInstallDir, entry.RemoteName);

                if (!File.Exists(path))
                {
                    failedList.Add(entry);
                    continue;
                }

                await using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);

                if (stream.Length != entry.FileSize)
                {
                    failedList.Add(entry);
                    continue;
                }

                if (!Verify(stream, md5Alg, entry.Md5))
                {
                    failedList.Add(entry);
                }
            }

            foreach (PkgVersionEntry entry in failedList)
            {
                //TODO: re-download failed files
            }
        }

        private static bool VerifyMd5(Stream stream, string expectedHash)
        {
            using MD5 md5Alg = MD5.Create();
            return Verify(stream, md5Alg, expectedHash);
        }

        private static bool Verify(Stream stream, HashAlgorithm hashAlgorithm, string expectedHash)
        {
            stream.Seek(0, SeekOrigin.Begin);
            byte[] hash = hashAlgorithm.ComputeHash(stream);

            return Convert.ToHexString(hash).Equals(expectedHash, StringComparison.InvariantCultureIgnoreCase);
        }

        private static bool? NullableStringEquals(string? value, string equals) =>
            value == null ? null : value == equals;

        private static bool? ToBoolean(string? value) =>
            (value as IConvertible)?.ToBoolean(null);

        public class RegistrySetting<T>
        {
            public bool Updated { get; private set; }

            [AllowNull, MaybeNull]
            private T _value;

            [DisallowNull, NotNull]
            private readonly T _defaultValue;

            public RegistrySetting([AllowNull] T value, [DisallowNull] T defaultValue)
            {
                _value        = value;
                _defaultValue = defaultValue;

                if (value == null)
                {
                    Updated = true;
                }
            }

            public void SetValue([AllowNull] T value)
            {
                Updated = true;
                _value  = value;
            }

            [NotNull]
            public T Value => _value ?? _defaultValue;

            [return: NotNull]
            public static implicit operator T(RegistrySetting<T> setting) => setting.Value;
        }
    }
}