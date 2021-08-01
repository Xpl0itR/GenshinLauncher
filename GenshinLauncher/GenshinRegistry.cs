// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;
using Microsoft.Win32;

namespace GenshinLauncher
{
    public class GenshinRegistry : IDisposable
    {
        private const string ResolutionWidthName = "Screenmanager Resolution Width_h182942802";
        public int ResolutionWidth
        {
            get => Convert.ToInt32(_genshinRegistryKey?.GetValue(ResolutionWidthName));
            set => _genshinRegistryKey?.SetValue(ResolutionWidthName, value, RegistryValueKind.DWord);
        }

        private const string ResolutionHeightName = "Screenmanager Resolution Height_h2627697771";
        public int ResolutionHeight
        {
            get => Convert.ToInt32(_genshinRegistryKey?.GetValue(ResolutionHeightName));
            set => _genshinRegistryKey?.SetValue(ResolutionHeightName, value, RegistryValueKind.DWord);
        }

        private const string FullscreenModeName = "Screenmanager Is Fullscreen mode_h3981298716";
        public bool FullscreenMode
        {
            get => Convert.ToBoolean(_genshinRegistryKey?.GetValue(FullscreenModeName));
            set => _genshinRegistryKey?.SetValue(FullscreenModeName, value, RegistryValueKind.DWord);
        }

        private readonly RegistryKey _genshinRegistryKey;

        public GenshinRegistry(bool writable)
        {
            _genshinRegistryKey = Registry.CurrentUser.OpenSubKey("Software\\miHoYo\\Genshin Impact", writable); //TODO: maybe handle this returning null instead of returning default values
        }

        void IDisposable.Dispose()
        {
            _genshinRegistryKey?.Dispose();
        }
    }
}