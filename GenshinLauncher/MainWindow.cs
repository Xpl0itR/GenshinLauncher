// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DarkUI.Forms;

namespace GenshinLauncher
{
    public partial class MainWindow : DarkForm
    {
        private readonly Settings _settings;
        
        public MainWindow(Settings settings)
        {
            _settings = settings;
            InitializeComponent();

            _numericMonitorIndex.Maximum   = Screen.AllScreens.Length - 1;
            _numericMonitorIndex.Value     = _settings.MonitorIndex.CurrentValue;
            _checkBoxCloseToTray.Checked   = _settings.CloseToTray.CurrentValue;
            _checkBoxExitOnLaunch.Checked  = _settings.ExitOnLaunch.CurrentValue;
            _numericWindowWidth.Value      = _settings.ResolutionWidth.CurrentValue;
            _numericWindowHeight.Value     = _settings.ResolutionHeight.CurrentValue;
            _radioButtonFullscreen.Checked = _settings.FullscreenMode.CurrentValue;

            if (_settings.BorderlessMode.CurrentValue)
            {
                _radioButtonBorderless.Checked = true;
            }
            else if (_settings.FullscreenMode.CurrentValue)
            {
                _radioButtonFullscreen.Checked = true;
            }
            else
            {
                _radioButtonWindowed.Checked = true;
            }

            string backgroundPath = Path.Join(AppContext.BaseDirectory, "bg", _settings.BackgroundName.CurrentValue);
            if (File.Exists(backgroundPath))
            {
                base.BackgroundImage = Image.FromFile(backgroundPath);
            }
        }

        private void UnHideMainForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void ButtonUseScreenResolution_Click(object sender, EventArgs args)
        {
            Rectangle bounds = Screen.FromControl(this).Bounds;

            _numericWindowWidth.Value  = bounds.Width;
            _numericWindowHeight.Value = bounds.Height;
        }


        // Launch button events
        private void ButtonLaunch_Click(object sender, EventArgs args)
        {
            _settings.Save();

            GenshinProcess process = null;

            try
            {
                process = new GenshinProcess(_settings.InstallPath.CurrentValue, _settings.EntryPoint.CurrentValue);
                process.Start();

                if (_settings.BorderlessMode.CurrentValue)
                {
                    process.RemoveWindowsTitlebar();
                    process.ResizeToFillScreen();
                }
            }
            catch (InvalidOperationException)
            {
                DarkMessageBox.ShowError(Resources.ErrorProcessAlreadyRunning, this.Text);
                return;
            }
            catch (Exception e) when (e is DirectoryNotFoundException or FileNotFoundException)
            {
                DarkMessageBox.ShowError(Resources.ErrorNotInstalled, this.Text);
                return;
            }
            finally
            {
                process?.Dispose();
            }

            if (_settings.ExitOnLaunch.CurrentValue)
            {
                Application.Exit();
            }
        }

        // Setting changed events
        private void WindowMode_CheckedChanged(object sender, EventArgs args)
        {
            if (_radioButtonBorderless.Checked)
            {
                _settings.BorderlessMode.NewValue = true;
                _settings.FullscreenMode.NewValue = false;
            }
            else
            {
                _settings.BorderlessMode.NewValue = false;
                _settings.FullscreenMode.NewValue = _radioButtonFullscreen.Checked;
            }
        }

        private void NumericWindowWidth_ValueChanged(object sender, EventArgs args)
        {
            _settings.ResolutionWidth.NewValue = (int)_numericWindowWidth.Value;
        }

        private void NumericWindowHeight_ValueChanged(object sender, EventArgs args)
        {
            _settings.ResolutionHeight.NewValue = (int)_numericWindowHeight.Value;
        }

        private void NumericMonitorIndex_ValueChanged(object sender, EventArgs args)
        {
            _settings.MonitorIndex.NewValue = (int)_numericMonitorIndex.Value;
        }

        private void CheckBoxCloseToTray_CheckedChanged(object sender, EventArgs args)
        {
            _settings.CloseToTray.NewValue = _checkBoxCloseToTray.Checked;
        }

        private void CheckBoxExitOnLaunch_CheckedChanged(object sender, EventArgs args)
        {
            _settings.ExitOnLaunch.NewValue = _checkBoxExitOnLaunch.Checked;
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
            if (args.CloseReason == CloseReason.UserClosing && _settings.CloseToTray.CurrentValue)
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
        private void TrayIcon_DoubleClick(object sender, EventArgs args) => UnHideMainForm();

        private void OpenTrayMenuItem_Click(object sender, EventArgs args) => UnHideMainForm();

        private void ExitTrayMenuItem_Click(object sender, EventArgs args)
        {
            Application.Exit();
        }
    }
}