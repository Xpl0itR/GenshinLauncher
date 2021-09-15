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
        event EventHandler<string> GameDirectoryUpdate;
        event EventHandler         ButtonAcceptClick;
        event EventHandler         ButtonStopDownloadClick;
        event EventHandler         ButtonInstallDirectXClick;
        event EventHandler         ButtonUseScreenResolutionClick;
        event EventHandler         NumericWindowHeightValueChanged;
        event EventHandler         NumericWindowWidthValueChanged;
        event EventHandler         NumericMonitorIndexValueChanged;
        event EventHandler         CheckBoxCloseToTrayCheckedChanged;
        event EventHandler         CheckBoxExitOnLaunchCheckedChanged;
        event EventHandler         WindowModeCheckedChanged;

        Components Components                        { get; set; }
        Image      BackgroundImage                   { get; set; }
        bool       CheckBoxCloseToTrayChecked        { get; set; }
        bool       CheckBoxExitOnLaunchChecked       { get; set; }
        bool       RadioButtonFullscreenChecked      { get; set; }
        bool       RadioButtonBorderlessChecked      { get; set; }
        bool       RadioButtonWindowedChecked        { get; set; }
        bool       RadioButtonGlobalVersionChecked   { get; set; }
        int        NumericMonitorIndexMaximum        { get; set; }
        int        NumericMonitorIndexValue          { get; set; }
        int        NumericWindowHeightValue          { get; set; }
        int        NumericWindowWidthValue           { get; set; }
        int        ProgressBarDownloadValue          { get; set; }
        string     LabelProgressBarDownloadTitleText { get; set; }
        string     LabelProgressBarDownloadText      { get; set; }
        string     TextBoxGameDirText                { get; set; }

        Rectangle GetCurrentScreenBounds();
    }
}