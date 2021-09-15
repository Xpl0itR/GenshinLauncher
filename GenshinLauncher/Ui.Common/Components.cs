// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace GenshinLauncher.Ui.Common
{
    [System.Flags]
    public enum Components
    {
        None               = 0x00,
        ButtonLaunch       = 0x01,
        ButtonDownload     = 0x02,
        ButtonDirectX      = 0x04,
        InstallDirOptions  = 0x08,
        SettingsBox        = 0x10,
        ProgressBarBlocks  = 0x20,
        ProgressBarMarquee = 0x40,

        ProgressBar = ProgressBarBlocks | ProgressBarMarquee
    }
}