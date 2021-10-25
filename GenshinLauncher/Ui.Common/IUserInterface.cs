// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace GenshinLauncher.Ui.Common
{
    public interface IUserInterface
    {
        IMainWindow MainWindow { get; }

        ISettingsWindow NewSettingsWindow();

        void RunMainWindow();

        void RunSettingsWindow(ISettingsWindow settingsWindow);

        void ShowErrorDialog(string title, string message);

        void ShowThreadExceptionDialog(System.Exception exception);

        void Exit();
    }
}