// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace GenshinLauncher.Ui.Common;

// ReSharper disable UnusedMember.Global
[Flags]
public enum Components
{
    None               = 0x00,
    ButtonLaunch       = 0x01,
    ButtonDownload     = 0x02,
    ButtonUpdate       = 0x04,
    ButtonPreload      = 0x08,
    ProgressBarBlocks  = 0x10,
    ProgressBarMarquee = 0x20,
    CheckingForUpdate  = 0x40,
    DisableDownloading = 0x80,

    ProgressBar = ProgressBarBlocks | ProgressBarMarquee
}