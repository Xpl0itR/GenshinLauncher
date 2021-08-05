// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;
using System.Net.Http;
using System.Text.Json.Serialization;

namespace GenshinLauncher
{
    public abstract record Data;

    public record GenshinApiJson<T> where T : Data
    {
        [JsonPropertyName("retcode")]
        public int RetCode { get; init; }

        [JsonPropertyName("message")]
        public string Message { get; init; }

        [JsonPropertyName("data")]
        public T Data { get; init; }

        public void EnsureSuccessRetCode()
        {
            if (RetCode != 0)
            {
                throw new HttpRequestException($"Response status code does not indicate success: {RetCode} ({Message})");
            }
        }
    }

    public record Content : Data
    {
        [JsonPropertyName("adv")]
        public Adv Adv { get; init; }

        [JsonPropertyName("banner")]
        public Banner[] Banner { get; init; }

        [JsonPropertyName("icon")]
        public Icon[] Icon { get; init; }

        [JsonPropertyName("post")]
        public Post[] Post { get; init; }

        [JsonPropertyName("qq")]
        public object[] Qq { get; init; }

        [JsonPropertyName("more")]
        public More More { get; init; }

        [JsonPropertyName("links")]
        public Links Links { get; init; }
    }

    public record Adv
    {
        [JsonPropertyName("background")]
        public Uri Background { get; init; }

        [JsonPropertyName("icon")]
        public string Icon { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }

        [JsonPropertyName("version")]
        public string Version { get; init; }

        [JsonPropertyName("bg_checksum")]
        public string BgChecksum { get; init; }
    }

    public record Banner
    {
        [JsonPropertyName("banner_id")]
        public string BannerId { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("img")]
        public Uri Img { get; init; }

        [JsonPropertyName("url")]
        public Uri Url { get; init; }

        [JsonPropertyName("order")]
        public string Order { get; init; }
    }

    public record Icon
    {
        [JsonPropertyName("icon_id")]
        public string IconId { get; init; }

        [JsonPropertyName("img")]
        public Uri Img { get; init; }

        [JsonPropertyName("tittle")]
        public string Tittle { get; init; }

        [JsonPropertyName("url")]
        public Uri Url { get; init; }

        [JsonPropertyName("qr_img")]
        public string QrImg { get; init; }

        [JsonPropertyName("qr_desc")]
        public string QrDesc { get; init; }

        [JsonPropertyName("img_hover")]
        public Uri ImgHover { get; init; }

        [JsonPropertyName("other_links")]
        public OtherLink[] OtherLinks { get; init; }
    }

    public record OtherLink
    {
        [JsonPropertyName("title")]
        public string Title { get; init; }

        [JsonPropertyName("url")]
        public Uri Url { get; init; }
    }

    public record Links
    {
        [JsonPropertyName("faq")]
        public Uri Faq { get; init; }

        [JsonPropertyName("version")]
        public string Version { get; init; }
    }

    public record More
    {
        [JsonPropertyName("activity_link")]
        public string ActivityLink { get; init; }

        [JsonPropertyName("announce_link")]
        public string AnnounceLink { get; init; }

        [JsonPropertyName("info_link")]
        public string InfoLink { get; init; }
    }

    public record Post
    {
        [JsonPropertyName("post_id")]
        public string PostId { get; init; }

        [JsonPropertyName("type")]
        public string Type { get; init; }

        [JsonPropertyName("tittle")]
        public string Tittle { get; init; }

        [JsonPropertyName("url")]
        public Uri Url { get; init; }

        [JsonPropertyName("show_time")]
        public string ShowTime { get; init; }

        [JsonPropertyName("order")]
        public string Order { get; init; }
    }

    public record Resource : Data
    {
        [JsonPropertyName("game")]
        public Game Game { get; init; }

        [JsonPropertyName("plugin")]
        public Plugin Plugin { get; init; }

        [JsonPropertyName("web_url")]
        public Uri WebUrl { get; init; }

        [JsonPropertyName("force_update")]
        public bool? ForceUpdate { get; init; }

        [JsonPropertyName("pre_download_game")]
        public PreDownloadGame PreDownloadGame { get; init; }

        [JsonPropertyName("deprecated_packages")]
        public Package[] DeprecatedPackages { get; init; }

        [JsonPropertyName("sdk")]
        public object Sdk { get; init; }
    }

    public record Game
    {
        [JsonPropertyName("latest")]
        public Package Latest { get; init; }

        [JsonPropertyName("history")]
        public Package[] History { get; init; }

        [JsonPropertyName("diffs")]
        public Package[] Diffs { get; init; }

        [JsonPropertyName("version")]
        public string Version { get; init; }

        [JsonPropertyName("stat")]
        public string Stat { get; init; }

        [JsonPropertyName("msg")]
        public string Msg { get; init; }
    }

    public record Package
    {
        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("version")]
        public string Version { get; init; }

        [JsonPropertyName("path")]
        public Uri Path { get; init; }

        [JsonPropertyName("size")]
        public string Size { get; init; }

        [JsonPropertyName("md5")]
        public string Md5 { get; init; }

        [JsonPropertyName("entry")]
        public string Entry { get; init; }

        [JsonPropertyName("is_recommended_update")]
        public bool? IsRecommendedUpdate { get; init; }

        [JsonPropertyName("voice_packs")]
        public Package[] VoicePacks { get; init; }

        [JsonPropertyName("decompressed_path")]
        public string DecompressedPath { get; init; }

        [JsonPropertyName("language")]
        public string Language { get; init; }
    }

    public record Plugin
    {
        [JsonPropertyName("plugins")]
        public Package[] Plugins { get; init; }

        [JsonPropertyName("version")]
        public string Version { get; init; }
    }

    public record PreDownloadGame
    {
        [JsonPropertyName("latest")]
        public Package Latest { get; init; }

        [JsonPropertyName("diffs")]
        public Package[] Diffs { get; init; }
    }
}