// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
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
        }

        private void LoadValues()
        {
            _checkBoxCloseToTray.Checked  = _launcher.CloseToTray;
            _checkBoxExitOnLaunch.Checked = _launcher.ExitOnLaunch;

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
                _buttonLaunch.Enabled     = false;
            }

            if (File.Exists(_launcher.BackgroundPath))
            {
                base.BackgroundImage = Image.FromFile(_launcher.BackgroundPath);
            }
        }

        private void UnHideMainForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void UseScreenResolution()
        {
            Rectangle bounds = Screen.FromControl(this).Bounds;

            _numericWindowWidth.Value  = bounds.Width;
            _numericWindowHeight.Value = bounds.Height;
        }

        private void ButtonUseScreenResolution_Click(object sender, EventArgs args) =>
            UseScreenResolution();

        // Launch button events
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
            UnHideMainForm();

        private void OpenTrayMenuItem_Click(object sender, EventArgs args) =>
            UnHideMainForm();

        private void ExitTrayMenuItem_Click(object sender, EventArgs args) =>
            Application.Exit();
    }
}