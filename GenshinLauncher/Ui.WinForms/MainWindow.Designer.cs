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
            this._numericMonitorIndex = new DarkUI.Controls.DarkNumericUpDown();
            this._labelMonitorIndex = new DarkUI.Controls.DarkLabel();
            this._groupBoxSettings = new DarkUI.Controls.DarkGroupBox();
            this._textBoxInstallDir = new DarkUI.Controls.DarkTextBox();
            this._buttonInstallDirectory = new DarkUI.Controls.DarkButton();
            this._folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._buttonDownload = new DarkUI.Controls.DarkButton();
            this._radioButtonGlobalVersion = new DarkUI.Controls.DarkRadioButton();
            this._radioButtonChinaVersion = new DarkUI.Controls.DarkRadioButton();
            this._progressBarDownload = new System.Windows.Forms.ProgressBar();
            this._labelProgressBarText = new DarkUI.Controls.DarkLabel();
            this._labelProgressBarTitle = new DarkUI.Controls.DarkLabel();
            this._trayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericMonitorIndex)).BeginInit();
            this._groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
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
            // 
            // _checkBoxCloseToTray
            // 
            resources.ApplyResources(this._checkBoxCloseToTray, "_checkBoxCloseToTray");
            this._checkBoxCloseToTray.Name = "_checkBoxCloseToTray";
            // 
            // _checkBoxExitOnLaunch
            // 
            resources.ApplyResources(this._checkBoxExitOnLaunch, "_checkBoxExitOnLaunch");
            this._checkBoxExitOnLaunch.Name = "_checkBoxExitOnLaunch";
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
            // 
            // _buttonUseScreenResolution
            // 
            resources.ApplyResources(this._buttonUseScreenResolution, "_buttonUseScreenResolution");
            this._buttonUseScreenResolution.Name = "_buttonUseScreenResolution";
            // 
            // _radioButtonFullscreen
            // 
            resources.ApplyResources(this._radioButtonFullscreen, "_radioButtonFullscreen");
            this._radioButtonFullscreen.Name = "_radioButtonFullscreen";
            this._radioButtonFullscreen.TabStop = true;
            // 
            // _radioButtonWindowed
            // 
            resources.ApplyResources(this._radioButtonWindowed, "_radioButtonWindowed");
            this._radioButtonWindowed.Name = "_radioButtonWindowed";
            this._radioButtonWindowed.TabStop = true;
            // 
            // _radioButtonBorderless
            // 
            resources.ApplyResources(this._radioButtonBorderless, "_radioButtonBorderless");
            this._radioButtonBorderless.Name = "_radioButtonBorderless";
            this._radioButtonBorderless.TabStop = true;
            // 
            // _numericMonitorIndex
            // 
            resources.ApplyResources(this._numericMonitorIndex, "_numericMonitorIndex");
            this._numericMonitorIndex.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._numericMonitorIndex.Name = "_numericMonitorIndex";
            // 
            // _labelMonitorIndex
            // 
            resources.ApplyResources(this._labelMonitorIndex, "_labelMonitorIndex");
            this._labelMonitorIndex.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelMonitorIndex.Name = "_labelMonitorIndex";
            // 
            // _groupBoxSettings
            // 
            this._groupBoxSettings.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this._groupBoxSettings.Controls.Add(this._labelWindowWidth);
            this._groupBoxSettings.Controls.Add(this._labelWindowMode);
            this._groupBoxSettings.Controls.Add(this._labelMonitorIndex);
            this._groupBoxSettings.Controls.Add(this._labelWindowHeight);
            this._groupBoxSettings.Controls.Add(this._numericMonitorIndex);
            this._groupBoxSettings.Controls.Add(this._numericWindowWidth);
            this._groupBoxSettings.Controls.Add(this._radioButtonBorderless);
            this._groupBoxSettings.Controls.Add(this._numericWindowHeight);
            this._groupBoxSettings.Controls.Add(this._radioButtonWindowed);
            this._groupBoxSettings.Controls.Add(this._buttonUseScreenResolution);
            this._groupBoxSettings.Controls.Add(this._radioButtonFullscreen);
            resources.ApplyResources(this._groupBoxSettings, "_groupBoxSettings");
            this._groupBoxSettings.Name = "_groupBoxSettings";
            this._groupBoxSettings.TabStop = false;
            // 
            // _textBoxInstallDir
            // 
            this._textBoxInstallDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this._textBoxInstallDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this._textBoxInstallDir, "_textBoxInstallDir");
            this._textBoxInstallDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._textBoxInstallDir.Name = "_textBoxInstallDir";
            this._textBoxInstallDir.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxInstallDir_Validating);
            this._textBoxInstallDir.Validated += new System.EventHandler(this.TextBoxInstallDir_Validated);
            // 
            // _buttonInstallDirectory
            // 
            this._buttonInstallDirectory.CausesValidation = false;
            resources.ApplyResources(this._buttonInstallDirectory, "_buttonInstallDirectory");
            this._buttonInstallDirectory.Name = "_buttonInstallDirectory";
            this._buttonInstallDirectory.Click += new System.EventHandler(this.ButtonInstallDir_Click);
            // 
            // _errorProvider
            // 
            this._errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this._errorProvider.ContainerControl = this;
            // 
            // _buttonDownload
            // 
            resources.ApplyResources(this._buttonDownload, "_buttonDownload");
            this._buttonDownload.Name = "_buttonDownload";
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
            // _progressBarDownload
            // 
            resources.ApplyResources(this._progressBarDownload, "_progressBarDownload");
            this._progressBarDownload.Name = "_progressBarDownload";
            // 
            // _labelProgressBarText
            // 
            resources.ApplyResources(this._labelProgressBarText, "_labelProgressBarText");
            this._labelProgressBarText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelProgressBarText.Name = "_labelProgressBarText";
            // 
            // _labelProgressBarTitle
            // 
            resources.ApplyResources(this._labelProgressBarTitle, "_labelProgressBarTitle");
            this._labelProgressBarTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelProgressBarTitle.Name = "_labelProgressBarTitle";
            // 
            // MainWindow
            // 
            this.AcceptButton = this._buttonLaunch;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._labelProgressBarTitle);
            this.Controls.Add(this._labelProgressBarText);
            this.Controls.Add(this._progressBarDownload);
            this.Controls.Add(this._radioButtonChinaVersion);
            this.Controls.Add(this._radioButtonGlobalVersion);
            this.Controls.Add(this._buttonDownload);
            this.Controls.Add(this._buttonInstallDirectory);
            this.Controls.Add(this._textBoxInstallDir);
            this.Controls.Add(this._checkBoxCloseToTray);
            this.Controls.Add(this._checkBoxExitOnLaunch);
            this.Controls.Add(this._groupBoxSettings);
            this.Controls.Add(this._buttonLaunch);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this._trayMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericMonitorIndex)).EndInit();
            this._groupBoxSettings.ResumeLayout(false);
            this._groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkButton _buttonLaunch;
        private DarkButton _buttonUseScreenResolution;
        private DarkCheckBox _checkBoxCloseToTray;
        private DarkCheckBox _checkBoxExitOnLaunch;
        private DarkLabel _labelWindowMode;
        private DarkLabel _labelWindowWidth;
        private DarkLabel _labelWindowHeight;
        private DarkNumericUpDown _numericWindowWidth;
        private DarkNumericUpDown _numericWindowHeight;
        private DarkRadioButton _radioButtonFullscreen;
        private DarkRadioButton _radioButtonWindowed;
        private DarkRadioButton _radioButtonBorderless;
        private NotifyIcon _trayIcon;
        private DarkContextMenu _trayMenu;
        private ToolStripMenuItem _trayMenuItemOpen;
        private ToolStripMenuItem _trayMenuItemExit;
        private ToolStripSeparator _trayMenuSeparator;
        private DarkNumericUpDown _numericMonitorIndex;
        private DarkLabel _labelMonitorIndex;
        private DarkGroupBox _groupBoxSettings;
        private DarkTextBox _textBoxInstallDir;
        private DarkButton _buttonInstallDirectory;
        private FolderBrowserDialog _folderBrowser;
        private ErrorProvider _errorProvider;
        private DarkButton _buttonDownload;
        private DarkRadioButton _radioButtonChinaVersion;
        private DarkRadioButton _radioButtonGlobalVersion;
        private ProgressBar _progressBarDownload;
        private DarkLabel _labelProgressBarText;
        private DarkLabel _labelProgressBarTitle;
    }
}