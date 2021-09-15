// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace GenshinLauncher.FileParsers
{
    public class LauncherIni : Ini
    {
        private const string LauncherSectionName        = "launcher";
        private const string CpsKeyName                 = "cps";
        private const string ChannelKeyName             = "channel";
        private const string SubChannelKeyName          = "sub_channel";
        private const string GameInstallPathKeyName     = "game_install_path";
        private const string GameDynamicBgNameKeyName   = "game_dynamic_bg_name";
        private const string GameDynamicBgMd5KeyName    = "game_dynamic_bg_md5";
        private const string GameStartNameKeyName       = "game_start_name";
        private const string IsFirstExitKeyName         = "is_first_exit";
        private const string ExitTypeKeyName            = "exit_type";
        private const string SpeedLimitEnabledKeyName   = "speed_limit_enabled";

        private const string GenshinLauncherSectionName = "GenshinLauncher";
        private const string BorderlessModeKeyName      = "borderless_mode";
        private const string ExitOnLaunchKeyName        = "exit_on_launch";

        private IniParser.Model.KeyDataCollection Launcher        => this.Data[LauncherSectionName];
        private IniParser.Model.KeyDataCollection GenshinLauncher => this.Data[GenshinLauncherSectionName]; //TODO: rename after program name is finalized

        public LauncherIni(string? path = null) : base(path) { }

        public string? Channel
        {
            get => Launcher[ChannelKeyName];
            set => Launcher[ChannelKeyName] = value;
        }

        public string? SubChannel
        {
            get => Launcher[SubChannelKeyName];
            set => Launcher[SubChannelKeyName] = value;
        }

        public string? GameInstallPath
        {
            get => Launcher[GameInstallPathKeyName];
            set => Launcher[GameInstallPathKeyName] = value;
        }

        public string? GameDynamicBgName
        {
            get => Launcher[GameDynamicBgNameKeyName];
            set => Launcher[GameDynamicBgNameKeyName] = value;
        }

        public string? GameDynamicBgMd5
        {
            get => Launcher[GameDynamicBgMd5KeyName];
            set => Launcher[GameDynamicBgMd5KeyName] = value;
        }

        public string? GameStartName
        {
            get => Launcher[GameStartNameKeyName];
            set => Launcher[GameStartNameKeyName] = value;
        }

        public string? ExitType
        {
            get => Launcher[ExitTypeKeyName];
            set => Launcher[ExitTypeKeyName] = value;
        }

        public string? BorderlessMode
        {
            get => GenshinLauncher[BorderlessModeKeyName];
            set => GenshinLauncher[BorderlessModeKeyName] = value;
        }

        public string? ExitOnLaunch
        {
            get => GenshinLauncher[ExitOnLaunchKeyName];
            set => GenshinLauncher[ExitOnLaunchKeyName] = value;
        }
    }
}