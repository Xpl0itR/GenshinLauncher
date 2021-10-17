// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Globalization;

namespace GenshinLauncher.MiHoYoApi
{
    public readonly struct Language
    {
        private readonly string _code;

        private Language(string code)
        {
            _code = code;
        }

        public override string ToString() => _code;

        public static Language Chinese    => new Language("zh-cn");
        public static Language English    => new Language("en-us");
        public static Language French     => new Language("fr-fr");
        public static Language German     => new Language("de-de");
        public static Language Indonesian => new Language("id-id");
        public static Language Japanese   => new Language("ja-jp");
        public static Language Korean     => new Language("ko-kr");
        public static Language Portuguese => new Language("pt-pt");
        public static Language Russian    => new Language("ru-ru");
        public static Language Spanish    => new Language("es-es");
        public static Language Thai       => new Language("th-th");
        public static Language Vietnamese => new Language("vi-vn");

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
}