// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace GenshinLauncher.FileParsers
{
    public class GameIni : Ini
    {
        private const string SectionNameGeneral = "General";
        private const string KeyNameChannel     = "channel";
        private const string KeyNameCps         = "cps";
        private const string KeyNameGameVersion = "game_version";
        private const string KeyNameSubChannel  = "sub_channel";
        private const string KeyNameSdkVersion  = "sdk_version";

        private IniParser.Model.KeyDataCollection General => this.Data[SectionNameGeneral];

        public GameIni(string? path = null) : base(path) { }

        public string? Channel
        {
            get => General[KeyNameChannel];
            set => General[KeyNameChannel] = value;
        }

        public string? GameVersion
        {
            get => General[KeyNameGameVersion];
            set => General[KeyNameGameVersion] = value;
        }

        public string? SubChannel
        {
            get => General[KeyNameSubChannel];
            set => General[KeyNameSubChannel] = value;
        }
    }
}