// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace GenshinLauncher.FileParsers
{
    public class LauncherIni : Ini
    {
        private const string SectionNameLauncher        = "launcher";
        private const string KeyNameCps                 = "cps";
        private const string KeyNameChannel             = "channel";
        private const string KeyNameSubChannel          = "sub_channel";
        private const string KeyNameGameInstallPath     = "game_install_path";
        private const string KeyNameGameDynamicBgName   = "game_dynamic_bg_name";
        private const string KeyNameGameDynamicBgMd5    = "game_dynamic_bg_md5";
        private const string KeyNameGameStartName       = "game_start_name";
        private const string KeyNameIsFirstExit         = "is_first_exit";
        private const string KeyNameExitType            = "exit_type";
        private const string KeyNameSpeedLimitEnabled   = "speed_limit_enabled";

        private const string SectionNameGenshinLauncher = "GenshinLauncher"; //TODO: rename after program name is finalized
        private const string KeyNameBorderlessMode      = "borderless_mode";
        private const string KeyNameExitOnLaunch        = "exit_on_launch";

        private IniParser.Model.KeyDataCollection Launcher        => this.Data[SectionNameLauncher];
        private IniParser.Model.KeyDataCollection GenshinLauncher => this.Data[SectionNameGenshinLauncher];

        public LauncherIni(string? path = null) : base(path) { }

        public string? Channel
        {
            get => Launcher[KeyNameChannel];
            set => Launcher[KeyNameChannel] = value;
        }

        public string? SubChannel
        {
            get => Launcher[KeyNameSubChannel];
            set => Launcher[KeyNameSubChannel] = value;
        }

        public string? GameInstallPath
        {
            get => Launcher[KeyNameGameInstallPath];
            set => Launcher[KeyNameGameInstallPath] = value;
        }

        public string? GameDynamicBgName
        {
            get => Launcher[KeyNameGameDynamicBgName];
            set => Launcher[KeyNameGameDynamicBgName] = value;
        }

        public string? GameDynamicBgMd5
        {
            get => Launcher[KeyNameGameDynamicBgMd5];
            set => Launcher[KeyNameGameDynamicBgMd5] = value;
        }

        public string? GameStartName
        {
            get => Launcher[KeyNameGameStartName];
            set => Launcher[KeyNameGameStartName] = value;
        }

        public string? ExitType
        {
            get => Launcher[KeyNameExitType];
            set => Launcher[KeyNameExitType] = value;
        }

        public string? BorderlessMode
        {
            get => GenshinLauncher[KeyNameBorderlessMode];
            set => GenshinLauncher[KeyNameBorderlessMode] = value;
        }

        public string? ExitOnLaunch
        {
            get => GenshinLauncher[KeyNameExitOnLaunch];
            set => GenshinLauncher[KeyNameExitOnLaunch] = value;
        }
    }
}