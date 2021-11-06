// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;

namespace GenshinLauncher.Ui.Common;

public interface ISettingsWindow
{
    event EventHandler ButtonSaveClick;

    bool   CheckBoxCloseToTrayChecked   { get; set; }
    bool   CheckBoxExitOnLaunchChecked  { get; set; }
    bool   RadioButtonFullscreenChecked { get; set; }
    bool   RadioButtonBorderlessChecked { get; set; }
    bool   RadioButtonWindowedChecked   { get; set; }
    int    NumericMonitorIndexMaximum   { set; }
    int    NumericMonitorIndexValue     { get; set; }
    int    NumericWindowHeightValue     { get; set; }
    int    NumericWindowWidthValue      { get; set; }
    string TextBoxGameDirText           { get; set; }

    void Close();
}