// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Text;
using System.Text.Json;
using Microsoft.Win32;

namespace GenshinLauncher.MiHoYoRegistry;

public class HonkaiRegistry : MiHoYoRegistry
{
    private const string KeyNameGraphicsSetting = "GENERAL_DATA_V2_PersonalGraphicsSetting_h906361411";
    private const string KeyNameScreenSetting   = "GENERAL_DATA_V2_ScreenSettingData_h1916288658";
    private const string KeyNameGameVersion     = "GENERAL_DATA_V2_ResourceDownloadVersion_h1528433916";

    public HonkaiRegistry(MiHoYoGameName miHoYoGameName, bool writable) : base(miHoYoGameName, writable) { }

    public bool TryGetGraphicsSetting(out JsonSettingGraphics graphicsSetting) =>
        TryGetFromBinary(KeyNameGraphicsSetting, out graphicsSetting);

    public void SetGraphicsSetting(JsonSettingGraphics graphicsSetting) =>
        SetAsBinary(KeyNameGraphicsSetting, graphicsSetting);

    public bool TryGetScreenSetting(out JsonSettingScreen screenSetting) =>
        TryGetFromBinary(KeyNameScreenSetting, out screenSetting);

    public void SetScreenSetting(JsonSettingScreen screenSetting) =>
        SetAsBinary(KeyNameScreenSetting, screenSetting);

    public bool TryGetGameVersion(out string gameVersion) =>
        TryGetFromBinary(KeyNameGameVersion, out gameVersion);

    public void SetGameVersion(string gameVersion) =>
        SetAsBinary(KeyNameGameVersion, gameVersion);

    protected bool TryGetFromBinary<T>(string name, out T value)
    {
        if (this.RegistryKey.GetValue(name) is byte[] bytes)
        {
            ReadOnlySpan<byte> span = new(bytes, 0, bytes.Length - 1); // Removes null separator

            if (typeof(T) == typeof(string))
            {
                value = (T)(object)Encoding.UTF8.GetString(span);
            }
            else
            {
                value = JsonSerializer.Deserialize<T>(span)!;
            }

            return true;
        }

        value = default(T);
        return false;
    }

    protected void SetAsBinary<T>(string name, T value)
    {
        if (value is not string str)
        {
            str = JsonSerializer.Serialize(value);
        }

        if (str[^1] != '\0')
        {
            str += '\0';
        }

        this.RegistryKey.SetValue(name, Encoding.UTF8.GetBytes(str), RegistryValueKind.Binary);
    }
}