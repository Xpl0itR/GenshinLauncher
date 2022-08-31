// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;

namespace GenshinLauncher.MiHoYoCdn;

// ReSharper disable ClassNeverInstantiated.Global, NotAccessedPositionalProperty.Global, SuggestBaseTypeForParameterInConstructor
public record DataJsonResource
(
    [property: JsonPropertyName("game")]                Game             Game,
    [property: JsonPropertyName("plugin")]              Plugin           Plugin,
    [property: JsonPropertyName("web_url")]             string           WebUrl,
    [property: JsonPropertyName("force_update")]        ForceUpdate?     ForceUpdate,
    [property: JsonPropertyName("pre_download_game")]   PreDownloadGame? PreDownloadGame,
    [property: JsonPropertyName("deprecated_packages")] Package[]        DeprecatedPackages,
    [property: JsonPropertyName("sdk")]                 object           Sdk
);

public record Game
(
    [property: JsonPropertyName("latest")]  Package   Latest,
    [property: JsonPropertyName("history")] Package[] History,
    [property: JsonPropertyName("diffs")]   Package[] Diffs,
    [property: JsonPropertyName("version")] string    Version,
    [property: JsonPropertyName("stat")]    string    Stat,
    [property: JsonPropertyName("msg")]     string    Msg
);

public record Package
(
    [property: JsonPropertyName("name")]                  string    Name,
    [property: JsonPropertyName("version")]               string    Version,
    [property: JsonPropertyName("path")]                  string    Path,
    [property: JsonPropertyName("size")]                  string    Size,
    [property: JsonPropertyName("md5")]                   string    Md5,
    [property: JsonPropertyName("entry")]                 string    Entry,
    [property: JsonPropertyName("is_recommended_update")] bool?     IsRecommendedUpdate,
    [property: JsonPropertyName("voice_packs")]           Package[] VoicePacks,
    [property: JsonPropertyName("decompressed_path")]     string    DecompressedPath,
    [property: JsonPropertyName("language")]              string    Language,
    [property: JsonPropertyName("segments")]              object[]  Segments
);

public record Plugin
(
    [property: JsonPropertyName("plugins")] Package[] Plugins,
    [property: JsonPropertyName("version")] string    Version
);

public record ForceUpdate(
    [property: JsonPropertyName("min_version")] string MinVersion,
    [property: JsonPropertyName("url")]         string Url
);

public record PreDownloadGame
(
    [property: JsonPropertyName("latest")] Package   Latest,
    [property: JsonPropertyName("diffs")]  Package[] Diffs
);