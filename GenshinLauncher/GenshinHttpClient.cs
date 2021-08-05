// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GenshinLauncher
{
    public class GenshinHttpClient : IDisposable
    {
        private const int LauncherIdGlobal   = 10;
        private const int LauncherIdChina    = 18;
        private const int SubChannelIdGlobal = 0;
        private const int SubChannelIdChina  = 1;
        private const int ChannelId          = 1;

        private const string SdkUrlGlobal        = "https://sdk-os-static.mihoyo.com";
        private const string SdkUrlChina         = "https://sdk-static.mihoyo.com";
        private const string ApiUrlGlobal        = "https://api-os-takumi.mihoyo.com";
        private const string ApiUrlChina         = "https://api-sdk.mihoyo.com";
        private const string WebsiteUrlGlobal    = "https://genshin.mihoyo.com";
        private const string WebsiteUrlChina     = "https://ys.mihoyo.com";
        private const string Hk4EGlobalEndpoint  = "/hk4e_global";
        private const string Hk4EChinaEndpoint   = "/hk4e_cn";
        private const string LauncherApiEndpoint = "/mdk/launcher/api";
        private const string ContentEndpoint     = "/content";
        private const string ResourceEndpoint    = "/resource";
        private const string LauncherEndpoint    = "/launcher";
        private const string KeyGlobal           = "";
        private const string KeyChina            = "";

        private const string ContentUrlGlobal     = SdkUrlGlobal     + Hk4EGlobalEndpoint + LauncherApiEndpoint + ContentEndpoint;
        private const string ContentUrlChina      = SdkUrlChina      + Hk4EChinaEndpoint  + LauncherApiEndpoint + ContentEndpoint;
        private const string ResourceUrlGlobal    = SdkUrlGlobal     + Hk4EGlobalEndpoint + LauncherApiEndpoint + ResourceEndpoint;
        private const string ResourceUrlChina     = SdkUrlChina      + Hk4EChinaEndpoint  + LauncherApiEndpoint + ResourceEndpoint;
        private const string LauncherApiUrlGlobal = ApiUrlGlobal     + Hk4EGlobalEndpoint;
        private const string LauncherApiUrlChina  = ApiUrlChina      + Hk4EChinaEndpoint;
        private const string LauncherWebUrlGlobal = WebsiteUrlGlobal + LauncherEndpoint;
        private const string LauncherWebUrlChina  = WebsiteUrlChina  + LauncherEndpoint;

        private readonly HttpClient _httpClient;

        public GenshinHttpClient()
        {
            _httpClient = new HttpClient
            {
                DefaultRequestHeaders =
                {
                    { "User-Agent", "Mozilla/5.0 (Windows NT 6.2; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) QtWebEngine/5.12.5 Chrome/69.0.3497.128 Safari/537.36" }
                }
            };
        }

        public async Task<Content> GetContent(string language = "en-us") =>
            await GetData<Content>(await GetContentResponse(language));

        public async Task<Resource> GetResource(bool globalVersion = true) =>
            await GetData<Resource>(await GetResourceResponse(globalVersion));

        public async Task Download(string url, Stream outStream, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            await response.Content.CopyToAsync(outStream, cancellationToken);
        }

        private Task<HttpResponseMessage> GetLauncherHtmlResponse(bool globalVersion = true) =>
            _httpClient.GetAsync(globalVersion
                ? $"{LauncherWebUrlGlobal}/{LauncherIdGlobal}/en-us?api_url={LauncherApiUrlGlobal}&prev=false"
                : $"{LauncherWebUrlChina}/{LauncherIdChina}?api_url={LauncherApiUrlChina}&prev=false");

        private Task<HttpResponseMessage> GetContentResponse(string language)
        {
            string url = language == "zh-cn" ? ContentUrlChina : ContentUrlGlobal;
            var json = new
            {
                filter_adv = "true",
                language,
                launcher_id = language == "zh-cn" ? LauncherIdChina : LauncherIdGlobal
            };

            return _httpClient.PostAsync(url, JsonContent.Create(json));
        }

        private Task<HttpResponseMessage> GetResourceResponse(bool globalVersion)
        {
            string url = globalVersion ? ResourceUrlGlobal : ResourceUrlChina;
            var json = new
            {
                launcher_id    = globalVersion ? LauncherIdGlobal   : LauncherIdChina,
                key            = globalVersion ? KeyGlobal          : KeyChina,
                sub_channel_id = globalVersion ? SubChannelIdGlobal : SubChannelIdChina,
                channel_id     = ChannelId
            };

            return _httpClient.PostAsync(url, JsonContent.Create(json));
        }

        private static async Task<T> GetData<T>(HttpResponseMessage response) where T : Data
        {
            response.EnsureSuccessStatusCode();

            GenshinApiJson<T> resource = await response.Content.ReadFromJsonAsync<GenshinApiJson<T>>();
            resource!.EnsureSuccessRetCode();

            return resource.Data;
        }

        void IDisposable.Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}