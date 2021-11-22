// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;

namespace GenshinLauncher.MiHoYoCdn;

// ReSharper disable UnusedAutoPropertyAccessor.Global, ClassNeverInstantiated.Global, UnusedMember.Global
#nullable disable

public record DataJsonContent : IDataJson
{
    [JsonPropertyName("adv")] public Adv Adv { get; init; }

    [JsonPropertyName("banner")] public Banner[] Banner { get; init; }

    [JsonPropertyName("icon")] public Icon[] Icon { get; init; }

    [JsonPropertyName("post")] public Post[] Post { get; init; }

    [JsonPropertyName("qq")] public object[] Qq { get; init; }

    [JsonPropertyName("more")] public More More { get; init; }

    [JsonPropertyName("links")] public Links Links { get; init; }
}

public record Adv
{
    [JsonPropertyName("background")] public string Background { get; init; }

    [JsonPropertyName("icon")] public string Icon { get; init; }

    [JsonPropertyName("url")] public string Url { get; init; }

    [JsonPropertyName("version")] public string Version { get; init; }

    [JsonPropertyName("bg_checksum")] public string BgChecksum { get; init; }
}

public record Banner
{
    [JsonPropertyName("banner_id")] public string BannerId { get; init; }

    [JsonPropertyName("name")] public string Name { get; init; }

    [JsonPropertyName("img")] public string Img { get; init; }

    [JsonPropertyName("url")] public string Url { get; init; }

    [JsonPropertyName("order")] public string Order { get; init; }
}

public record Icon
{
    [JsonPropertyName("icon_id")] public string IconId { get; init; }

    [JsonPropertyName("img")] public string Img { get; init; }

    [JsonPropertyName("tittle")] public string Tittle { get; init; }

    [JsonPropertyName("url")] public string Url { get; init; }

    [JsonPropertyName("qr_img")] public string QrImg { get; init; }

    [JsonPropertyName("qr_desc")] public string QrDesc { get; init; }

    [JsonPropertyName("img_hover")] public string ImgHover { get; init; }

    [JsonPropertyName("other_links")] public OtherLink[] OtherLinks { get; init; }
}

public record OtherLink
{
    [JsonPropertyName("title")] public string Title { get; init; }

    [JsonPropertyName("url")] public string Url { get; init; }
}

public record Links
{
    [JsonPropertyName("faq")] public string Faq { get; init; }

    [JsonPropertyName("version")] public string Version { get; init; }
}

public record More
{
    [JsonPropertyName("activity_link")] public string ActivityLink { get; init; }

    [JsonPropertyName("announce_link")] public string AnnounceLink { get; init; }

    [JsonPropertyName("info_link")] public string InfoLink { get; init; }
}

public record Post
{
    [JsonPropertyName("post_id")] public string PostId { get; init; }

    [JsonPropertyName("type")] public string Type { get; init; }

    [JsonPropertyName("tittle")] public string Tittle { get; init; }

    [JsonPropertyName("url")] public string Url { get; init; }

    [JsonPropertyName("show_time")] public string ShowTime { get; init; }

    [JsonPropertyName("order")] public string Order { get; init; }
}