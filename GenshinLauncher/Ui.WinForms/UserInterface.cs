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
        public IMainWindow MainWindow => _mainWindow;

        private readonly MainWindow _mainWindow;

        public UserInterface()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _mainWindow = new MainWindow();
        }

        public ISettingsWindow NewSettingsWindow() =>
            new SettingsWindow();

        public void RunMainWindow() =>
            Application.Run(_mainWindow);

        public void RunSettingsWindow(ISettingsWindow settingsWindow) =>
            ((Form)settingsWindow).ShowDialog(_mainWindow);

        public void ShowErrorDialog(string title, string message)
        {
            using DarkMessageBox darkMessageBox = new DarkMessageBox(message, title, DarkMessageBoxIcon.Error, DarkDialogButton.Ok);
            darkMessageBox.ShowDialog(_mainWindow);
        }

        public void Exit() =>
            Application.Exit();
    }
}