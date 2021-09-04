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
        public MainWindow()
        {
            InitializeComponent();
            _numericMonitorIndex.Maximum = Screen.AllScreens.Length - 1;
        }

        #region Events
        public event EventHandler<string>? GameDirectoryUpdate;

        public event EventHandler ButtonLaunchClick
        {
            add    => _buttonLaunch.Click += value;
            remove => _buttonLaunch.Click -= value;
        }

        public event EventHandler ButtonDownloadClick
        {
            add    => _buttonDownload.Click += value;
            remove => _buttonDownload.Click -= value;
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
        public bool ButtonDownloadEnabled
        {
            get => _buttonDownload.Enabled;
            set => _buttonDownload.Enabled = value;
        }

        public bool ButtonInstallDirectXEnabled
        {
            get => _buttonInstallDirectX.Enabled;
            set => _buttonInstallDirectX.Enabled = value;
        }

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

        public bool GroupBoxSettingsEnabled
        {
            get => _groupBoxSettings.Enabled;
            set => _groupBoxSettings.Enabled = value;
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

        public Rectangle GetCurrentScreenBounds() =>
            Screen.FromControl(this).Bounds;

        public void ShowErrorProcessAlreadyRunning() =>
            DarkMessageBox.ShowError("Another instance of Genshin Impact is already running!", this.Text);

        public void ShowButtonLaunch()
        {
            _buttonLaunch.Show();
            _buttonDownload.Hide();
            _radioButtonGlobalVersion.Hide();
            _radioButtonChinaVersion.Hide();
        }

        public void ShowButtonDownload()
        {
            _buttonLaunch.Hide();
            _buttonDownload.Show();
            _radioButtonGlobalVersion.Show();
            _radioButtonChinaVersion.Show();
        }

        public void ShowProgressBar()
        {
            _textBoxInstallDir.Hide();
            _buttonInstallDirectory.Hide();
            _buttonStopDownload.Show();
            _labelProgressBarTitle.Show();
            _progressBarDownload.Show();
            _labelProgressBarText.Show();
        }

        public void ShowInstallPath()
        {
            _textBoxInstallDir.Show();
            _buttonInstallDirectory.Show();
            _buttonStopDownload.Hide();
            _labelProgressBarTitle.Hide();
            _progressBarDownload.Hide();
            _labelProgressBarText.Hide();
        }

        public void SetProgressBarDownloadStyleBlock() =>
            _progressBarDownload.Style = ProgressBarStyle.Blocks;

        public void SetProgressBarDownloadStyleMarquee() =>
            _progressBarDownload.Style = ProgressBarStyle.Marquee;

        private void UnHideMainWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

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
                args.Cancel = true;
                this.Hide();
            }
            else
            {
                this._trayIcon.Visible = false;
            }
        }

        // Tray events
        private void TrayIcon_DoubleClick(object sender, EventArgs args) =>
            UnHideMainWindow();

        private void OpenTrayMenuItem_Click(object sender, EventArgs args) =>
            UnHideMainWindow();

        private void ExitTrayMenuItem_Click(object sender, EventArgs args) =>
            Application.Exit();
    }
}