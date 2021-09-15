// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Windows.Forms;
using DarkUI.Forms;
using GenshinLauncher.Ui.Common;

namespace GenshinLauncher.Ui.WinForms
{
    public class UserInterface : IUserInterface
    {
        public UserInterface()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
        }

        public void RunMainWindow(IMainWindow mainWindow) =>
            Application.Run((Form)mainWindow);

        public void ShowErrorDialog(string title, string message, object? owner)
        {
            using DarkMessageBox darkMessageBox = new DarkMessageBox(message, title, DarkMessageBoxIcon.Error, DarkDialogButton.Ok);
            darkMessageBox.ShowDialog(owner as IWin32Window);
        }

        public void Exit() =>
            Application.Exit();
    }
}