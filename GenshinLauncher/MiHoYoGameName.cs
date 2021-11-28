// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace GenshinLauncher;

public class MiHoYoGameName
{
    private const string NameBengHuai     = "崩坏3";
    private const string NameGenshin      = "Genshin Impact";
    private const string NameHonkaiKr     = "붕괴3rd";
    private const string NameHonkaiNaEu   = "Honkai Impact 3rd";
    private const string NameHonkaiSea    = "Honkai Impact 3";
    private const string NameHonkaiTwHkMo = "崩壞3";
    private const string NameYuanShen     = "原神";

    private static MiHoYoGameName? _nameBengHuai;
    private static MiHoYoGameName? _nameGenshin;
    private static MiHoYoGameName? _nameHonkaiKr;
    private static MiHoYoGameName? _nameHonkaiNaEu;
    private static MiHoYoGameName? _nameHonkaiSea;
    private static MiHoYoGameName? _nameHonkaiTwHkMo;
    private static MiHoYoGameName? _nameYuanShen;

    private readonly string _name;

    private MiHoYoGameName(string name) =>
        _name = name;

    public override string ToString() =>
        _name;

    public static  MiHoYoGameName  BengHuai     => _nameBengHuai     ??= new MiHoYoGameName(NameBengHuai);
    public static  MiHoYoGameName  Genshin      => _nameGenshin      ??= new MiHoYoGameName(NameGenshin);
    public static  MiHoYoGameName  HonkaiKr     => _nameHonkaiKr     ??= new MiHoYoGameName(NameHonkaiKr);
    public static  MiHoYoGameName  HonkaiNaEu   => _nameHonkaiNaEu   ??= new MiHoYoGameName(NameHonkaiNaEu);
    public static  MiHoYoGameName  HonkaiSea    => _nameHonkaiSea    ??= new MiHoYoGameName(NameHonkaiSea);
    public static  MiHoYoGameName  HonkaiTwHkMo => _nameHonkaiTwHkMo ??= new MiHoYoGameName(NameHonkaiTwHkMo);
    public static  MiHoYoGameName  YuanShen     => _nameYuanShen     ??= new MiHoYoGameName(NameYuanShen);

    public static implicit operator string(MiHoYoGameName miHoYoGameName) =>
        miHoYoGameName._name;

    public static MiHoYoGameName Parse(string? name) =>
        ParseInternal(name) ?? throw new ArgumentException();

    public static bool TryParse(string? name, out MiHoYoGameName? miHoYoGameName) =>
        (miHoYoGameName = ParseInternal(name)) != null;

    private static MiHoYoGameName? ParseInternal(string? name) =>
        name switch
        {
            NameBengHuai     => BengHuai,
            NameGenshin      => Genshin,
            NameHonkaiKr     => HonkaiKr,
            NameHonkaiNaEu   => HonkaiNaEu,
            NameHonkaiSea    => HonkaiSea,
            NameHonkaiTwHkMo => HonkaiTwHkMo,
            NameYuanShen     => YuanShen,
            _                => null
        };
}