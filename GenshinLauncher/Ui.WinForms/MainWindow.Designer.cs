using System.ComponentModel;
using System.Windows.Forms;
using DarkUI.Controls;

namespace GenshinLauncher.Ui.WinForms
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this._trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this._trayMenu = new DarkUI.Controls.DarkContextMenu();
            this._trayMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this._trayMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this._trayMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this._buttonAccept = new DarkUI.Controls.DarkButton();
            this._radioButtonGlobalVersion = new DarkUI.Controls.DarkRadioButton();
            this._radioButtonChinaVersion = new DarkUI.Controls.DarkRadioButton();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._labelProgressBarTextBottom = new DarkUI.Controls.DarkLabel();
            this._labelProgressBarTextLeft = new DarkUI.Controls.DarkLabel();
            this._buttonStop = new DarkUI.Controls.DarkButton();
            this._buttonInstallDirectX = new DarkUI.Controls.DarkButton();
            this._buttonSettings = new DarkUI.Controls.DarkButton();
            this._labelCheckingForUpdates = new DarkUI.Controls.DarkLabel();
            this._buttonDownloadPreload = new DarkUI.Controls.DarkButton();
            this._labelProgressBarTextRight = new DarkUI.Controls.DarkLabel();
            this._trayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // _trayIcon
            // 
            this._trayIcon.ContextMenuStrip = this._trayMenu;
            resources.ApplyResources(this._trayIcon, "_trayIcon");
            this._trayIcon.DoubleClick += new System.EventHandler(this.TrayIcon_DoubleClick);
            // 
            // _trayMenu
            // 
            this._trayMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this._trayMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._trayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._trayMenuItemOpen,
            this._trayMenuSeparator,
            this._trayMenuItemExit});
            this._trayMenu.Name = "_trayMenu";
            resources.ApplyResources(this._trayMenu, "_trayMenu");
            // 
            // _trayMenuItemOpen
            // 
            this._trayMenuItemOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this._trayMenuItemOpen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._trayMenuItemOpen.Name = "_trayMenuItemOpen";
            resources.ApplyResources(this._trayMenuItemOpen, "_trayMenuItemOpen");
            this._trayMenuItemOpen.Click += new System.EventHandler(this.TrayMenuItemOpen_Click);
            // 
            // _trayMenuSeparator
            // 
            this._trayMenuSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this._trayMenuSeparator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._trayMenuSeparator.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this._trayMenuSeparator.Name = "_trayMenuSeparator";
            resources.ApplyResources(this._trayMenuSeparator, "_trayMenuSeparator");
            // 
            // _trayMenuItemExit
            // 
            this._trayMenuItemExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this._trayMenuItemExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._trayMenuItemExit.Name = "_trayMenuItemExit";
            resources.ApplyResources(this._trayMenuItemExit, "_trayMenuItemExit");
            this._trayMenuItemExit.Click += new System.EventHandler(this.TrayMenuItemExit_Click);
            // 
            // _buttonAccept
            // 
            resources.ApplyResources(this._buttonAccept, "_buttonAccept");
            this._buttonAccept.Name = "_buttonAccept";
            // 
            // _radioButtonGlobalVersion
            // 
            resources.ApplyResources(this._radioButtonGlobalVersion, "_radioButtonGlobalVersion");
            this._radioButtonGlobalVersion.Checked = true;
            this._radioButtonGlobalVersion.Name = "_radioButtonGlobalVersion";
            this._radioButtonGlobalVersion.TabStop = true;
            // 
            // _radioButtonChinaVersion
            // 
            resources.ApplyResources(this._radioButtonChinaVersion, "_radioButtonChinaVersion");
            this._radioButtonChinaVersion.Name = "_radioButtonChinaVersion";
            // 
            // _progressBar
            // 
            resources.ApplyResources(this._progressBar, "_progressBar");
            this._progressBar.MarqueeAnimationSpeed = 1;
            this._progressBar.Maximum = 2147483647;
            this._progressBar.Name = "_progressBar";
            // 
            // _labelProgressBarTextBottom
            // 
            resources.ApplyResources(this._labelProgressBarTextBottom, "_labelProgressBarTextBottom");
            this._labelProgressBarTextBottom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelProgressBarTextBottom.Name = "_labelProgressBarTextBottom";
            // 
            // _labelProgressBarTextLeft
            // 
            resources.ApplyResources(this._labelProgressBarTextLeft, "_labelProgressBarTextLeft");
            this._labelProgressBarTextLeft.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelProgressBarTextLeft.Name = "_labelProgressBarTextLeft";
            // 
            // _buttonStop
            // 
            resources.ApplyResources(this._buttonStop, "_buttonStop");
            this._buttonStop.Name = "_buttonStop";
            // 
            // _buttonInstallDirectX
            // 
            resources.ApplyResources(this._buttonInstallDirectX, "_buttonInstallDirectX");
            this._buttonInstallDirectX.Name = "_buttonInstallDirectX";
            // 
            // _buttonSettings
            // 
            resources.ApplyResources(this._buttonSettings, "_buttonSettings");
            this._buttonSettings.Name = "_buttonSettings";
            // 
            // _labelCheckingForUpdates
            // 
            resources.ApplyResources(this._labelCheckingForUpdates, "_labelCheckingForUpdates");
            this._labelCheckingForUpdates.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelCheckingForUpdates.Name = "_labelCheckingForUpdates";
            // 
            // _buttonDownloadPreload
            // 
            resources.ApplyResources(this._buttonDownloadPreload, "_buttonDownloadPreload");
            this._buttonDownloadPreload.Name = "_buttonDownloadPreload";
            // 
            // _labelProgressBarTextRight
            // 
            this._labelProgressBarTextRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            resources.ApplyResources(this._labelProgressBarTextRight, "_labelProgressBarTextRight");
            this._labelProgressBarTextRight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelProgressBarTextRight.Name = "_labelProgressBarTextRight";
            // 
            // MainWindow
            // 
            this.AcceptButton = this._buttonAccept;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._labelProgressBarTextRight);
            this.Controls.Add(this._buttonDownloadPreload);
            this.Controls.Add(this._labelCheckingForUpdates);
            this.Controls.Add(this._buttonSettings);
            this.Controls.Add(this._buttonInstallDirectX);
            this.Controls.Add(this._buttonStop);
            this.Controls.Add(this._labelProgressBarTextLeft);
            this.Controls.Add(this._labelProgressBarTextBottom);
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._radioButtonChinaVersion);
            this.Controls.Add(this._radioButtonGlobalVersion);
            this.Controls.Add(this._buttonAccept);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this._trayMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkButton _buttonAccept;
        private NotifyIcon _trayIcon;
        private DarkContextMenu _trayMenu;
        private ToolStripMenuItem _trayMenuItemOpen;
        private ToolStripMenuItem _trayMenuItemExit;
        private ToolStripSeparator _trayMenuSeparator;
        private DarkRadioButton _radioButtonChinaVersion;
        private DarkRadioButton _radioButtonGlobalVersion;
        private ProgressBar _progressBar;
        private DarkLabel _labelProgressBarTextBottom;
        private DarkLabel _labelProgressBarTextLeft;
        private DarkButton _buttonStop;
        private DarkButton _buttonInstallDirectX;
        private DarkButton _buttonSettings;
        private DarkLabel _labelCheckingForUpdates;
        private DarkButton _buttonDownloadPreload;
        private DarkLabel _labelProgressBarTextRight;
    }
}