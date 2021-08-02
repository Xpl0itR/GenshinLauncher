// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System.ComponentModel;
using System.Windows.Forms;
using DarkUI.Controls;

namespace GenshinLauncher
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
            this._buttonLaunch = new DarkUI.Controls.DarkButton();
            this._checkBoxCloseToTray = new DarkUI.Controls.DarkCheckBox();
            this._checkBoxExitOnLaunch = new DarkUI.Controls.DarkCheckBox();
            this._labelWindowMode = new DarkUI.Controls.DarkLabel();
            this._labelWindowWidth = new DarkUI.Controls.DarkLabel();
            this._labelWindowHeight = new DarkUI.Controls.DarkLabel();
            this._numericWindowWidth = new DarkUI.Controls.DarkNumericUpDown();
            this._numericWindowHeight = new DarkUI.Controls.DarkNumericUpDown();
            this._buttonUseScreenResolution = new DarkUI.Controls.DarkButton();
            this._radioButtonFullscreen = new DarkUI.Controls.DarkRadioButton();
            this._radioButtonWindowed = new DarkUI.Controls.DarkRadioButton();
            this._radioButtonBorderless = new DarkUI.Controls.DarkRadioButton();
            this._trayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowHeight)).BeginInit();
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
            this._trayMenuItemOpen.Click += new System.EventHandler(this.OpenTrayMenuItem_Click);
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
            this._trayMenuItemExit.Click += new System.EventHandler(this.ExitTrayMenuItem_Click);
            // 
            // _buttonLaunch
            // 
            resources.ApplyResources(this._buttonLaunch, "_buttonLaunch");
            this._buttonLaunch.Name = "_buttonLaunch";
            this._buttonLaunch.Click += new System.EventHandler(this.ButtonLaunch_Click);
            // 
            // _checkBoxCloseToTray
            // 
            resources.ApplyResources(this._checkBoxCloseToTray, "_checkBoxCloseToTray");
            this._checkBoxCloseToTray.Name = "_checkBoxCloseToTray";
            this._checkBoxCloseToTray.CheckedChanged += new System.EventHandler(this.CheckBoxCloseToTray_CheckedChanged);
            // 
            // _checkBoxExitOnLaunch
            // 
            resources.ApplyResources(this._checkBoxExitOnLaunch, "_checkBoxExitOnLaunch");
            this._checkBoxExitOnLaunch.Name = "_checkBoxExitOnLaunch";
            this._checkBoxExitOnLaunch.CheckedChanged += new System.EventHandler(this.CheckBoxExitOnLaunch_CheckedChanged);
            // 
            // _labelWindowMode
            // 
            resources.ApplyResources(this._labelWindowMode, "_labelWindowMode");
            this._labelWindowMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowMode.Name = "_labelWindowMode";
            // 
            // _labelWindowWidth
            // 
            resources.ApplyResources(this._labelWindowWidth, "_labelWindowWidth");
            this._labelWindowWidth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowWidth.Name = "_labelWindowWidth";
            // 
            // _labelWindowHeight
            // 
            resources.ApplyResources(this._labelWindowHeight, "_labelWindowHeight");
            this._labelWindowHeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowHeight.Name = "_labelWindowHeight";
            // 
            // _numericWindowWidth
            // 
            resources.ApplyResources(this._numericWindowWidth, "_numericWindowWidth");
            this._numericWindowWidth.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._numericWindowWidth.Name = "_numericWindowWidth";
            this._numericWindowWidth.ValueChanged += new System.EventHandler(this.NumericWindowWidth_ValueChanged);
            // 
            // _numericWindowHeight
            // 
            resources.ApplyResources(this._numericWindowHeight, "_numericWindowHeight");
            this._numericWindowHeight.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._numericWindowHeight.Name = "_numericWindowHeight";
            this._numericWindowHeight.ValueChanged += new System.EventHandler(this.NumericWindowHeight_ValueChanged);
            // 
            // _buttonUseScreenResolution
            // 
            resources.ApplyResources(this._buttonUseScreenResolution, "_buttonUseScreenResolution");
            this._buttonUseScreenResolution.Name = "_buttonUseScreenResolution";
            this._buttonUseScreenResolution.Click += new System.EventHandler(this.ButtonUseScreenResolution_Click);
            // 
            // _radioButtonFullscreen
            // 
            resources.ApplyResources(this._radioButtonFullscreen, "_radioButtonFullscreen");
            this._radioButtonFullscreen.Name = "_radioButtonFullscreen";
            this._radioButtonFullscreen.TabStop = true;
            this._radioButtonFullscreen.CheckedChanged += new System.EventHandler(this.WindowMode_CheckedChanged);
            // 
            // _radioButtonWindowed
            // 
            resources.ApplyResources(this._radioButtonWindowed, "_radioButtonWindowed");
            this._radioButtonWindowed.Name = "_radioButtonWindowed";
            this._radioButtonWindowed.TabStop = true;
            this._radioButtonWindowed.CheckedChanged += new System.EventHandler(this.WindowMode_CheckedChanged);
            // 
            // _radioButtonBorderless
            // 
            resources.ApplyResources(this._radioButtonBorderless, "_radioButtonBorderless");
            this._radioButtonBorderless.Name = "_radioButtonBorderless";
            this._radioButtonBorderless.TabStop = true;
            this._radioButtonBorderless.CheckedChanged += new System.EventHandler(this.WindowMode_CheckedChanged);
            // 
            // MainWindow
            // 
            this.AcceptButton = this._buttonLaunch;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._radioButtonBorderless);
            this.Controls.Add(this._radioButtonWindowed);
            this.Controls.Add(this._radioButtonFullscreen);
            this.Controls.Add(this._buttonUseScreenResolution);
            this.Controls.Add(this._numericWindowHeight);
            this.Controls.Add(this._numericWindowWidth);
            this.Controls.Add(this._labelWindowHeight);
            this.Controls.Add(this._labelWindowWidth);
            this.Controls.Add(this._labelWindowMode);
            this.Controls.Add(this._checkBoxExitOnLaunch);
            this.Controls.Add(this._checkBoxCloseToTray);
            this.Controls.Add(this._buttonLaunch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this._trayMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkButton         _buttonLaunch;
        private DarkButton         _buttonUseScreenResolution;
        private DarkCheckBox       _checkBoxCloseToTray;
        private DarkCheckBox       _checkBoxExitOnLaunch;
        private DarkLabel          _labelWindowMode;
        private DarkLabel          _labelWindowWidth;
        private DarkLabel          _labelWindowHeight;
        private DarkNumericUpDown  _numericWindowWidth;
        private DarkNumericUpDown  _numericWindowHeight;
        private DarkRadioButton    _radioButtonFullscreen;
        private DarkRadioButton    _radioButtonWindowed;
        private DarkRadioButton    _radioButtonBorderless;
        private NotifyIcon         _trayIcon;
        private DarkContextMenu    _trayMenu;
        private ToolStripMenuItem  _trayMenuItemOpen;
        private ToolStripMenuItem  _trayMenuItemExit;
        private ToolStripSeparator _trayMenuSeparator;
    }
}