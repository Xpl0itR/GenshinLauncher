// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Text;
using Microsoft.Win32;

namespace GenshinLauncher.MiHoYoRegistry
{
    public class HonkaiRegistry : MiHoYoRegistry
    {
        private const string KeyNameBengHuai     = "崩坏3";
        private const string KeyNameHonkaiTwHkMo = "崩壞3";
        private const string KeyNameHonkaiSea    = "Honkai Impact 3";
        private const string KeyNameHonkaiNaEu   = "Honkai Impact 3rd";
        private const string KeyNameHonkaiKr     = "붕괴3rd";

        private const string KeyNameGraphicsSetting = "GENERAL_DATA_V2_PersonalGraphicsSetting_h906361411";
        private const string KeyNameScreenSetting   = "GENERAL_DATA_V2_ScreenSettingData_h1916288658";
        private const string KeyNameGameVersion     = "GENERAL_DATA_V2_ResourceDownloadVersion_h1528433916";

        public HonkaiRegistry(bool writable) : base(KeyNameHonkaiNaEu, writable) { } //TODO: support other versions

        //TODO: deserialize JSON strings into objects
        public string? GraphicsSetting
        {
            get => GetStringFromBinary(KeyNameGraphicsSetting);
            set => SetBinaryFromString(KeyNameGraphicsSetting, value);
        }

        public string? ScreenSetting
        {
            get => GetStringFromBinary(KeyNameScreenSetting);
            set => SetBinaryFromString(KeyNameScreenSetting, value);
        }

        public string? GameVersion
        {
            get => GetStringFromBinary(KeyNameGameVersion);
            set => SetBinaryFromString(KeyNameGameVersion, value);
        }

        private string? GetStringFromBinary(string name) =>
            this.RegistryKey.GetValue(name) is byte[] bytes 
                ? Encoding.UTF8.GetString(bytes) // TODO: test if .TrimEnd('\0') is necessary for deserializing JSON
                : null;

        private void SetBinaryFromString(string name, string? value)
        {
            if (value == null)
            {
                this.RegistryKey.DeleteValue(name, false);
            }
            else
            {
                this.RegistryKey.SetValue(name, Encoding.UTF8.GetBytes(value), RegistryValueKind.Binary);
            }
        }
    }
}