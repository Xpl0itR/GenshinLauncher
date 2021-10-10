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
            this._groupBoxSettings.Location = new System.Drawing.Point(12, 12);
            this._groupBoxSettings.Name = "_groupBoxSettings";
            this._groupBoxSettings.Size = new System.Drawing.Size(255, 198);
            this._groupBoxSettings.TabIndex = 19;
            this._groupBoxSettings.TabStop = false;
            this._groupBoxSettings.Text = "Extended Settings";
            // 
            // _labelWindowWidth
            // 
            this._labelWindowWidth.AutoSize = true;
            this._labelWindowWidth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowWidth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._labelWindowWidth.Location = new System.Drawing.Point(6, 27);
            this._labelWindowWidth.Name = "_labelWindowWidth";
            this._labelWindowWidth.Size = new System.Drawing.Size(86, 15);
            this._labelWindowWidth.TabIndex = 7;
            this._labelWindowWidth.Text = "Window Width";
            // 
            // _labelWindowMode
            // 
            this._labelWindowMode.AutoSize = true;
            this._labelWindowMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._labelWindowMode.Location = new System.Drawing.Point(6, 121);
            this._labelWindowMode.Name = "_labelWindowMode";
            this._labelWindowMode.Size = new System.Drawing.Size(85, 15);
            this._labelWindowMode.TabIndex = 6;
            this._labelWindowMode.Text = "Window Mode";
            // 
            // _labelMonitorIndex
            // 
            this._labelMonitorIndex.AutoSize = true;
            this._labelMonitorIndex.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelMonitorIndex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._labelMonitorIndex.Location = new System.Drawing.Point(6, 86);
            this._labelMonitorIndex.Name = "_labelMonitorIndex";
            this._labelMonitorIndex.Size = new System.Drawing.Size(50, 15);
            this._labelMonitorIndex.TabIndex = 17;
            this._labelMonitorIndex.Text = "Monitor";
            // 
            // _labelWindowHeight
            // 
            this._labelWindowHeight.AutoSize = true;
            this._labelWindowHeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._labelWindowHeight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._labelWindowHeight.Location = new System.Drawing.Point(6, 56);
            this._labelWindowHeight.Name = "_labelWindowHeight";
            this._labelWindowHeight.Size = new System.Drawing.Size(90, 15);
            this._labelWindowHeight.TabIndex = 8;
            this._labelWindowHeight.Text = "Window Height";
            // 
            // _numericMonitorIndex
            // 
            this._numericMonitorIndex.Location = new System.Drawing.Point(62, 83);
            this._numericMonitorIndex.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this._numericMonitorIndex.Name = "_numericMonitorIndex";
            this._numericMonitorIndex.Size = new System.Drawing.Size(37, 23);
            this._numericMonitorIndex.TabIndex = 16;
            // 
            // _numericWindowWidth
            // 
            this._numericWindowWidth.Location = new System.Drawing.Point(102, 24);
            this._numericWindowWidth.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._numericWindowWidth.Name = "_numericWindowWidth";
            this._numericWindowWidth.Size = new System.Drawing.Size(143, 23);
            this._numericWindowWidth.TabIndex = 9;
            // 
            // _radioButtonBorderless
            // 
            this._radioButtonBorderless.AutoSize = true;
            this._radioButtonBorderless.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._radioButtonBorderless.Location = new System.Drawing.Point(102, 166);
            this._radioButtonBorderless.Name = "_radioButtonBorderless";
            this._radioButtonBorderless.Size = new System.Drawing.Size(139, 19);
            this._radioButtonBorderless.TabIndex = 15;
            this._radioButtonBorderless.TabStop = true;
            this._radioButtonBorderless.Text = "Borderless Windowed";
            // 
            // _numericWindowHeight
            // 
            this._numericWindowHeight.Location = new System.Drawing.Point(102, 54);
            this._numericWindowHeight.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this._numericWindowHeight.Name = "_numericWindowHeight";
            this._numericWindowHeight.Size = new System.Drawing.Size(143, 23);
            this._numericWindowHeight.TabIndex = 10;
            // 
            // _radioButtonWindowed
            // 
            this._radioButtonWindowed.AutoSize = true;
            this._radioButtonWindowed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._radioButtonWindowed.Location = new System.Drawing.Point(102, 143);
            this._radioButtonWindowed.Name = "_radioButtonWindowed";
            this._radioButtonWindowed.Size = new System.Drawing.Size(82, 19);
            this._radioButtonWindowed.TabIndex = 14;
            this._radioButtonWindowed.TabStop = true;
            this._radioButtonWindowed.Text = "Windowed";
            // 
            // _buttonUseScreenResolution
            // 
            this._buttonUseScreenResolution.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._buttonUseScreenResolution.Location = new System.Drawing.Point(102, 83);
            this._buttonUseScreenResolution.Name = "_buttonUseScreenResolution";
            this._buttonUseScreenResolution.Padding = new System.Windows.Forms.Padding(5);
            this._buttonUseScreenResolution.Size = new System.Drawing.Size(143, 23);
            this._buttonUseScreenResolution.TabIndex = 12;
            this._buttonUseScreenResolution.Text = "Use screen resolution";
            this._buttonUseScreenResolution.Click += new System.EventHandler(this.ButtonUseScreenResolution_Click);
            // 
            // _radioButtonFullscreen
            // 
            this._radioButtonFullscreen.AutoSize = true;
            this._radioButtonFullscreen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._radioButtonFullscreen.Location = new System.Drawing.Point(102, 120);
            this._radioButtonFullscreen.Name = "_radioButtonFullscreen";
            this._radioButtonFullscreen.Size = new System.Drawing.Size(129, 19);
            this._radioButtonFullscreen.TabIndex = 13;
            this._radioButtonFullscreen.TabStop = true;
            this._radioButtonFullscreen.Text = "Exclusive Fullscreen";
            // 
            // _buttonInstallDirectory
            // 
            this._buttonInstallDirectory.CausesValidation = false;
            this._buttonInstallDirectory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._buttonInstallDirectory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._buttonInstallDirectory.Location = new System.Drawing.Point(9, 50);
            this._buttonInstallDirectory.Name = "_buttonInstallDirectory";
            this._buttonInstallDirectory.Padding = new System.Windows.Forms.Padding(5);
            this._buttonInstallDirectory.Size = new System.Drawing.Size(161, 26);
            this._buttonInstallDirectory.TabIndex = 22;
            this._buttonInstallDirectory.Text = "Change install path";
            this._buttonInstallDirectory.Click += new System.EventHandler(this.ButtonInstallDir_Click);
            // 
            // _textBoxInstallDir
            // 
            this._textBoxInstallDir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this._textBoxInstallDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._textBoxInstallDir.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._textBoxInstallDir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._textBoxInstallDir.Location = new System.Drawing.Point(9, 19);
            this._textBoxInstallDir.Name = "_textBoxInstallDir";
            this._textBoxInstallDir.Size = new System.Drawing.Size(501, 25);
            this._textBoxInstallDir.TabIndex = 21;
            this._textBoxInstallDir.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxInstallDir_Validating);
            this._textBoxInstallDir.Validated += new System.EventHandler(this.TextBoxInstallDir_Validated);
            // 
            // _checkBoxCloseToTray
            // 
            this._checkBoxCloseToTray.AutoSize = true;
            this._checkBoxCloseToTray.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._checkBoxCloseToTray.Location = new System.Drawing.Point(8, 18);
            this._checkBoxCloseToTray.Name = "_checkBoxCloseToTray";
            this._checkBoxCloseToTray.Padding = new System.Windows.Forms.Padding(0, 3, 11, 0);
            this._checkBoxCloseToTray.Size = new System.Drawing.Size(104, 22);
            this._checkBoxCloseToTray.TabIndex = 23;
            this._checkBoxCloseToTray.Text = "Close to Tray";
            // 
            // _checkBoxExitOnLaunch
            // 
            this._checkBoxExitOnLaunch.AutoSize = true;
            this._checkBoxExitOnLaunch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this._checkBoxExitOnLaunch.Location = new System.Drawing.Point(8, 40);
            this._checkBoxExitOnLaunch.Name = "_checkBoxExitOnLaunch";
            this._checkBoxExitOnLaunch.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this._checkBoxExitOnLaunch.Size = new System.Drawing.Size(104, 22);
            this._checkBoxExitOnLaunch.TabIndex = 24;
            this._checkBoxExitOnLaunch.Text = "Exit on Launch";
            // 
            // _groupBoxLauncherSettings
            // 
            this._groupBoxLauncherSettings.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this._groupBoxLauncherSettings.Controls.Add(this._checkBoxCloseToTray);
            this._groupBoxLauncherSettings.Controls.Add(this._checkBoxExitOnLaunch);
            this._groupBoxLauncherSettings.Location = new System.Drawing.Point(273, 13);
            this._groupBoxLauncherSettings.Name = "_groupBoxLauncherSettings";
            this._groupBoxLauncherSettings.Size = new System.Drawing.Size(255, 197);
            this._groupBoxLauncherSettings.TabIndex = 18;
            this._groupBoxLauncherSettings.TabStop = false;
            this._groupBoxLauncherSettings.Text = "Launcher Settings";
            // 
            // _groupBoxInstallPath
            // 
            this._groupBoxInstallPath.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this._groupBoxInstallPath.Controls.Add(this._textBoxInstallDir);
            this._groupBoxInstallPath.Controls.Add(this._buttonInstallDirectory);
            this._groupBoxInstallPath.Location = new System.Drawing.Point(12, 216);
            this._groupBoxInstallPath.Name = "_groupBoxInstallPath";
            this._groupBoxInstallPath.Size = new System.Drawing.Size(516, 87);
            this._groupBoxInstallPath.TabIndex = 19;
            this._groupBoxInstallPath.TabStop = false;
            this._groupBoxInstallPath.Text = "Game Install Path";
            // 
            // _errorProvider
            // 
            this._errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this._errorProvider.ContainerControl = this;
            // 
            // _buttonClose
            // 
            this._buttonClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._buttonClose.Location = new System.Drawing.Point(427, 315);
            this._buttonClose.Name = "_buttonClose";
            this._buttonClose.Padding = new System.Windows.Forms.Padding(5);
            this._buttonClose.Size = new System.Drawing.Size(101, 29);
            this._buttonClose.TabIndex = 20;
            this._buttonClose.Text = "Close";
            this._buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // _buttonSave
            // 
            this._buttonSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this._buttonSave.Location = new System.Drawing.Point(320, 315);
            this._buttonSave.Name = "_buttonSave";
            this._buttonSave.Padding = new System.Windows.Forms.Padding(5);
            this._buttonSave.Size = new System.Drawing.Size(101, 29);
            this._buttonSave.TabIndex = 21;
            this._buttonSave.Text = "Save";
            // 
            // SettingsWindow
            // 
            this.AcceptButton = this._buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._buttonClose;
            this.ClientSize = new System.Drawing.Size(541, 356);
            this.Controls.Add(this._buttonSave);
            this.Controls.Add(this._buttonClose);
            this.Controls.Add(this._groupBoxInstallPath);
            this.Controls.Add(this._groupBoxLauncherSettings);
            this.Controls.Add(this._groupBoxSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsWindow";
            this.Text = "Settings";
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