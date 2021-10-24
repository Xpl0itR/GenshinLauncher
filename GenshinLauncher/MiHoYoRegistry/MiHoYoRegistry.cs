// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using Microsoft.Win32;

namespace GenshinLauncher.MiHoYoRegistry
{
    public abstract class MiHoYoRegistry : IDisposable
    {
        private const string KeyNameFullscreenMode   = "Screenmanager Is Fullscreen mode_h3981298716";
        private const string KeyNameResolutionHeight = "Screenmanager Resolution Height_h2627697771";
        private const string KeyNameResolutionWidth  = "Screenmanager Resolution Width_h182942802";
        private const string KeyNameMonitorIndex     = "UnitySelectMonitor_h17969598";

        protected readonly RegistryKey RegistryKey;

        protected MiHoYoRegistry(MiHoYoGameName miHoYoGameName, bool writable)
        {
            string keyName = $"Software\\miHoYo\\{miHoYoGameName}";
            RegistryKey    = Registry.CurrentUser.OpenSubKey(keyName, writable)
                          ?? Registry.CurrentUser.CreateSubKey(keyName, writable);
        }

        public bool? FullscreenMode
        {
            get => GetBoolean(KeyNameFullscreenMode);
            set => SetDWord(KeyNameFullscreenMode, value);
        }

        public int? MonitorIndex
        {
            get => GetInt32(KeyNameMonitorIndex);
            set => SetDWord(KeyNameMonitorIndex, value);
        }

        public int? ResolutionHeight
        {
            get => GetInt32(KeyNameResolutionHeight);
            set => SetDWord(KeyNameResolutionHeight, value);
        }

        public int? ResolutionWidth
        {
            get => GetInt32(KeyNameResolutionWidth);
            set => SetDWord(KeyNameResolutionWidth, value);
        }

        protected bool? GetBoolean(string name) =>
            (RegistryKey.GetValue(name) as IConvertible)?.ToBoolean(null);

        protected int? GetInt32(string name) =>
            (RegistryKey.GetValue(name) as IConvertible)?.ToInt32(null);

        protected void SetDWord(string name, object? value)
        {
            if (value == null)
            {
                RegistryKey.DeleteValue(name, false);
            }
            else
            {
                RegistryKey.SetValue(name, value, RegistryValueKind.DWord);
            }
        }

        public void Dispose()
        {
            RegistryKey.Dispose();
        }
    }
}