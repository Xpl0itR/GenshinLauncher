// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace GenshinLauncher.MiHoYoRegistry
{
    public class GenshinRegistry : MiHoYoRegistry
    { 
        public GenshinRegistry(MiHoYoGameName miHoYoGameName, bool writable) 
            : base(ValidateGameName(miHoYoGameName), writable) { }

        private static MiHoYoGameName ValidateGameName(MiHoYoGameName miHoYoGameName)
        {
            if (miHoYoGameName != MiHoYoGameName.Genshin && miHoYoGameName != MiHoYoGameName.YuanShen)
            {
                throw new ArgumentOutOfRangeException(miHoYoGameName);
            }

            return miHoYoGameName;
        }
    }
}