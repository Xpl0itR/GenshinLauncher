// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using Microsoft.Win32;

namespace GenshinLauncher
{
    public class GenshinRegistry : IDisposable
    {
        private const string FullscreenModeKeyName   = "Screenmanager Is Fullscreen mode_h3981298716";
        private const string ResolutionHeightKeyName = "Screenmanager Resolution Height_h2627697771";
        private const string ResolutionWidthKeyName  = "Screenmanager Resolution Width_h182942802";
        private const string MonitorIndexKeyName     = "UnitySelectMonitor_h17969598";
        private const string MiHoYoKeyName           = "Software\\miHoYo\\";
        private const string GenshinReleaseKeyName   = MiHoYoKeyName + "Genshin Impact";
        private const string YuanshenReleaseKeyName  = MiHoYoKeyName + "原神";

        private readonly RegistryKey _genshinRegistryKey;

        public GenshinRegistry(bool writable, bool globalVersion)
        {
            string keyName = globalVersion ? GenshinReleaseKeyName : YuanshenReleaseKeyName;
            _genshinRegistryKey = Registry.CurrentUser.OpenSubKey(keyName, writable) ?? Registry.CurrentUser.CreateSubKey(keyName, writable);
        }

        public bool? FullscreenMode
        {
            get => GetBoolean(FullscreenModeKeyName);
            set => SetDWord(FullscreenModeKeyName, value);
        }

        public int? ResolutionHeight
        {
            get => GetInt32(ResolutionHeightKeyName);
            set => SetDWord(ResolutionHeightKeyName, value);
        }

        public int? ResolutionWidth
        {
            get => GetInt32(ResolutionWidthKeyName);
            set => SetDWord(ResolutionWidthKeyName, value);
        }

        public int? MonitorIndex
        {
            get => GetInt32(MonitorIndexKeyName);
            set => SetDWord(MonitorIndexKeyName, value);
        }

        private bool? GetBoolean(string name) =>
            (_genshinRegistryKey.GetValue(name) as IConvertible)?.ToBoolean(null);

        private int? GetInt32(string name) =>
            (_genshinRegistryKey.GetValue(name) as IConvertible)?.ToInt32(null);

        private void SetDWord(string name, object? value)
        {
            if (value == null)
            {
                _genshinRegistryKey.DeleteValue(name, false);
            }
            else
            {
                _genshinRegistryKey.SetValue(name, value, RegistryValueKind.DWord);
            }
        }

        public void Dispose()
        {
            _genshinRegistryKey.Dispose();
        }
    }
}