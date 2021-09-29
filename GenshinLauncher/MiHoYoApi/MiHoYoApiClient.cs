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
using System.Threading;
using System.Threading.Tasks;

namespace GenshinLauncher.MiHoYoApi
{
    public class MiHoYoApiClient : IDisposable
    {
        private const int LauncherIdGenshin    = 10;
        private const int LauncherIdYuanshen   = 18;
        private const int SubChannelIdGenshin  = 0;
        private const int SubChannelIdYuanshen = 1;
        private const int ChannelId            = 1;

        private const string ApiUrlGlobal        = "https://sdk-os-static.mihoyo.com/hk4e_global";
        private const string ApiUrlChina         = "https://sdk-static.mihoyo.com/hk4e_cn";
        private const string LauncherApiEndpoint = "/mdk/launcher/api";
        private const string ContentEndpoint     = "/content";
        private const string ResourceEndpoint    = "/resource";
        private const string ApiKeyGlobal        = "";
        private const string ApiKeyChina         = "";

        private const string ContentUrlGlobal  = ApiUrlGlobal + LauncherApiEndpoint + ContentEndpoint;
        private const string ContentUrlChina   = ApiUrlChina  + LauncherApiEndpoint + ContentEndpoint;
        private const string ResourceUrlGlobal = ApiUrlGlobal + LauncherApiEndpoint + ResourceEndpoint;
        private const string ResourceUrlChina  = ApiUrlChina  + LauncherApiEndpoint + ResourceEndpoint;

        private readonly HttpClient _httpClient;

        public MiHoYoApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<ContentJson> GetContent(bool globalVersion, Language language) =>
            await GetData<ContentJson>(await GetContentResponse(globalVersion, language));

        public async Task<ResourceJson> GetResource(bool globalVersion) =>
            await GetData<ResourceJson>(await GetResourceResponse(globalVersion));

        private Task<HttpResponseMessage> GetContentResponse(bool globalVersion, Language language)
        {
            string url = globalVersion ? ContentUrlGlobal : ContentUrlChina;
            var json = new
            {
                language    = language.ToString(),
                filter_adv  = "false",
                launcher_id = globalVersion ? LauncherIdGenshin : LauncherIdYuanshen
            };

            return _httpClient.PostAsync(url, JsonContent.Create(json));
        }

        private Task<HttpResponseMessage> GetResourceResponse(bool globalVersion)
        {
            string url = globalVersion ? ResourceUrlGlobal : ResourceUrlChina;
            var json = new
            {
                launcher_id    = globalVersion ? LauncherIdGenshin   : LauncherIdYuanshen,
                key            = globalVersion ? ApiKeyGlobal        : ApiKeyChina,
                sub_channel_id = globalVersion ? SubChannelIdGenshin : SubChannelIdYuanshen,
                channel_id     = ChannelId
            };

            return _httpClient.PostAsync(url, JsonContent.Create(json));
        }

        private static async Task<T> GetData<T>(HttpResponseMessage response) where T : IDataJson
        {
            response.EnsureSuccessStatusCode();

            ResponseJson<T>? responseJson = await response.Content.ReadFromJsonAsync<ResponseJson<T>>();
            responseJson!.EnsureSuccessStatusCode();

            return responseJson.Data;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}