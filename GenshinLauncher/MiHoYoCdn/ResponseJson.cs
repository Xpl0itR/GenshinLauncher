// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Net.Http;
using System.Text.Json.Serialization;

namespace GenshinLauncher.MiHoYoCdn;

// ReSharper disable once ClassNeverInstantiated.Global
public record ResponseJson<T>
( // ReSharper disable once StringLiteralTypo
    [property: JsonPropertyName("retcode")] int    StatusCode,
    [property: JsonPropertyName("message")] string StatusMessage,
    [property: JsonPropertyName("data")]    T      Data
)
{
    public void EnsureSuccessStatusCode()
    {
        if (StatusCode != 0)
        {
            throw new HttpRequestException($"Response body status code does not indicate success: {StatusCode} ({StatusMessage})");
        }
    }
}