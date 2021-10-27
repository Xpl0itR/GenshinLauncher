namespace GenshinLauncher.Ui.WinForms
{
    partial class SettingsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this._groupBoxSettings = new DarkUI.Controls.DarkGroupBox();
            this._labelWindowWidth = new DarkUI.Controls.DarkLabel();
            this._labelWindowMode = new DarkUI.Controls.DarkLabel();
            this._labelMonitorIndex = new DarkUI.Controls.DarkLabel();
            this._labelWindowHeight = new DarkUI.Controls.DarkLabel();
            this._numericMonitorIndex = new DarkUI.Controls.DarkNumericUpDown();
            this._numericWindowWidth = new DarkUI.Controls.DarkNumericUpDown();
            this._radioButtonBorderless = new DarkUI.Controls.DarkRadioButton();
            this._numericWindowHeight = new DarkUI.Controls.DarkNumericUpDown();
            this._radioButtonWindowed = new DarkUI.Controls.DarkRadioButton();
            this._buttonUseScreenResolution = new DarkUI.Controls.DarkButton();
            this._radioButtonFullscreen = new DarkUI.Controls.DarkRadioButton();
            this._buttonInstallDirectory = new DarkUI.Controls.DarkButton();
            this._textBoxInstallDir = new DarkUI.Controls.DarkTextBox();
            this._checkBoxCloseToTray = new DarkUI.Controls.DarkCheckBox();
            this._checkBoxExitOnLaunch = new DarkUI.Controls.DarkCheckBox();
            this._groupBoxLauncherSettings = new DarkUI.Controls.DarkGroupBox();
            this._groupBoxInstallPath = new DarkUI.Controls.DarkGroupBox();
            this._folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._buttonClose = new DarkUI.Controls.DarkButton();
            this._buttonSave = new DarkUI.Controls.DarkButton();
            this._groupBoxSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numericMonitorIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowHeight)).BeginInit();
            this._groupBoxLauncherSettings.SuspendLayout();
            this._groupBoxInstallPath.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
            this.SuspendLayout();
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
            // _labelWindowWidth
            // 
            resources.ApplyResources(this._labelWindowWidth, "_labelWindowWidth");
            this._labelWindowWidth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowWidth.Name = "_labelWindowWidth";
            // 
            // _labelWindowMode
            // 
            resources.ApplyResources(this._labelWindowMode, "_labelWindowMode");
            this._labelWindowMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowMode.Name = "_labelWindowMode";
            // 
            // _labelMonitorIndex
            // 
            resources.ApplyResources(this._labelMonitorIndex, "_labelMonitorIndex");
            this._labelMonitorIndex.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelMonitorIndex.Name = "_labelMonitorIndex";
            // 
            // _labelWindowHeight
            // 
            resources.ApplyResources(this._labelWindowHeight, "_labelWindowHeight");
            this._labelWindowHeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowHeight.Name = "_labelWindowHeight";
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
            // _radioButtonBorderless
            // 
            resources.ApplyResources(this._radioButtonBorderless, "_radioButtonBorderless");
            this._radioButtonBorderless.Name = "_radioButtonBorderless";
            this._radioButtonBorderless.TabStop = true;
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
            // _radioButtonWindowed
            // 
            resources.ApplyResources(this._radioButtonWindowed, "_radioButtonWindowed");
            this._radioButtonWindowed.Name = "_radioButtonWindowed";
            this._radioButtonWindowed.TabStop = true;
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
            // 
            // _buttonInstallDirectory
            // 
            this._buttonInstallDirectory.CausesValidation = false;
            resources.ApplyResources(this._buttonInstallDirectory, "_buttonInstallDirectory");
            this._buttonInstallDirectory.Name = "_buttonInstallDirectory";
            this._buttonInstallDirectory.Click += new System.EventHandler(this.ButtonInstallDir_Click);
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
            // _groupBoxLauncherSettings
            // 
            this._groupBoxLauncherSettings.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this._groupBoxLauncherSettings.Controls.Add(this._checkBoxCloseToTray);
            this._groupBoxLauncherSettings.Controls.Add(this._checkBoxExitOnLaunch);
            resources.ApplyResources(this._groupBoxLauncherSettings, "_groupBoxLauncherSettings");
            this._groupBoxLauncherSettings.Name = "_groupBoxLauncherSettings";
            this._groupBoxLauncherSettings.TabStop = false;
            // 
            // _groupBoxInstallPath
            // 
            this._groupBoxInstallPath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this._groupBoxInstallPath.Controls.Add(this._textBoxInstallDir);
            this._groupBoxInstallPath.Controls.Add(this._buttonInstallDirectory);
            resources.ApplyResources(this._groupBoxInstallPath, "_groupBoxInstallPath");
            this._groupBoxInstallPath.Name = "_groupBoxInstallPath";
            this._groupBoxInstallPath.TabStop = false;
            // 
            // _errorProvider
            // 
            this._errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this._errorProvider.ContainerControl = this;
            // 
            // _buttonClose
            // 
            resources.ApplyResources(this._buttonClose, "_buttonClose");
            this._buttonClose.Name = "_buttonClose";
            this._buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // _buttonSave
            // 
            resources.ApplyResources(this._buttonSave, "_buttonSave");
            this._buttonSave.Name = "_buttonSave";
            // 
            // SettingsWindow
            // 
            this.AcceptButton = this._buttonSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._buttonClose;
            this.Controls.Add(this._buttonSave);
            this.Controls.Add(this._buttonClose);
            this.Controls.Add(this._groupBoxInstallPath);
            this.Controls.Add(this._groupBoxLauncherSettings);
            this.Controls.Add(this._groupBoxSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsWindow_FormClosing);
            this._groupBoxSettings.ResumeLayout(false);
            this._groupBoxSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._numericMonitorIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._numericWindowHeight)).EndInit();
            this._groupBoxLauncherSettings.ResumeLayout(false);
            this._groupBoxLauncherSettings.PerformLayout();
            this._groupBoxInstallPath.ResumeLayout(false);
            this._groupBoxInstallPath.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkGroupBox _groupBoxSettings;
        private DarkUI.Controls.DarkLabel _labelWindowWidth;
        private DarkUI.Controls.DarkLabel _labelWindowMode;
        private DarkUI.Controls.DarkLabel _labelMonitorIndex;
        private DarkUI.Controls.DarkLabel _labelWindowHeight;
        private DarkUI.Controls.DarkNumericUpDown _numericMonitorIndex;
        private DarkUI.Controls.DarkNumericUpDown _numericWindowWidth;
        private DarkUI.Controls.DarkRadioButton _radioButtonBorderless;
        private DarkUI.Controls.DarkNumericUpDown _numericWindowHeight;
        private DarkUI.Controls.DarkRadioButton _radioButtonWindowed;
        private DarkUI.Controls.DarkButton _buttonUseScreenResolution;
        private DarkUI.Controls.DarkRadioButton _radioButtonFullscreen;
        private DarkUI.Controls.DarkButton _buttonInstallDirectory;
        private DarkUI.Controls.DarkTextBox _textBoxInstallDir;
        private DarkUI.Controls.DarkCheckBox _checkBoxCloseToTray;
        private DarkUI.Controls.DarkCheckBox _checkBoxExitOnLaunch;
        private DarkUI.Controls.DarkGroupBox _groupBoxLauncherSettings;
        private DarkUI.Controls.DarkGroupBox _groupBoxInstallPath;
        private System.Windows.Forms.FolderBrowserDialog _folderBrowser;
        private System.Windows.Forms.ErrorProvider _errorProvider;
        private DarkUI.Controls.DarkButton _buttonSave;
        private DarkUI.Controls.DarkButton _buttonClose;
    }
}