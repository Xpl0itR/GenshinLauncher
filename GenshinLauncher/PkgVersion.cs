// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenshinLauncher
{
    public class PkgVersion : List<PkgVersionEntry>
    {
        public PkgVersion(TextReader reader)
        {
            string? entryString;
            while ((entryString = reader.ReadLine()) != null)
            {
                PkgVersionEntry entry = JsonSerializer.Deserialize<PkgVersionEntry>(entryString)!;
                this.Add(entry);
            }
        }
    }

#nullable disable
    // ReSharper disable UnusedMember.Global
    public record PkgVersionEntry
    {
        [JsonPropertyName("remoteName")]
        public string RemoteName { get; init; }

        [JsonPropertyName("md5")]
        public string Md5 { get; init; }

        [JsonPropertyName("fileSize")]
        public int FileSize { get; init; }
    }
    // ReSharper restore UnusedMember.Global
#nullable restore
}