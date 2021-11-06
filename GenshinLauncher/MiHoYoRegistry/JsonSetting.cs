// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;
// ReSharper disable UnusedAutoPropertyAccessor.Global, UnusedMember.Global

namespace GenshinLauncher.MiHoYoRegistry;

public readonly record struct JsonSettingScreen
{
    [JsonPropertyName("width")] public int Width { get; init; }

    [JsonPropertyName("height")] public int Height { get; init; }

    // ReSharper disable once StringLiteralTypo
    [JsonPropertyName("isfullScreen")] public bool IsFullscreen { get; init; }
}

public readonly record struct JsonSettingGraphics
{
    [JsonPropertyName("RecommendGrade")] public string RecommendGrade { get; init; }

    [JsonPropertyName("IsUserDefinedGrade")]
    public bool IsUserDefinedGrade { get; init; }

    [JsonPropertyName("IsUserDefinedVolatile")]
    public bool IsUserDefinedVolatile { get; init; }

    [JsonPropertyName("IsEcoMode")] public bool IsEcoMode { get; init; }

    [JsonPropertyName("RecommendResolutionX")]
    public int RecommendResolutionX { get; init; }

    [JsonPropertyName("RecommendResolutionY")]
    public int RecommendResolutionY { get; init; }

    [JsonPropertyName("ResolutionQuality")]
    public string ResolutionQuality { get; init; }

    [JsonPropertyName("TargetFrameRateForInLevel")]
    public int TargetFrameRateForInLevel { get; init; }

    [JsonPropertyName("TargetFrameRateForOthers")]
    public int TargetFrameRateForOthers { get; init; }

    [JsonPropertyName("ContrastDelta")] public float ContrastDelta { get; init; }

    [JsonPropertyName("isBrightnessStandardModeOn")]
    public bool IsBrightnessStandardModeOn { get; init; }

    [JsonPropertyName("VolatileSetting")] public object VolatileSetting { get; init; }
}