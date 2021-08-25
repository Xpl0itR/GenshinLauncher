// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;

namespace GenshinLauncher
{
    public class Setting<T>
    {
        public bool Updated { get; private set; }

        [AllowNull, MaybeNull]
        private T _value;

        [DisallowNull, NotNull]
        private readonly T _defaultValue;

        public Setting([AllowNull] T value, [DisallowNull] T defaultValue)
        {
            _value        = value;
            _defaultValue = defaultValue;

            if (value == null)
            {
                Updated = true;
            }
        }

        public void SetValue([AllowNull] T value)
        {
            Updated = true;
            _value  = value;
        }

        [NotNull]
        public T Value => _value ?? _defaultValue;

        [return: NotNull]
        public static implicit operator T(Setting<T> setting) => setting.Value;
    }
}