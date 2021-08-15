// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GenshinLauncher
{
    public class Launcher
    {
        private const string ExeNameGlobal = "GenshinImpact.exe";
        private const string ExeNameChina  = "YuanShen.exe";

        private readonly string  _bgDirectory;
        private readonly string  _launcherIniPath;
        private readonly string  _gameInstallDir;
        private readonly string  _backgroundFileName;
        private readonly string  _entryPoint;
        private readonly Process _genshinProcess;

        public string EntryPointPath  => Path.Combine(_gameInstallDir, _entryPoint);
        public string BackgroundPath  => Path.Combine(_bgDirectory, _backgroundFileName);
        public bool   ValidEntryPoint => _entryPoint is ExeNameGlobal or ExeNameChina;

        public bool CloseToTray    { get; set; }
        public bool BorderlessMode { get; set; }
        public bool ExitOnLaunch   { get; set; }

        public RegistrySetting<int?>  ResolutionWidth  { get; }
        public RegistrySetting<int?>  ResolutionHeight { get; }
        public RegistrySetting<bool?> FullscreenMode   { get; }
        public RegistrySetting<int?>  MonitorIndex     { get; }

        public Launcher()
        {
            _bgDirectory     = Path.Combine(AppContext.BaseDirectory, "bg");
            _launcherIniPath = Path.Combine(AppContext.BaseDirectory, "config.ini");

            LauncherConfig ini = File.Exists(_launcherIniPath)
                ? new LauncherConfig(_launcherIniPath)
                : new LauncherConfig();

            _gameInstallDir     = ini.GameInstallPath                     ?? string.Empty;
            _backgroundFileName = ini.GameDynamicBgName                   ?? string.Empty;
            _entryPoint         = ini.GameStartName                       ?? string.Empty;
            CloseToTray         = NullableStringEquals(ini.ExitType, "1") ?? false;
            BorderlessMode      = ToBoolean(ini.BorderlessMode)           ?? false;
            ExitOnLaunch        = ToBoolean(ini.ExitOnLaunch)             ?? false;

            using GenshinRegistry? genshinRegistry = ValidEntryPoint
                ? new GenshinRegistry(false, _entryPoint == ExeNameGlobal)
                : null;

            ResolutionWidth  = new RegistrySetting<int?>(genshinRegistry?.ResolutionWidth, 0);
            ResolutionHeight = new RegistrySetting<int?>(genshinRegistry?.ResolutionHeight, 0);
            FullscreenMode   = new RegistrySetting<bool?>(genshinRegistry?.FullscreenMode, true);
            MonitorIndex     = new RegistrySetting<int?>(genshinRegistry?.MonitorIndex, 0);

            _genshinProcess = new Process();
        }

        public void SaveLauncherConfig()
        {
            LauncherConfig ini = File.Exists(_launcherIniPath)
                ? new LauncherConfig(_launcherIniPath)
                : new LauncherConfig();

            ini.GameInstallPath   = _gameInstallDir;
            ini.GameDynamicBgName = _backgroundFileName;
            ini.GameStartName     = _entryPoint;
            ini.ExitType          = CloseToTray    ? "1"    : "2";
            ini.BorderlessMode    = BorderlessMode ? "true" : "false";
            ini.ExitOnLaunch      = ExitOnLaunch   ? "true" : "false";
            ini.WriteFile(_launcherIniPath);

            if (ValidEntryPoint)
            {
                using GenshinRegistry genshinRegistry = new GenshinRegistry(true, _entryPoint == ExeNameGlobal);

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

        public void StartGame()
        {
            string genshinProcessName = Path.GetFileNameWithoutExtension(_entryPoint);
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