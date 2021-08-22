// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Net.Http;
using System.Text.Json.Serialization;

#nullable disable
namespace GenshinLauncher.MiHoYoApi
{
    public abstract record Data;

    public record ResponseJson<T> where T : Data
    {
        // ReSharper disable once StringLiteralTypo
        [JsonPropertyName("retcode")]
        public int StatusCode { get; init; }

        [JsonPropertyName("message")]
        public string StatusMessage { get; init; }

        [JsonPropertyName("data")]
        public T Data { get; init; }

        public void EnsureSuccessStatusCode()
        {
            if (StatusCode != 0)
            {
                throw new HttpRequestException($"Response body status code does not indicate success: {StatusCode} ({StatusMessage})");
            }
        }
    }
}