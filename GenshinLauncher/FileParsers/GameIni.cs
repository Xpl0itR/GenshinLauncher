// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using IniParser.Model;

namespace GenshinLauncher.FileParsers
{
    public class GameIni : Ini
    {
        private const string GeneralSectionName = "General";
        private const string ChannelKeyName     = "channel";
        private const string CpsKeyName         = "cps";
        private const string GameVersionKeyName = "game_version";
        private const string SubChannelKeyName  = "sub_channel";
        private const string SdkVersionKeyName  = "sdk_version";

        private KeyDataCollection General => this.Data[GeneralSectionName];

        public GameIni(string? path = null) : base(path) { }

        public string? Channel
        {
            get => General[ChannelKeyName];
            set => General[ChannelKeyName] = value;
        }

        public string? GameVersion
        {
            get => General[GameVersionKeyName];
            set => General[GameVersionKeyName] = value;
        }

        public string? SubChannel
        {
            get => General[SubChannelKeyName];
            set => General[SubChannelKeyName] = value;
        }
    }
}