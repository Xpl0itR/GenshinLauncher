// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using IniParser.Model;

namespace GenshinLauncher.FileParsers;

// ReSharper disable UnusedMember.Global, UnusedMember.Local
public class GameIni : Ini
{
    private const string SectionNameGeneral = "General";
    private const string KeyNameChannel     = "channel";
    private const string KeyNameCps         = "cps";
    private const string KeyNameGameVersion = "game_version";
    private const string KeyNameSubChannel  = "sub_channel";
    private const string KeyNameSdkVersion  = "sdk_version";

    public GameIni(string? path = null) : base(path) { }

    private KeyDataCollection SectionGeneral => this.Data[SectionNameGeneral];

    public string? Channel
    {
        get => SectionGeneral[KeyNameChannel];
        set => SectionGeneral[KeyNameChannel] = value;
    }

    public string? GameVersion
    {
        get => SectionGeneral[KeyNameGameVersion];
        set => SectionGeneral[KeyNameGameVersion] = value;
    }

    public string? SubChannel
    {
        get => SectionGeneral[KeyNameSubChannel];
        set => SectionGeneral[KeyNameSubChannel] = value;
    }
}