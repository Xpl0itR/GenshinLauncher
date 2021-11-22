// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Windows.Forms;
using DarkUI.Forms;
using GenshinLauncher.Ui.Common;

namespace GenshinLauncher.Ui.WinForms;

public partial class MainWindow : DarkForm, IMainWindow
{
    private Components _components;

    public MainWindow()
    {
        InitializeComponent();
        base.Text = nameof(GenshinLauncher);
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

    public event EventHandler ButtonStopClick
    {
        add    => _buttonStop.Click += value;
        remove => _buttonStop.Click -= value;
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

    public int ProgressBarValue
    {
        set => _progressBar.Value = value;
    }

    public string LabelProgressBarTextLeft1
    {
        set => _labelProgressBarTextLeft1.Text = value;
    }

    public string LabelProgressBarTextLeft2
    {
        set => _labelProgressBarTextLeft2.Text = value;
    }

    public string LabelProgressBarTextRight
    {
        set => _labelProgressBarTextRight.Text = value;
    }

    public string LabelProgressBarTextBottom
    {
        set => _labelProgressBarTextBottom.Text = value;
    }

    public Components Components
    {
        get => _components;

        set
        {
            _components = value;

            bool downloadingDisabled = _components.HasFlag(Components.DisableDownloading);
            if (downloadingDisabled)
            {
                _buttonInstallDirectX.Enabled  = false;
                _buttonDownloadPreload.Enabled = false;
            }
            else
            {
                _buttonInstallDirectX.Enabled  = true;
                _buttonDownloadPreload.Enabled = true;
            }

            if (_components.HasFlag(Components.ButtonLaunch))
            {
                _buttonAccept.Text    = LocalizedStrings.Launch;
                _buttonAccept.Enabled = true;
            }
            else if (_components.HasFlag(Components.ButtonDownload))
            {
                _buttonAccept.Text    = LocalizedStrings.Download;
                _buttonAccept.Enabled = !downloadingDisabled;
            }
            else if (_components.HasFlag(Components.ButtonUpdate))
            {
                _buttonAccept.Text    = LocalizedStrings.Update;
                _buttonAccept.Enabled = !downloadingDisabled;
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
                _progressBar.Hide();
                _buttonStop.Hide();
                _labelProgressBarTextLeft1.Hide();
                _labelProgressBarTextLeft2.Hide();
                _labelProgressBarTextBottom.Hide();
            }
            else
            {
                _progressBar.Show();
                _buttonStop.Show();
                _labelProgressBarTextLeft1.Show();
                _labelProgressBarTextLeft2.Show();
                _labelProgressBarTextBottom.Show();

                _progressBar.Style = _components.HasFlag(Components.ProgressBarMarquee)
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
            _trayIcon.Visible = false;
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