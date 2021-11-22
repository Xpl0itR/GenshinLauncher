// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using IniParser.Model;

namespace GenshinLauncher.FileParsers;

// ReSharper disable UnusedMember.Global, UnusedMember.Local
public class LauncherIni : Ini
{
    private const string SectionNameLauncher      = "launcher";
    private const string KeyNameCps               = "cps";
    private const string KeyNameChannel           = "channel";
    private const string KeyNameSubChannel        = "sub_channel";
    private const string KeyNameGameInstallPath   = "game_install_path";
    private const string KeyNameGameDynamicBgName = "game_dynamic_bg_name";
    private const string KeyNameGameDynamicBgMd5  = "game_dynamic_bg_md5";
    private const string KeyNameGameStartName     = "game_start_name";
    private const string KeyNameIsFirstExit       = "is_first_exit";
    private const string KeyNameExitType          = "exit_type";
    private const string KeyNameSpeedLimitEnabled = "speed_limit_enabled";
    private const string KeyNameBorderlessMode    = "borderless_mode";
    private const string KeyNameExitOnLaunch      = "exit_on_launch";

    public LauncherIni(string? path = null) : base(path) { }

    private KeyDataCollection SectionLauncher => this.Data[SectionNameLauncher];
    private KeyDataCollection SectionCustom   => this.Data[nameof(GenshinLauncher)];

    public string? Channel
    {
        get => SectionLauncher[KeyNameChannel];
        set => SectionLauncher[KeyNameChannel] = value;
    }

    public string? SubChannel
    {
        get => SectionLauncher[KeyNameSubChannel];
        set => SectionLauncher[KeyNameSubChannel] = value;
    }

    public string? GameInstallPath
    {
        get => SectionLauncher[KeyNameGameInstallPath];
        set => SectionLauncher[KeyNameGameInstallPath] = value;
    }

    public string? GameDynamicBgName
    {
        get => SectionLauncher[KeyNameGameDynamicBgName];
        set => SectionLauncher[KeyNameGameDynamicBgName] = value;
    }

    public string? GameDynamicBgMd5
    {
        get => SectionLauncher[KeyNameGameDynamicBgMd5];
        set => SectionLauncher[KeyNameGameDynamicBgMd5] = value;
    }

    public string? GameStartName
    {
        get => SectionLauncher[KeyNameGameStartName];
        set => SectionLauncher[KeyNameGameStartName] = value;
    }

    public string? ExitType
    {
        get => SectionLauncher[KeyNameExitType];
        set => SectionLauncher[KeyNameExitType] = value;
    }

    public string? BorderlessMode
    {
        get => SectionCustom[KeyNameBorderlessMode];
        set => SectionCustom[KeyNameBorderlessMode] = value;
    }

    public string? ExitOnLaunch
    {
        get => SectionCustom[KeyNameExitOnLaunch];
        set => SectionCustom[KeyNameExitOnLaunch] = value;
    }
}