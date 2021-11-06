// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using Microsoft.Win32;

namespace GenshinLauncher.MiHoYoRegistry;

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
        RegistryKey = Registry.CurrentUser.OpenSubKey(keyName, writable)
                   ?? Registry.CurrentUser.CreateSubKey(keyName, writable);
    }

    public void Dispose() { RegistryKey.Dispose(); }

    public static MiHoYoRegistry New(MiHoYoGameName miHoYoGameName, bool writable)
    {
        if (miHoYoGameName == MiHoYoGameName.Genshin || miHoYoGameName == MiHoYoGameName.YuanShen)
        {
            return new GenshinRegistry(miHoYoGameName, writable);
        }

        if (miHoYoGameName == MiHoYoGameName.BengHuai || miHoYoGameName == MiHoYoGameName.HonkaiKr || miHoYoGameName == MiHoYoGameName.HonkaiNaEu || miHoYoGameName == MiHoYoGameName.HonkaiSea || miHoYoGameName == MiHoYoGameName.HonkaiTwHkMo)
        {
            return new HonkaiRegistry(miHoYoGameName, writable);
        }

        throw new ArgumentOutOfRangeException(miHoYoGameName);
    }

    public bool TryGetFullscreenMode(out bool fullscreenMode) =>
        TryGet(KeyNameFullscreenMode, out fullscreenMode);

    public void SetFullscreenMode(bool fullscreenMode) =>
        SetAsDWord(KeyNameFullscreenMode, fullscreenMode);

    public bool TryGetMonitorIndex(out int index) =>
        TryGet(KeyNameMonitorIndex, out index);

    public void SetMonitorIndex(int index) =>
        SetAsDWord(KeyNameMonitorIndex, index);

    public bool TryGetResolutionHeight(out int height) =>
        TryGet(KeyNameResolutionHeight, out height);

    public void SetResolutionHeight(int height) =>
        SetAsDWord(KeyNameResolutionHeight, height);

    public bool TryGetResolutionWidth(out int width) =>
        TryGet(KeyNameResolutionWidth, out width);

    public void SetResolutionWidth(int width) =>
        SetAsDWord(KeyNameResolutionWidth, width);

    protected bool TryGet<T>(string name, out T value)
    {
        object? obj = RegistryKey.GetValue(name);

        if (obj == null)
        {
            value = default(T);
            return false;
        }

        value = obj is T val ? val : (T)Convert.ChangeType(obj, typeof(T));
        return true;
    }

    protected void SetAsDWord(string name, object value) =>
        RegistryKey.SetValue(name, value, RegistryValueKind.DWord);
}