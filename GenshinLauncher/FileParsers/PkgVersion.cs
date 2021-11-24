// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GenshinLauncher.FileParsers;

public class PkgVersion : List<PkgVersionEntry>
{
    public PkgVersion(TextReader reader)
    {
        string? entryString;
        while ((entryString = reader.ReadLine()) != null)
        {
            PkgVersionEntry? entry = JsonSerializer.Deserialize<PkgVersionEntry>(entryString);
            this.Add(entry!);
        }
    }
}

public record PkgVersionEntry
(
    [property: JsonPropertyName("remoteName")] string RemoteName,
    [property: JsonPropertyName("md5")]        string Md5,
    [property: JsonPropertyName("fileSize")]   int    FileSize
);