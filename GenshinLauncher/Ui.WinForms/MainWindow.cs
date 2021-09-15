// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.ComponentModel;
using System.Drawing;
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

        #region Events
        public event EventHandler<string>? GameDirectoryUpdate;

        public event EventHandler ButtonAcceptClick
        {
            add    => _buttonAccept.Click += value;
            remove => _buttonAccept.Click -= value;
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

        public event EventHandler ButtonUseScreenResolutionClick
        {
            add    => _buttonUseScreenResolution.Click += value;
            remove => _buttonUseScreenResolution.Click -= value;
        }

        public event EventHandler NumericWindowHeightValueChanged
        {
            add    => _numericWindowHeight.ValueChanged += value;
            remove => _numericWindowHeight.ValueChanged -= value;
        }

        public event EventHandler NumericWindowWidthValueChanged
        {
            add    => _numericWindowWidth.ValueChanged += value;
            remove => _numericWindowWidth.ValueChanged -= value;
        }

        public event EventHandler NumericMonitorIndexValueChanged
        {
            add    => _numericMonitorIndex.ValueChanged += value;
            remove => _numericMonitorIndex.ValueChanged -= value;
        }

        public event EventHandler CheckBoxCloseToTrayCheckedChanged
        {
            add    => _checkBoxCloseToTray.CheckedChanged += value;
            remove => _checkBoxCloseToTray.CheckedChanged -= value;
        }

        public event EventHandler CheckBoxExitOnLaunchCheckedChanged
        {
            add    => _checkBoxExitOnLaunch.CheckedChanged += value;
            remove => _checkBoxExitOnLaunch.CheckedChanged -= value;
        }

        public event EventHandler WindowModeCheckedChanged
        {
            add
            {
                _radioButtonFullscreen.CheckedChanged += value;
                _radioButtonBorderless.CheckedChanged += value;
                _radioButtonWindowed.CheckedChanged   += value;
            }

            remove
            {
                _radioButtonFullscreen.CheckedChanged -= value;
                _radioButtonBorderless.CheckedChanged -= value;
                _radioButtonWindowed.CheckedChanged   -= value;
            }
        }
        #endregion

        #region Properties
        public bool CheckBoxCloseToTrayChecked
        {
            get => _checkBoxCloseToTray.Checked;
            set => _checkBoxCloseToTray.Checked = value;
        }

        public bool CheckBoxExitOnLaunchChecked
        {
            get => _checkBoxExitOnLaunch.Checked;
            set => _checkBoxExitOnLaunch.Checked = value;
        }

        public bool RadioButtonFullscreenChecked
        {
            get => _radioButtonFullscreen.Checked;
            set => _radioButtonFullscreen.Checked = value;
        }

        public bool RadioButtonBorderlessChecked
        {
            get => _radioButtonBorderless.Checked;
            set => _radioButtonBorderless.Checked = value;
        }

        public bool RadioButtonWindowedChecked
        {
            get => _radioButtonWindowed.Checked;
            set => _radioButtonWindowed.Checked = value;
        }

        public bool RadioButtonGlobalVersionChecked
        {
            get => _radioButtonGlobalVersion.Checked;
            set => _radioButtonGlobalVersion.Checked = value;
        }

        public int NumericMonitorIndexMaximum 
        {
            get => (int)_numericMonitorIndex.Maximum;
            set => _numericMonitorIndex.Maximum = value;
        }

        public int NumericMonitorIndexValue
        {
            get => (int)_numericMonitorIndex.Value;
            set => _numericMonitorIndex.Value = value;
        }

        public int NumericWindowHeightValue
        {
            get => (int)_numericWindowHeight.Value;
            set => _numericWindowHeight.Value = value;
        }

        public int NumericWindowWidthValue
        {
            get => (int)_numericWindowWidth.Value;
            set => _numericWindowWidth.Value = value;
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

        public string TextBoxGameDirText
        {
            get => _textBoxInstallDir.Text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _textBoxInstallDir.Select();
                }
                else
                {
                    _textBoxInstallDir.Text = value;
                }
            }
        }

        #endregion

        public Components Components
        {
            get => _components;

            set
            {
                _components = value;

                _buttonInstallDirectX.Enabled = _components.HasFlag(Components.ButtonDirectX);
                _groupBoxSettings.Enabled     = _components.HasFlag(Components.SettingsBox);

                if (_components.HasFlag(Components.ButtonLaunch))
                {
                    _buttonAccept.Text    = "Launch";
                    _buttonAccept.Enabled = true;
                }
                else if (_components.HasFlag(Components.ButtonDownload))
                {
                    _buttonAccept.Text    = "Download";
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

                if (_components.HasFlag(Components.InstallDirOptions))
                {
                    _textBoxInstallDir.Show();
                    _buttonInstallDirectory.Show();
                }
                else
                {
                    _textBoxInstallDir.Hide();
                    _buttonInstallDirectory.Hide();
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

        public Rectangle GetCurrentScreenBounds() =>
            Screen.FromControl(this).Bounds;

        // Install directory text box events
        private void ButtonInstallDir_Click(object sender, EventArgs args)
        {
            if (_folderBrowser.ShowDialog() == DialogResult.OK)
            {
                _textBoxInstallDir.Text = _folderBrowser.SelectedPath;
                GameDirectoryUpdate?.Invoke(this, _folderBrowser.SelectedPath);
            }
        }

        private void TextBoxInstallDir_Validating(object sender, CancelEventArgs args)
        {
            if (Utils.IsFolderPathValid(_textBoxInstallDir.Text))
            {
                return;
            }

            args.Cancel = true;
            _errorProvider.SetError(_textBoxInstallDir, "Invalid characters in path");
        }

        private void TextBoxInstallDir_Validated(object sender, EventArgs args)
        {
            _errorProvider.SetError(_textBoxInstallDir, null);
            GameDirectoryUpdate?.Invoke(this, _textBoxInstallDir.Text);
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
            if (args.CloseReason == CloseReason.UserClosing && _checkBoxCloseToTray.Checked)
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