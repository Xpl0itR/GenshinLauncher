// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DarkUI.Forms;

namespace GenshinLauncher.WinForms
{
    public partial class MainWindow : DarkForm
    {
        private readonly Launcher _launcher;

        public MainWindow(Launcher launcher)
        {
            _launcher = launcher;

            InitializeComponent();
            LoadValues();
            LoadAdditionalContentAsync();
        }

        private void LoadValues()
        {
            _checkBoxCloseToTray.Checked  = _launcher.CloseToTray;
            _checkBoxExitOnLaunch.Checked = _launcher.ExitOnLaunch;

            if (string.IsNullOrWhiteSpace(_launcher.GameInstallDir))
            {
                _textBoxInstallDir.Select();
            }
            else
            {
                _textBoxInstallDir.Text = _launcher.GameInstallDir;
            }

            if (File.Exists(_launcher.EntryPointPath))
            {
                _numericMonitorIndex.Maximum   = Screen.AllScreens.Length - 1;
                _numericMonitorIndex.Value     = (int)_launcher.MonitorIndex;
                _radioButtonFullscreen.Checked = (bool)_launcher.FullscreenMode;

                if (_launcher.BorderlessMode)
                {
                    _radioButtonBorderless.Checked = true;
                }
                else if ((bool)_launcher.FullscreenMode)
                {
                    _radioButtonFullscreen.Checked = true;
                }
                else
                {
                    _radioButtonWindowed.Checked = true;
                }

                if (_launcher.ResolutionHeight == 0 || _launcher.ResolutionWidth == 0)
                {
                    UseScreenResolution();
                }
                else
                {
                    _numericWindowWidth.Value  = (int)_launcher.ResolutionWidth;
                    _numericWindowHeight.Value = (int)_launcher.ResolutionHeight;
                }
            }
            else
            {
                _groupBoxSettings.Enabled = false;
                ShowDownloadButton();
            }

            if (File.Exists(_launcher.BackgroundPath))
            {
                base.BackgroundImage = Image.FromFile(_launcher.BackgroundPath);
            }
        }

        private async void LoadAdditionalContentAsync()
        {
            (string bgName, string bgMd5, Banner[] banners, Post[] posts, Stream? bgStream) = await _launcher.GetAdditionalContent(Resources.LauncherApiContentLanguage);

            _launcher.BackgroundMd5      = bgMd5;
            _launcher.BackgroundFileName = bgName;
            _launcher.SaveLauncherConfig();

            if (bgStream != null)
            {
                base.BackgroundImage = Image.FromStream(bgStream);
            }

            //TODO: implement banner and post viewers into UI
        }

        private void UnHideMainWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void ShowLaunchButton()
        {
            _buttonLaunch.Show();
            _buttonDownload.Hide();
            _radioButtonGlobalVersion.Hide();
            _radioButtonChinaVersion.Hide();
        }

        private void ShowDownloadButton()
        {
            _buttonLaunch.Hide();
            _buttonDownload.Show();
            _radioButtonGlobalVersion.Show();
            _radioButtonChinaVersion.Show();
        }

        private void ShowProgressBar()
        {
            _textBoxInstallDir.Hide();
            _buttonInstallPath.Hide();
            _labelProgressBarTitle.Show();
            _progressBarDownload.Show();
            _labelProgressBarText.Show();
        }

        private void ShowInstallPath()
        {
            _textBoxInstallDir.Show();
            _buttonInstallPath.Show();
            _labelProgressBarTitle.Hide();
            _progressBarDownload.Hide();
            _labelProgressBarText.Hide();
        }

        private void UseScreenResolution()
        {
            Rectangle bounds = Screen.FromControl(this).Bounds;

            _numericWindowWidth.Value = bounds.Width;
            _launcher.ResolutionWidth.SetValue(bounds.Width);

            _numericWindowHeight.Value = bounds.Height;
            _launcher.ResolutionHeight.SetValue(bounds.Height);
        }

