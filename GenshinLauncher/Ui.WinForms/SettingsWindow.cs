using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Forms;
using GenshinLauncher.Ui.Common;

namespace GenshinLauncher.Ui.WinForms
{
    public partial class SettingsWindow : DarkForm, ISettingsWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        public event EventHandler ButtonSaveClick
        {
            add    => _buttonSave.Click += value;
            remove => _buttonSave.Click -= value;
        }

        public bool CheckBoxCloseToTrayChecked
        {
            get => _checkBoxCloseToTray.Checked;
            set => _checkBoxCloseToTray.Checked = value;
        }

        public bool CheckBoxExitOnLaunchChecked
        {
            get => _checkBoxExitOnLaunch.Checked;
            set => _checkBoxExitOnLaunch.Checked = value;
        }

        public bool RadioButtonFullscreenChecked
        {
            get => _radioButtonFullscreen.Checked;
            set => _radioButtonFullscreen.Checked = value;
        }

        public bool RadioButtonBorderlessChecked
        {
            get => _radioButtonBorderless.Checked;
            set => _radioButtonBorderless.Checked = value;
        }

        public bool RadioButtonWindowedChecked
        {
            get => _radioButtonWindowed.Checked;
            set => _radioButtonWindowed.Checked = value;
        }

        public int NumericMonitorIndexMaximum
        {
            get => (int)_numericMonitorIndex.Maximum;
            set => _numericMonitorIndex.Maximum = value;
        }

        public int NumericMonitorIndexValue
        {
            get => (int)_numericMonitorIndex.Value;
            set => _numericMonitorIndex.Value = value;
        }

        public int NumericWindowHeightValue
        {
            get => (int)_numericWindowHeight.Value;
            set => _numericWindowHeight.Value = value;
        }

        public int NumericWindowWidthValue
        {
            get => (int)_numericWindowWidth.Value;
            set => _numericWindowWidth.Value = value;
        }

        public string TextBoxGameDirText
        {
            get => _textBoxInstallDir.Text;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _textBoxInstallDir.Select();
                }
                else
                {
                    _textBoxInstallDir.Text = value;
                }
            }
        }

        public void ShowDialog(IMainWindow parent) =>
            base.ShowDialog((Form)parent);

        private void ButtonClose_Click(object sender, EventArgs args) =>
            this.Close();
        
        private void ButtonUseScreenResolution_Click(object sender, EventArgs args)
        {
            Rectangle bounds = Screen.FromControl(this).Bounds;

            _numericWindowWidth.Value  = bounds.Width;
            _numericWindowHeight.Value = bounds.Height;
        }

        private void ButtonInstallDir_Click(object sender, EventArgs args)
        {
            if (_folderBrowser.ShowDialog() == DialogResult.OK)
            {
                _textBoxInstallDir.Text = _folderBrowser.SelectedPath;
            }
        }

        private void TextBoxInstallDir_Validating(object sender, CancelEventArgs args)
        {
            if (Utils.IsFolderPathValid(_textBoxInstallDir.Text))
            {
                return;
            }

            args.Cancel = true;
            _errorProvider.SetError(_textBoxInstallDir, "Invalid characters in path"); //TODO: localize text
        }

        private void TextBoxInstallDir_Validated(object sender, EventArgs args)
        {
            _errorProvider.SetError(_textBoxInstallDir, null);
        }
    }
}