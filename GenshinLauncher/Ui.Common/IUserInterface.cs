// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace GenshinLauncher.Ui.Common
{
    public interface IUserInterface
    {
        void RunMainWindow(IMainWindow mainWindow);
        void ShowErrorDialog(string title, string message, object? owner = null);
        void Exit();
    }
}