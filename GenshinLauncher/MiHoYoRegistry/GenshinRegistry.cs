// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace GenshinLauncher.MiHoYoRegistry
{
    public class GenshinRegistry : MiHoYoRegistry
    {

        private const string KeyNameGenshin  = "Genshin Impact";
        private const string KeyNameYuanShen = "原神";

        public GenshinRegistry(bool writable, bool globalVersion) 
            : base(globalVersion ? KeyNameGenshin : KeyNameYuanShen, writable) { }
    }
}