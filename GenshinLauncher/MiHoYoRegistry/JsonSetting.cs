// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Text.Json.Serialization;

namespace GenshinLauncher.MiHoYoRegistry;

// ReSharper disable ClassNeverInstantiated.Global, NotAccessedPositionalProperty.Global
public record JsonSettingScreen
(   // ReSharper disable once StringLiteralTypo
    [property: JsonPropertyName("isfullScreen")] bool IsFullscreen,
    [property: JsonPropertyName("width")]        int  Width,
    [property: JsonPropertyName("height")]       int  Height
);

public record JsonSettingGraphics
(
    [property: JsonPropertyName("RecommendGrade")]             string RecommendGrade,
    [property: JsonPropertyName("IsUserDefinedGrade")]         bool   IsUserDefinedGrade,
    [property: JsonPropertyName("IsUserDefinedVolatile")]      bool   IsUserDefinedVolatile,
    [property: JsonPropertyName("IsEcoMode")]                  bool   IsEcoMode,
    [property: JsonPropertyName("RecommendResolutionX")]       int    RecommendResolutionX,
    [property: JsonPropertyName("RecommendResolutionY")]       int    RecommendResolutionY,
    [property: JsonPropertyName("ResolutionQuality")]          string ResolutionQuality,
    [property: JsonPropertyName("TargetFrameRateForInLevel")]  int    TargetFrameRateForInLevel,
    [property: JsonPropertyName("TargetFrameRateForOthers")]   int    TargetFrameRateForOthers,
    [property: JsonPropertyName("ContrastDelta")]              float  ContrastDelta,
    [property: JsonPropertyName("isBrightnessStandardModeOn")] bool   IsBrightnessStandardModeOn,
    [property: JsonPropertyName("VolatileSetting")]            object VolatileSetting
);