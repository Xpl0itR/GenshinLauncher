// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GenshinLauncher.MiHoYoApi
{
    public class MiHoYoApiClient : IDisposable
    {
        private const int LauncherIdBengHuai     = 4;
        private const int LauncherIdHonkaiTwHkMo = 3;
        private const int LauncherIdHonkaiSea    = 4;
        private const int LauncherIdHonkaiNaEu   = 5;
        private const int LauncherIdHonkaiKr     = 6;
        private const int LauncherIdGenshin      = 10;
        private const int LauncherIdYuanShen     = 18;
        private const int SubChannelIdGenshin    = 0;
        private const int SubChannelIdYuanShen   = 1;
        private const int SubChannelIdBengHuai   = 1;
        private const int ChannelId              = 1;

        private const string UrlApiOverseas      = "https://sdk-os-static.mihoyo.com";
        private const string UrlApiChina         = "https://sdk-static.mihoyo.com";
        private const string Honkai              = "/bh3_global";
        private const string BengHuai            = "/bh3_cn";
        private const string Genshin             = "/hk4e_global";
        private const string YuanShen            = "/hk4e_cn";
        private const string EndpointLauncherApi = "/mdk/launcher/api";
        private const string EndpointChangelog   = "/changelog";
        private const string EndpointContent     = "/content";
        private const string EndpointProtocol    = "/protocol";
        private const string EndpointResource    = "/resource";
        private const string ApiKeyHonkaiTwHkMo  = "";
        private const string ApiKeyHonkaiSea     = "";
        private const string ApiKeyHonkaiNaEu    = "";
        private const string ApiKeyHonkaiKr      = "";
        private const string ApiKeyBengHuai      = "";
        private const string ApiKeyGenshin       = "";
        private const string ApiKeyYuanShen      = "";

        private const string UrlContentHonkai    = UrlApiOverseas + Honkai   + EndpointLauncherApi + EndpointContent;
        private const string UrlContentBengHuai  = UrlApiChina    + BengHuai + EndpointLauncherApi + EndpointContent;
        private const string UrlContentGenshin   = UrlApiOverseas + Genshin  + EndpointLauncherApi + EndpointContent;
        private const string UrlContentYuanShen  = UrlApiChina    + YuanShen + EndpointLauncherApi + EndpointContent;
        private const string UrlResourceHonkai   = UrlApiOverseas + Honkai   + EndpointLauncherApi + EndpointResource;
        private const string UrlResourceBengHuai = UrlApiChina    + BengHuai + EndpointLauncherApi + EndpointResource;
        private const string UrlResourceGenshin  = UrlApiOverseas + Genshin  + EndpointLauncherApi + EndpointResource;
        private const string UrlResourceYuanShen = UrlApiChina    + YuanShen + EndpointLauncherApi + EndpointResource;

        private readonly HttpClient            _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public MiHoYoApiClient(HttpClient httpClient)
        {
            _httpClient            = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                IgnoreNullValues    = true,
                ReadCommentHandling = JsonCommentHandling.Skip
            };
        }

        public MiHoYoApiClient() : this(new HttpClient
        {
            DefaultRequestHeaders =
            {
                // ReSharper disable once StringLiteralTypo
                { "User-Agent", "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) QtWebEngine/5.12.5 Chrome/69.0.3497.128 Safari/537.36" }
            }
        }) { }

        public async Task Download(string url, Stream outStream, RangeHeaderValue? range = null, HashAlgorithm? hashAlgorithm = null, IProgress<double>? progress = null, CancellationToken cancellationToken = default)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Range = range;

            HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using Stream contentStream = await response.Content.OpenStreamAsync(cancellationToken);
            await contentStream.CopyToAsync(outStream, hashAlgorithm, progress, cancellationToken);
        }

        public Task<DataJsonContent> GetContentBengHuai() =>
            GetDataJson<DataJsonContent>(UrlContentBengHuai, ContentObject(LauncherIdBengHuai, Language.Chinese));

        public Task<DataJsonContent> GetContentYuanShen() =>
            GetDataJson<DataJsonContent>(UrlContentYuanShen, ContentObject(LauncherIdYuanShen, Language.Chinese));

        public Task<DataJsonContent> GetContentHonkaiTwHkMo(Language language) =>
            GetDataJson<DataJsonContent>(UrlContentHonkai, ContentObject(LauncherIdHonkaiTwHkMo, language));

        public Task<DataJsonContent> GetContentHonkaiSea(Language language) =>
            GetDataJson<DataJsonContent>(UrlContentHonkai, ContentObject(LauncherIdHonkaiSea, language));

        public Task<DataJsonContent> GetContentHonkaiNaEu(Language language) =>
            GetDataJson<DataJsonContent>(UrlContentHonkai, ContentObject(LauncherIdHonkaiNaEu, language));

        public Task<DataJsonContent> GetContentHonkaiKr(Language language) =>
            GetDataJson<DataJsonContent>(UrlContentHonkai, ContentObject(LauncherIdHonkaiKr, language));

        public Task<DataJsonContent> GetContentGenshin(Language language) =>
            GetDataJson<DataJsonContent>(UrlContentGenshin, ContentObject(LauncherIdGenshin, language));

        public Task<DataJsonResource> GetResourceHonkaiTwHkMo() =>
            GetDataJson<DataJsonResource>(UrlResourceHonkai, ResourceObject(ApiKeyHonkaiTwHkMo, LauncherIdHonkaiTwHkMo));

        public Task<DataJsonResource> GetResourceHonkaiSea() =>
            GetDataJson<DataJsonResource>(UrlResourceHonkai, ResourceObject(ApiKeyHonkaiSea, LauncherIdHonkaiSea));

        public Task<DataJsonResource> GetResourceHonkaiNaEu() =>
            GetDataJson<DataJsonResource>(UrlResourceHonkai, ResourceObject(ApiKeyHonkaiNaEu, LauncherIdHonkaiNaEu));

        public Task<DataJsonResource> GetResourceHonkaiKr() =>
            GetDataJson<DataJsonResource>(UrlResourceHonkai, ResourceObject(ApiKeyHonkaiKr, LauncherIdHonkaiKr));

        public Task<DataJsonResource> GetResourceBengHuai() =>
            GetDataJson<DataJsonResource>(UrlResourceBengHuai, ResourceObject(ApiKeyBengHuai, LauncherIdBengHuai, ChannelId, SubChannelIdBengHuai));

        public Task<DataJsonResource> GetResourceGenshin() =>
            GetDataJson<DataJsonResource>(UrlResourceGenshin, ResourceObject(ApiKeyGenshin, LauncherIdGenshin, ChannelId, SubChannelIdGenshin));

        public Task<DataJsonResource> GetResourceYuanShen() =>
            GetDataJson<DataJsonResource>(UrlResourceYuanShen, ResourceObject(ApiKeyYuanShen, LauncherIdYuanShen, ChannelId, SubChannelIdYuanShen));

        private async Task<T> GetDataJson<T>(string url, object obj) where T : IDataJson
        {
            JsonContent         content  = JsonContent.Create(obj, options: _jsonSerializerOptions);
            HttpResponseMessage response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            ResponseJson<T>? responseJson = await response.Content.ReadFromJsonAsync<ResponseJson<T>>();
            responseJson!.EnsureSuccessStatusCode();

            return responseJson.Data;
        }

        private static object ContentObject(int launcherId, Language language) => new
        {
            launcher_id = launcherId,
            language    = language.ToString(),
            filter_adv  = "false"
        };

        public static object ResourceObject(string apiKey, int launcherId, int? channelId = null, int? subChannelId = null) => new
        {
            key            = apiKey,
            launcher_id    = launcherId,
            channel_id     = channelId,
            sub_channel_id = subChannelId
        };

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}