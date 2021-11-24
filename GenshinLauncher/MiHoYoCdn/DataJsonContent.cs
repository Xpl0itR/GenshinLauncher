// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;

namespace GenshinLauncher.MiHoYoCdn;

// ReSharper disable ClassNeverInstantiated.Global, NotAccessedPositionalProperty.Global, SuggestBaseTypeForParameterInConstructor
public record DataJsonContent
(
    [property: JsonPropertyName("adv")]    Adv      Adv,
    [property: JsonPropertyName("banner")] Banner[] Banner,
    [property: JsonPropertyName("icon")]   Icon[]   Icon,
    [property: JsonPropertyName("post")]   Post[]   Post,
    [property: JsonPropertyName("qq")]     object[] Qq,
    [property: JsonPropertyName("more")]   More     More,
    [property: JsonPropertyName("links")]  Links    Links
);

public record Adv
(
    [property: JsonPropertyName("background")]  string Background,
    [property: JsonPropertyName("icon")]        string Icon,
    [property: JsonPropertyName("url")]         string Url,
    [property: JsonPropertyName("version")]     string Version,
    [property: JsonPropertyName("bg_checksum")] string BgChecksum
);

public record Banner
(
    [property: JsonPropertyName("banner_id")] string BannerId,
    [property: JsonPropertyName("name")]      string Name,
    [property: JsonPropertyName("img")]       string Img,
    [property: JsonPropertyName("url")]       string Url,
    [property: JsonPropertyName("order")]     string Order
);

public record Icon
(
    [property: JsonPropertyName("icon_id")]     string      IconId,
    [property: JsonPropertyName("img")]         string      Img,
    [property: JsonPropertyName("tittle")]      string      Title,
    [property: JsonPropertyName("url")]         string      Url,
    [property: JsonPropertyName("qr_img")]      string      QrImg,
    [property: JsonPropertyName("qr_desc")]     string      QrDesc,
    [property: JsonPropertyName("img_hover")]   string      ImgHover,
    [property: JsonPropertyName("other_links")] OtherLink[] OtherLinks
);

public record OtherLink
(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("url")]   string Url
);

public record Links
(
    [property: JsonPropertyName("faq")]     string Faq,
    [property: JsonPropertyName("version")] string Version
);

public record More
(
    [property: JsonPropertyName("activity_link")] string ActivityLink,
    [property: JsonPropertyName("announce_link")] string AnnounceLink,
    [property: JsonPropertyName("info_link")]     string InfoLink
);

public record Post
(
    [property: JsonPropertyName("post_id")]   string PostId,
    [property: JsonPropertyName("type")]      string Type,
    [property: JsonPropertyName("tittle")]    string Title,
    [property: JsonPropertyName("url")]       string Url,
    [property: JsonPropertyName("show_time")] string ShowTime,
    [property: JsonPropertyName("order")]     string Order
);