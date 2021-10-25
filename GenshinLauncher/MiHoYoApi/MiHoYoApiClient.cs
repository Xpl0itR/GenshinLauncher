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

        public async Task Download(string url, Stream outStream, RangeHeaderValue? range = null, HashAlgorithm? hashAlgorithm = null, IProgress<double>? progress = null, CancellationToken cancellationToken = default)
        { //TODO: move this to dedicated downloading class
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Range = range;

            HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using Stream contentStream = await response.Content.OpenStreamAsync(cancellationToken);
            await contentStream.CopyToAsync(outStream, hashAlgorithm, progress, cancellationToken);
        }

        public Task<DataJsonContent> GetContent(MiHoYoGameName gameName, Language language, CancellationToken cancellationToken = default)
        {
            string url;
            object obj;

            if (gameName == MiHoYoGameName.BengHuai)
            {
                url = UrlContentBengHuai;
                obj = new { launcher_id = LauncherIdBengHuai, language = Language.Chinese, filter_adv = "false" };
            }
            else if (gameName == MiHoYoGameName.Genshin)
            {
                url = UrlContentGenshin;
                obj = new { launcher_id = LauncherIdGenshin, language = language.ToString(), filter_adv = "false" };
            }
            else if (gameName == MiHoYoGameName.HonkaiKr)
            {
                url = UrlContentHonkai;
                obj = new { launcher_id = LauncherIdHonkaiKr, language = language.ToString(), filter_adv = "false" };
            }
            else if (gameName == MiHoYoGameName.HonkaiNaEu)
            {
                url = UrlContentHonkai;
                obj = new { launcher_id = LauncherIdHonkaiNaEu, language = language.ToString(), filter_adv = "false" };
            }
            else if (gameName == MiHoYoGameName.HonkaiSea)
            {
                url = UrlContentHonkai;
                obj = new { launcher_id = LauncherIdHonkaiSea, language = language.ToString(), filter_adv = "false" };
            }
            else if (gameName == MiHoYoGameName.HonkaiTwHkMo)
            {
                url = UrlContentHonkai;
                obj = new { launcher_id = LauncherIdHonkaiTwHkMo, language = language.ToString(), filter_adv = "false" };
            }
            else if (gameName == MiHoYoGameName.YuanShen)
            {
                url = UrlContentYuanShen;
                obj = new { launcher_id = LauncherIdYuanShen, language = Language.Chinese, filter_adv = "false" };
            }
            else
            {
                throw new ArgumentOutOfRangeException(gameName);
            }

            return GetDataJson<DataJsonContent>(url, obj, cancellationToken);
        }

        public Task<DataJsonResource> GetResource(MiHoYoGameName gameName, CancellationToken cancellationToken = default)
        {
            string url;
            object obj;

            if (gameName == MiHoYoGameName.BengHuai)
            {
                url = UrlResourceBengHuai;
                obj = new { key = ApiKeyBengHuai, launcher_id = LauncherIdBengHuai, channel_id = ChannelId, sub_channel_id = SubChannelIdBengHuai };
            }
            else if (gameName == MiHoYoGameName.Genshin)
            {
                url = UrlResourceGenshin;
                obj = new { key = ApiKeyGenshin, launcher_id = LauncherIdGenshin, channel_id = ChannelId, sub_channel_id = SubChannelIdGenshin };
            }
            else if (gameName == MiHoYoGameName.HonkaiKr)
            {
                url = UrlResourceHonkai;
                obj = new { key = ApiKeyHonkaiKr, launcher_id = LauncherIdHonkaiKr };
            }
            else if (gameName == MiHoYoGameName.HonkaiNaEu)
            {
                url = UrlResourceHonkai;
                obj = new { key = ApiKeyHonkaiNaEu, launcher_id = LauncherIdHonkaiNaEu };
            }
            else if (gameName == MiHoYoGameName.HonkaiSea)
            {
                url = UrlResourceHonkai;
                obj = new { key = ApiKeyHonkaiSea, launcher_id = LauncherIdHonkaiSea };
            }
            else if (gameName == MiHoYoGameName.HonkaiTwHkMo)
            {
                url = UrlResourceHonkai;
                obj = new { key = ApiKeyHonkaiTwHkMo, launcher_id = LauncherIdHonkaiTwHkMo };
            }
            else if (gameName == MiHoYoGameName.YuanShen)
            {
                url = UrlResourceYuanShen;
                obj = new { key = ApiKeyYuanShen, launcher_id = LauncherIdYuanShen, channel_id = ChannelId, sub_channel_id = SubChannelIdYuanShen };
            }
            else
            {
                throw new ArgumentOutOfRangeException(gameName);
            }

            return GetDataJson<DataJsonResource>(url, obj, cancellationToken);
        }

        private async Task<T> GetDataJson<T>(string url, object obj, CancellationToken cancellationToken = default) where T : IDataJson
        {
            JsonContent         content  = JsonContent.Create(obj, options: _jsonSerializerOptions);
            HttpResponseMessage response = await _httpClient.PostAsync(url, content, cancellationToken);
            response.EnsureSuccessStatusCode();

            ResponseJson<T>? responseJson = await response.Content.ReadFromJsonAsync<ResponseJson<T>>(_jsonSerializerOptions, cancellationToken);
            responseJson!.EnsureSuccessStatusCode();

            return responseJson.Data;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}