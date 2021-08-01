// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System.ComponentModel;
using System.Windows.Forms;
using DarkUI.Controls;

namespace GenshinLauncher
{
    partial class MainForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this._trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this._trayMenu = new DarkUI.Controls.DarkContextMenu();
            this._trayMenuItemOpen = new System.Windows.Forms.ToolStripMenuItem();
            this._trayMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this._trayMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this._buttonLaunch = new DarkUI.Controls.DarkButton();
            this._checkBoxCloseToTray = new DarkUI.Controls.DarkCheckBox();
            this._checkBoxExitOnLaunch = new DarkUI.Controls.DarkCheckBox();
            this._comboBoxWindowMode = new DarkUI.Controls.DarkComboBox();
            this._labelWindowMode = new DarkUI.Controls.DarkLabel();
            this._labelWindowWidth = new DarkUI.Controls.DarkLabel();
            this._labelWindowHeight = new DarkUI.Controls.DarkLabel();
            this._numericWindowWidth = new DarkUI.Controls.DarkNumericUpDown();
            this._numericWindowHeight = new DarkUI.Controls.DarkNumericUpDown();
            this._buttonUseScreenResolution = new DarkUI.Controls.DarkButton();
            this._trayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // _trayIcon
            // 
            this._trayIcon.ContextMenuStrip = this._trayMenu;
            this._trayIcon.Icon = System.Drawing.SystemIcons.Application;
            this._trayIcon.Text = "Genshin Impact Launcher";
            this._trayIcon.Visible = true;
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
            this._trayMenu.Size = new System.Drawing.Size(179, 55);
            // 
            // _trayMenuItemOpen
            // 
            this._trayMenuItemOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this._trayMenuItemOpen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._trayMenuItemOpen.Name = "_trayMenuItemOpen";
            this._trayMenuItemOpen.Size = new System.Drawing.Size(178, 22);
            this._trayMenuItemOpen.Text = "Open main window";
            this._trayMenuItemOpen.Click += new System.EventHandler(this.OpenTrayMenuItem_Click);
            // 
            // _trayMenuSeparator
            // 
            this._trayMenuSeparator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this._trayMenuSeparator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._trayMenuSeparator.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this._trayMenuSeparator.Name = "_trayMenuSeparator";
            this._trayMenuSeparator.Size = new System.Drawing.Size(175, 6);
            // 
            // _trayMenuItemExit
            // 
            this._trayMenuItemExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this._trayMenuItemExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._trayMenuItemExit.Name = "_trayMenuItemExit";
            this._trayMenuItemExit.Size = new System.Drawing.Size(178, 22);
            this._trayMenuItemExit.Text = "Exit";
            this._trayMenuItemExit.Click += new System.EventHandler(this.ExitTrayMenuItem_Click);
            // 
            // _buttonLaunch
            // 
            this._buttonLaunch.Location = new System.Drawing.Point(1025, 612);
            this._buttonLaunch.Name = "_buttonLaunch";
            this._buttonLaunch.Padding = new System.Windows.Forms.Padding(5);
            this._buttonLaunch.Size = new System.Drawing.Size(192, 62);
            this._buttonLaunch.TabIndex = 0;
            this._buttonLaunch.Text = "Launch";
            this._buttonLaunch.Click += new System.EventHandler(this.LaunchButton_Click);
            // 
            // _checkBoxCloseToTray
            // 
            this._checkBoxCloseToTray.AutoSize = true;
            this._checkBoxCloseToTray.Location = new System.Drawing.Point(1174, 12);
            this._checkBoxCloseToTray.Name = "_checkBoxCloseToTray";
            this._checkBoxCloseToTray.Size = new System.Drawing.Size(93, 19);
            this._checkBoxCloseToTray.TabIndex = 3;
            this._checkBoxCloseToTray.Text = "Close to Tray";
            this._checkBoxCloseToTray.CheckedChanged += new System.EventHandler(this.CheckBoxCloseToTray_CheckedChanged);
            // 
            // _checkBoxExitOnLaunch
            // 
            this._checkBoxExitOnLaunch.AutoSize = true;
            this._checkBoxExitOnLaunch.Location = new System.Drawing.Point(1174, 37);
            this._checkBoxExitOnLaunch.Name = "_checkBoxExitOnLaunch";
            this._checkBoxExitOnLaunch.Size = new System.Drawing.Size(104, 19);
            this._checkBoxExitOnLaunch.TabIndex = 4;
            this._checkBoxExitOnLaunch.Text = "Exit on Launch";
            this._checkBoxExitOnLaunch.CheckedChanged += new System.EventHandler(this.CheckBoxExitOnLaunch_CheckedChanged);
            // 
            // _comboBoxWindowMode
            // 
            this._comboBoxWindowMode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this._comboBoxWindowMode.FormattingEnabled = true;
            this._comboBoxWindowMode.Items.AddRange(new object[] {
            "Exclusive Fullscreen",
            "Windowed",
            "Borderless Windowed"});
            this._comboBoxWindowMode.Location = new System.Drawing.Point(104, 10);
            this._comboBoxWindowMode.Name = "_comboBoxWindowMode";
            this._comboBoxWindowMode.Size = new System.Drawing.Size(143, 24);
            this._comboBoxWindowMode.TabIndex = 5;
            this._comboBoxWindowMode.SelectionChangeCommitted += new System.EventHandler(this.ComboBoxWindowMode_SelectionChangeCommitted);
            // 
            // _labelWindowMode
            // 
            this._labelWindowMode.AutoSize = true;
            this._labelWindowMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowMode.Location = new System.Drawing.Point(12, 13);
            this._labelWindowMode.Name = "_labelWindowMode";
            this._labelWindowMode.Size = new System.Drawing.Size(88, 15);
            this._labelWindowMode.TabIndex = 6;
            this._labelWindowMode.Text = "Window Mode:";
            // 
            // _labelWindowWidth
            // 
            this._labelWindowWidth.AutoSize = true;
            this._labelWindowWidth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowWidth.Location = new System.Drawing.Point(11, 44);
            this._labelWindowWidth.Name = "_labelWindowWidth";
            this._labelWindowWidth.Size = new System.Drawing.Size(89, 15);
            this._labelWindowWidth.TabIndex = 7;
            this._labelWindowWidth.Text = "Window Width:";
            // 
            // _labelWindowHeight
            // 
            this._labelWindowHeight.AutoSize = true;
            this._labelWindowHeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowHeight.Location = new System.Drawing.Point(8, 73);
            this._labelWindowHeight.Name = "_labelWindowHeight";
            this._labelWindowHeight.Size = new System.Drawing.Size(93, 15);
            this._labelWindowHeight.TabIndex = 8;
            this._labelWindowHeight.Text = "Window Height:";
            // 
            // _numericWindowWidth
            // 
            this._numericWindowWidth.Location = new System.Drawing.Point(104, 41);
            this._numericWindowWidth.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._numericWindowWidth.Name = "_numericWindowWidth";
            this._numericWindowWidth.Size = new System.Drawing.Size(143, 23);
            this._numericWindowWidth.TabIndex = 9;
            this._numericWindowWidth.ValueChanged += new System.EventHandler(this.NumericWindowWidth_ValueChanged);
            // 
            // _numericWindowHeight
            // 
            this._numericWindowHeight.Location = new System.Drawing.Point(104, 71);
            this._numericWindowHeight.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._numericWindowHeight.Name = "_numericWindowHeight";
            this._numericWindowHeight.Size = new System.Drawing.Size(143, 23);
            this._numericWindowHeight.TabIndex = 10;
            this._numericWindowHeight.ValueChanged += new System.EventHandler(this.NumericWindowHeight_ValueChanged);
            // 
            // _buttonUseScreenResolution
            // 
            this._buttonUseScreenResolution.Location = new System.Drawing.Point(104, 100);
            this._buttonUseScreenResolution.Name = "_buttonUseScreenResolution";
            this._buttonUseScreenResolution.Padding = new System.Windows.Forms.Padding(5);
            this._buttonUseScreenResolution.Size = new System.Drawing.Size(143, 23);
            this._buttonUseScreenResolution.TabIndex = 12;
            this._buttonUseScreenResolution.Text = "Use screen resolution";
            this._buttonUseScreenResolution.Click += new System.EventHandler(this.ButtonUseScreenResolution_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this._buttonLaunch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1280, 730);
            this.Controls.Add(this._buttonUseScreenResolution);
            this.Controls.Add(this._numericWindowHeight);
            this.Controls.Add(this._numericWindowWidth);
            this.Controls.Add(this._labelWindowHeight);
            this.Controls.Add(this._labelWindowWidth);
            this.Controls.Add(this._labelWindowMode);
            this.Controls.Add(this._comboBoxWindowMode);
            this.Controls.Add(this._checkBoxExitOnLaunch);
            this.Controls.Add(this._checkBoxCloseToTray);
            this.Controls.Add(this._buttonLaunch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Genshin Impact Launcher";
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
        private NotifyIcon         _trayIcon;
        private DarkContextMenu    _trayMenu;
        private ToolStripMenuItem  _trayMenuItemOpen;
        private ToolStripMenuItem  _trayMenuItemExit;
        private ToolStripSeparator _trayMenuSeparator;
        private DarkCheckBox       _checkBoxCloseToTray;
        private DarkCheckBox       _checkBoxExitOnLaunch;
        private DarkComboBox       _comboBoxWindowMode;
        private DarkLabel          _labelWindowMode;
        private DarkLabel          _labelWindowWidth;
        private DarkLabel          _labelWindowHeight;
        private DarkNumericUpDown  _numericWindowWidth;
        private DarkNumericUpDown  _numericWindowHeight;
    }
}