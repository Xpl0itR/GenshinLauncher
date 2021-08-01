// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DarkUI.Forms;

namespace GenshinLauncher
{
    public partial class MainForm : DarkForm
    {
        private readonly Settings _settings;
        
        public MainForm(Settings settings)
        {
            _settings = settings;
            InitializeComponent();

            _checkBoxCloseToTray.Checked  = _settings.CloseToTray.CurrentValue;
            _checkBoxExitOnLaunch.Checked = _settings.ExitOnLaunch.CurrentValue;
            _numericWindowWidth.Value     = _settings.ResolutionWidth.CurrentValue;
            _numericWindowHeight.Value    = _settings.ResolutionHeight.CurrentValue;
            _comboBoxWindowMode.Text      = _settings.BorderlessMode.CurrentValue
                ? "Borderless Windowed" 
                : _settings.FullscreenMode.CurrentValue
                    ? "Exclusive Fullscreen" 
                    : "Windowed";
        }

        private void UnHideMainForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private static IntPtr GetMainWindowHandle(Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            while (handle == IntPtr.Zero)
            {
                handle = process.MainWindowHandle;
            }

            return handle;
        }

        private void LaunchButton_Click(object sender, EventArgs args) //TODO: rewrite
        {
            _settings.Save();

            string path = Path.Join(_settings.InstallPath.CurrentValue, _settings.EntryPoint.CurrentValue);

            if (!File.Exists(path))
            {
                _buttonLaunch.Enabled = false;
                DarkMessageBox.ShowError("Genshin Impact is not installed", "Genshin Impact Launcher - Error");
                return;
            }

            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(_settings.EntryPoint.CurrentValue)).Length > 0)
            {
                DarkMessageBox.ShowError("Genshin Impact is already running", "Genshin Impact Launcher - Error");
                return;
            }

            Process process = new Process();
            process.StartInfo.FileName = path;
            process.Start();

            if (_settings.BorderlessMode.CurrentValue)
            {
                IntPtr handle = GetMainWindowHandle(process);
                WinApiUtils.RemoveTitleBar(handle);
                WinApiUtils.ResizeToFullscreen(handle);
            }

            if (_settings.ExitOnLaunch.CurrentValue)
            {
                Application.Exit();
            }
        }

        private void ButtonUseScreenResolution_Click(object sender, EventArgs args)
        {
            Rectangle bounds = Screen.FromControl(this).Bounds;

            _numericWindowWidth.Value  = bounds.Width;
            _numericWindowHeight.Value = bounds.Height;
        }

        // Setting changed Events
        private void ComboBoxWindowMode_SelectionChangeCommitted(object sender, EventArgs args)
        {
            if (_comboBoxWindowMode.Text == "Borderless Windowed")
            {
                _settings.BorderlessMode.NewValue = true;
                _settings.FullscreenMode.NewValue = false;
            }
            else
            {
                _settings.BorderlessMode.NewValue = false;
                _settings.FullscreenMode.NewValue = _comboBoxWindowMode.Text == "Exclusive Fullscreen";
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

        private void CheckBoxCloseToTray_CheckedChanged(object sender, EventArgs args)
        {
            _settings.CloseToTray.NewValue = _checkBoxCloseToTray.Checked;
        }

        private void CheckBoxExitOnLaunch_CheckedChanged(object sender, EventArgs args)
        {
            _settings.ExitOnLaunch.NewValue = _checkBoxExitOnLaunch.Checked;
        }

        // Titlebar Events
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


        // Tray Events
        private void TrayIcon_DoubleClick(object sender, EventArgs args) => UnHideMainForm();

        private void OpenTrayMenuItem_Click(object sender, EventArgs args) => UnHideMainForm();

        private void ExitTrayMenuItem_Click(object sender, EventArgs args)
        {
            Application.Exit();
        }
    }
}