        private void UpdateInstallPath()
        {
            _launcher.GameInstallDir = _textBoxInstallDir.Text.Replace(@"\", "/");

            if (File.Exists(Path.Combine(_launcher.GameInstallDir, Launcher.ExeNameGlobal)))
            {
                _launcher.EntryPoint = Launcher.ExeNameGlobal;
                ShowLaunchButton();
            }
            else if (File.Exists(Path.Combine(_launcher.GameInstallDir, Launcher.ExeNameChina)))
            {
                _launcher.EntryPoint = Launcher.ExeNameChina;
                ShowLaunchButton();
            }
            else
            {
                _launcher.EntryPoint = string.Empty;
                ShowDownloadButton();
            }
        }

        private void TextBoxInstallPath_Validating(object sender, CancelEventArgs args)
        {
            if (IsFolderPathValid(_textBoxInstallDir.Text))
            {
                return;
            }

            args.Cancel = true;
            _errorProvider.SetError(_textBoxInstallDir, Resources.ErrorInvalidCharactersInPath);
        }

        private void TextBoxInstallPath_Validated(object sender, EventArgs args)
        {
            _errorProvider.SetError(_textBoxInstallDir, null);
            UpdateInstallPath();
        }

        // Button events
        private void ButtonLaunch_Click(object sender, EventArgs args)
        {
            _launcher.SaveLauncherConfig();

            try
            {
                _launcher.StartGame();

                if (_launcher.BorderlessMode)
                {
                    _launcher.RemoveGameTitlebar();
                    _launcher.ResizeGameToFillBounds();
                }
            }
            catch (InvalidOperationException)
            {
                DarkMessageBox.ShowError(Resources.ErrorProcessAlreadyRunning, this.Text);
                return;
            }

            if (_launcher.ExitOnLaunch)
            {
                Application.Exit();
            }
        }

        private async void ButtonDownload_Click(object sender, EventArgs args)
        {
            _launcher.SaveLauncherConfig();

            _buttonLaunch.Enabled = false;
            ShowLaunchButton();
            //ShowProgressBar();

            await _launcher.DownloadLatestVersion(_radioButtonGlobalVersion.Checked);

            ShowInstallPath();
            _buttonLaunch.Enabled = true;
        }

        private void ButtonInstallPath_Click(object sender, EventArgs args)
        {
            if (_folderBrowser.ShowDialog() == DialogResult.OK)
            {
                _textBoxInstallDir.Text = _folderBrowser.SelectedPath;
                UpdateInstallPath();
            }
        }

        private void ButtonUseScreenResolution_Click(object sender, EventArgs args) =>
            UseScreenResolution();

        // Setting changed events
        private void WindowMode_CheckedChanged(object sender, EventArgs args)
        {
            if (_radioButtonBorderless.Checked)
            {
                _launcher.BorderlessMode = true;
                _launcher.FullscreenMode.SetValue(false);
            }
            else
            {
                _launcher.BorderlessMode = false;
                _launcher.FullscreenMode.SetValue(_radioButtonFullscreen.Checked);
            }
        }

        private void NumericWindowWidth_ValueChanged(object sender, EventArgs args)
        {
            _launcher.ResolutionWidth.SetValue((int)_numericWindowWidth.Value);
        }

        private void NumericWindowHeight_ValueChanged(object sender, EventArgs args)
        {
            _launcher.ResolutionHeight.SetValue((int)_numericWindowHeight.Value);
        }

        private void NumericMonitorIndex_ValueChanged(object sender, EventArgs args)
        {
            _launcher.MonitorIndex.SetValue((int)_numericMonitorIndex.Value);
        }

        private void CheckBoxCloseToTray_CheckedChanged(object sender, EventArgs args)
        {
            _launcher.CloseToTray = _checkBoxCloseToTray.Checked;
        }

        private void CheckBoxExitOnLaunch_CheckedChanged(object sender, EventArgs args)
        {
            _launcher.ExitOnLaunch = _checkBoxExitOnLaunch.Checked;
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
            if (args.CloseReason == CloseReason.UserClosing && _launcher.CloseToTray)
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

        private static bool IsFolderPathValid(string path) //TODO: Come up with a better method of validating paths
        {
            if (string.IsNullOrWhiteSpace(path) || path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                return false;
            }

            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}