// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Drawing;

namespace GenshinLauncher.Ui.Common
{
    public interface IMainWindow
    {
        event EventHandler ButtonAcceptClick;
        event EventHandler ButtonDownloadPreloadClick;
        event EventHandler ButtonSettingsClick;
        event EventHandler ButtonStopDownloadClick;
        event EventHandler ButtonInstallDirectXClick;

        Components Components                        { get; set; }
        Image      BackgroundImage                   { get; set; }
        bool       RadioButtonGlobalVersionChecked   { get; set; }
        int        ProgressBarDownloadValue          { get; set; }
        string     LabelProgressBarDownloadTitleText { get; set; }
        string     LabelProgressBarDownloadText      { get; set; }
    }
}