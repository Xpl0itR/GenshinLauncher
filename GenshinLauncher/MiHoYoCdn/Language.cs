// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Globalization;

namespace GenshinLauncher.MiHoYoCdn;

public readonly struct Language
{
    private readonly string _code;

    private Language(string code) =>
        _code = code;

    public override string ToString() =>
        _code;

    public static implicit operator string(Language language) =>
        language._code;

    public static Language Chinese    => new("zh-cn");
    public static Language English    => new("en-us");
    public static Language French     => new("fr-fr");
    public static Language German     => new("de-de");
    public static Language Indonesian => new("id-id");
    public static Language Japanese   => new("ja-jp");
    public static Language Korean     => new("ko-kr");
    public static Language Portuguese => new("pt-pt");
    public static Language Russian    => new("ru-ru");
    public static Language Spanish    => new("es-es");
    public static Language Thai       => new("th-th");
    public static Language Vietnamese => new("vi-vn");

    public static Language FromCulture(CultureInfo cultureInfo) =>
        cultureInfo.TwoLetterISOLanguageName switch
        {
            "zh" => Chinese,
            "fr" => French,
            "de" => German,
            "id" => Indonesian,
            "ja" => Japanese,
            "ko" => Korean,
            "pt" => Portuguese,
            "ru" => Russian,
            "es" => Spanish,
            "th" => Thai,
            "vi" => Vietnamese,
            _    => English
        };
}