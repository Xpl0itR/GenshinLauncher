// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;
using Microsoft.Win32;

namespace GenshinLauncher
{
    public class GenshinRegistry : IDisposable
    {
        private const string MiHoYoKeyName        = "Software\\miHoYo\\";
        private const string GlobalReleaseKeyName = MiHoYoKeyName + "Genshin Impact";
        private const string ChinaReleaseKeyName  = MiHoYoKeyName + "原神";

        private const string FullscreenModeKeyName = "Screenmanager Is Fullscreen mode_h3981298716";
        public bool FullscreenMode
        {
            get => Convert.ToBoolean(_genshinRegistryKey?.GetValue(FullscreenModeKeyName));
            set => _genshinRegistryKey?.SetValue(FullscreenModeKeyName, value, RegistryValueKind.DWord);
        }

        private const string ResolutionHeightKeyName = "Screenmanager Resolution Height_h2627697771";
        public int ResolutionHeight
        {
            get => Convert.ToInt32(_genshinRegistryKey?.GetValue(ResolutionHeightKeyName));
            set => _genshinRegistryKey?.SetValue(ResolutionHeightKeyName, value, RegistryValueKind.DWord);
        }

        private const string ResolutionWidthKeyName = "Screenmanager Resolution Width_h182942802";
        public int ResolutionWidth
        {
            get => Convert.ToInt32(_genshinRegistryKey?.GetValue(ResolutionWidthKeyName));
            set => _genshinRegistryKey?.SetValue(ResolutionWidthKeyName, value, RegistryValueKind.DWord);
        }

        private const string MonitorIndexKeyName = "UnitySelectMonitor_h17969598";
        public int MonitorIndex
        {
            get => Convert.ToInt32(_genshinRegistryKey?.GetValue(MonitorIndexKeyName));
            set => _genshinRegistryKey?.SetValue(MonitorIndexKeyName, value, RegistryValueKind.DWord);
        }

        private readonly RegistryKey _genshinRegistryKey;

        public GenshinRegistry(bool writable, bool globalVersion = true)
        {
            _genshinRegistryKey = Registry.CurrentUser.OpenSubKey(globalVersion ? GlobalReleaseKeyName : ChinaReleaseKeyName, writable); //TODO: maybe handle this returning null instead of returning default values
        }

        void IDisposable.Dispose()
        {
            _genshinRegistryKey?.Dispose();
        }
    }
}