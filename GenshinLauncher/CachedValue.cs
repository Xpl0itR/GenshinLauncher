// Copyright © 2021 Xpl0itR
// 
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace GenshinLauncher;

public record CachedValue<T>
{
    private readonly DateTime _expiry;

    public CachedValue(T value, double ttl, object? state = null)
    {
        _expiry = DateTime.UtcNow.AddSeconds(ttl);
        Value   = value;
        State   = state;
    }

    public bool    Expired => DateTime.UtcNow > _expiry;
    public T       Value   { get; }
    public object? State   { get; }

    public static implicit operator T(CachedValue<T> cachedValue) =>
        cachedValue.Value;
}