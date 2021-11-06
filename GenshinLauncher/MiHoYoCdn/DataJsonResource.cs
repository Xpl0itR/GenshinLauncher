// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global, ClassNeverInstantiated.Global, UnusedMember.Global
#nullable disable

namespace GenshinLauncher.MiHoYoCdn;

public record DataJsonResource : IDataJson
{
    [JsonPropertyName("game")] public Game Game { get; init; }

    [JsonPropertyName("plugin")] public Plugin Plugin { get; init; }

    [JsonPropertyName("web_url")] public string WebUrl { get; init; }

    [JsonPropertyName("force_update")] public bool? ForceUpdate { get; init; }

    [JsonPropertyName("pre_download_game")]
    public PreDownloadGame PreDownloadGame { get; init; }

    [JsonPropertyName("deprecated_packages")]
    public Package[] DeprecatedPackages { get; init; }

    [JsonPropertyName("sdk")] public object Sdk { get; init; }
}

public record Game
{
    [JsonPropertyName("latest")] public Package Latest { get; init; }

    [JsonPropertyName("history")] public Package[] History { get; init; }

    [JsonPropertyName("diffs")] public Package[] Diffs { get; init; }

    [JsonPropertyName("version")] public string Version { get; init; }

    [JsonPropertyName("stat")] public string Stat { get; init; }

    [JsonPropertyName("msg")] public string Msg { get; init; }
}

public record Package
{
    [JsonPropertyName("name")] public string Name { get; init; }

    [JsonPropertyName("version")] public string Version { get; init; }

    [JsonPropertyName("path")] public string Path { get; init; }

    [JsonPropertyName("size")] public string Size { get; init; }

    [JsonPropertyName("md5")] public string Md5 { get; init; }

    [JsonPropertyName("entry")] public string Entry { get; init; }

    [JsonPropertyName("is_recommended_update")]
    public bool? IsRecommendedUpdate { get; init; }

    [JsonPropertyName("voice_packs")] public Package[] VoicePacks { get; init; }

    [JsonPropertyName("decompressed_path")]
    public string DecompressedPath { get; init; }

    [JsonPropertyName("language")] public string Language { get; init; }

    [JsonPropertyName("segments")] public object[] Segments { get; init; }
}

public record Plugin
{
    [JsonPropertyName("plugins")] public Package[] Plugins { get; init; }

    [JsonPropertyName("version")] public string Version { get; init; }
}

public record PreDownloadGame
{
    [JsonPropertyName("latest")] public Package Latest { get; init; }

    [JsonPropertyName("diffs")] public Package[] Diffs { get; init; }
}