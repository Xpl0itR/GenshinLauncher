// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Windows.Forms;
using DarkUI.Forms;
using GenshinLauncher.Ui.Common;

namespace GenshinLauncher.Ui.WinForms
{
    public partial class MainWindow : DarkForm, IMainWindow
    {
        private Components _components;

        public MainWindow()
        {
            InitializeComponent();
        }

        public event EventHandler ButtonAcceptClick
        {
            add    => _buttonAccept.Click += value;
            remove => _buttonAccept.Click -= value;
        }

        public event EventHandler ButtonDownloadPreloadClick
        {
            add    => _buttonDownloadPreload.Click += value;
            remove => _buttonDownloadPreload.Click -= value;
        }

        public event EventHandler ButtonSettingsClick
        {
            add    => _buttonSettings.Click += value;
            remove => _buttonSettings.Click -= value;
        }

        public event EventHandler ButtonStopDownloadClick
        {
            add    => _buttonStopDownload.Click += value;
            remove => _buttonStopDownload.Click -= value;
        }

        public event EventHandler ButtonInstallDirectXClick
        {
            add    => _buttonInstallDirectX.Click += value;
            remove => _buttonInstallDirectX.Click -= value;
        }

        public bool RadioButtonGlobalVersionChecked
        {
            get => _radioButtonGlobalVersion.Checked;
            set => _radioButtonGlobalVersion.Checked = value;
        }

        public int ProgressBarDownloadValue
        {
            get => _progressBarDownload.Value;
            set => _progressBarDownload.Value = value;
        }

        public string LabelProgressBarDownloadTitleText
        {
            get => _labelProgressBarTitle.Text;
            set => _labelProgressBarTitle.Text = value;
        }

        public string LabelProgressBarDownloadText
        {
            get => _labelProgressBarText.Text;
            set => _labelProgressBarText.Text = value;
        }

        public Components Components
        {
            get => _components;

            set
            {
                _components = value;

                if (_components.HasFlag(Components.ButtonLaunch))
                {
                    _buttonAccept.Text    = "Launch"; //TODO: localize text
                    _buttonAccept.Enabled = true;
                }
                else if (_components.HasFlag(Components.ButtonDownload))
                {
                    _buttonAccept.Text    = "Download"; //TODO: localize text
                    _buttonAccept.Enabled = true;
                }
                else if (_components.HasFlag(Components.ButtonUpdate))
                {
                    _buttonAccept.Text    = "Update"; //TODO: localize text
                    _buttonAccept.Enabled = true;
                }
                else
                {
                    _buttonAccept.Enabled = false;
                }

                if (_components.HasFlag(Components.ButtonDownload))
                {
                    _radioButtonGlobalVersion.Show();
                    _radioButtonChinaVersion.Show();
                }
                else
                {
                    _radioButtonGlobalVersion.Hide();
                    _radioButtonChinaVersion.Hide();
                }

                if (_components.HasFlag(Components.ButtonPreload))
                {
                    _buttonDownloadPreload.Show();
                }
                else
                {
                    _buttonDownloadPreload.Hide();
                }

                if (_components.HasFlag(Components.CheckingForUpdate))
                {
                    _labelCheckingForUpdates.Show();
                }
                else
                {
                    _labelCheckingForUpdates.Hide();
                }

                if ((_components & Components.ProgressBar) == 0)
                {
                    _progressBarDownload.Hide();
                    _buttonStopDownload.Hide();
                    _labelProgressBarTitle.Hide();
                    _labelProgressBarText.Hide();
                }
                else
                {
                    _progressBarDownload.Show();
                    _buttonStopDownload.Show();
                    _labelProgressBarTitle.Show();
                    _labelProgressBarText.Show();

                    _progressBarDownload.Style = _components.HasFlag(Components.ProgressBarMarquee)
                        ? ProgressBarStyle.Marquee
                        : ProgressBarStyle.Blocks;
                }
            }
        }

        // Titlebar events
        private void MainForm_Resize(object sender, EventArgs args)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs args)
        {
            if (args.CloseReason == CloseReason.UserClosing && Program.CloseToTray)
            {
                args.Cancel      = true;
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            else
            {
                this._trayIcon.Visible = false;
            }
        }

        // Tray events
        private void TrayMenuItemExit_Click(object sender, EventArgs args) =>
            Application.Exit();

        private void TrayMenuItemOpen_Click(object sender, EventArgs args) =>
            UnHideMainWindow();

        private void TrayIcon_DoubleClick(object sender, EventArgs args) =>
            UnHideMainWindow();

        private void UnHideMainWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}