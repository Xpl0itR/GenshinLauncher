// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Windows.Forms;
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

        public void Run(IMainWindow mainWindow) =>
            Application.Run((Form)mainWindow);

        public void Exit() =>
            Application.Exit();
    }
}