// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Drawing;
using System.IO;
using GenshinLauncher.Ui.Common;
using GenshinLauncher.Ui.WinForms;

namespace GenshinLauncher
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            IUserInterface ui         = new UserInterface();
            IMainWindow    mainWindow = new MainWindow();
            Launcher       launcher   = new Launcher();

            mainWindow.GameDirectoryUpdate += (_, newPath) =>
            {
                launcher.GameInstallDir = newPath.Replace(@"\", "/");

                if (File.Exists(Path.Combine(launcher.GameInstallDir, Launcher.ExeNameGlobal)))
                {
                    launcher.EntryPoint = Launcher.ExeNameGlobal;
                    mainWindow.ShowButtonLaunch();
                }
                else if (File.Exists(Path.Combine(launcher.GameInstallDir, Launcher.ExeNameChina)))
                {
                    launcher.EntryPoint = Launcher.ExeNameChina;
                    mainWindow.ShowButtonLaunch();
                }
                else
                {
                    launcher.EntryPoint = string.Empty;
                    mainWindow.ShowButtonDownload();
                }
            };

            mainWindow.ButtonLaunchClick += (_, _) =>
            {
                launcher.SaveLauncherConfig();

                try
                {
                    launcher.StartGame();

                    if (launcher.BorderlessMode)
                    {
                        launcher.RemoveGameTitlebar();
                        launcher.ResizeGameToFillBounds();
                    }
                }
                catch (InvalidOperationException)
                {
                    mainWindow.ShowErrorProcessAlreadyRunning();
                    return;
                }

                if (launcher.ExitOnLaunch)
                {
                    ui.Exit();
                }
            };

            mainWindow.ButtonDownloadClick += (_, _) =>
                OnMainWindowOnButtonDownloadClick(mainWindow, launcher);

            mainWindow.ButtonUseScreenResolutionClick += (_, _) =>
            {
                Rectangle bounds = mainWindow.GetCurrentScreenBounds();

                mainWindow.NumericWindowWidthValue = bounds.Width;
                launcher.ResolutionWidth.SetValue(bounds.Width);

                mainWindow.NumericWindowHeightValue = bounds.Height;
                launcher.ResolutionHeight.SetValue(bounds.Height);
            };

            mainWindow.WindowModeCheckedChanged += (_, _) =>
            {
                if (mainWindow.RadioButtonBorderlessChecked)
                {
                    launcher.BorderlessMode = true;
                    launcher.FullscreenMode.SetValue(false);
                }
                else
                {
                    launcher.BorderlessMode = false;
                    launcher.FullscreenMode.SetValue(mainWindow.RadioButtonFullscreenChecked);
                }
            };

            mainWindow.NumericWindowWidthValueChanged += (_, _) =>
            {
                launcher.ResolutionWidth.SetValue(mainWindow.NumericWindowWidthValue);
            };

            mainWindow.NumericWindowHeightValueChanged += (_, _) =>
            {
                launcher.ResolutionHeight.SetValue(mainWindow.NumericWindowHeightValue);
            };

            mainWindow.NumericMonitorIndexValueChanged += (_, _) =>
            {
                launcher.MonitorIndex.SetValue(mainWindow.NumericMonitorIndexValue);
            };

            mainWindow.CheckBoxCloseToTrayCheckedChanged += (_, _) =>
            {
                launcher.CloseToTray = mainWindow.CheckBoxCloseToTrayChecked;
            };

            mainWindow.CheckBoxExitOnLaunchCheckedChanged += (_, _) =>
            {
                launcher.ExitOnLaunch = mainWindow.CheckBoxExitOnLaunchChecked;
            };

            mainWindow.CheckBoxCloseToTrayChecked   = launcher.CloseToTray;
            mainWindow.CheckBoxExitOnLaunchChecked  = launcher.ExitOnLaunch;
            mainWindow.TextBoxGameDirText           = launcher.GameInstallDir;
            mainWindow.NumericMonitorIndexValue     = (int)launcher.MonitorIndex;

            if (launcher.BorderlessMode)
            {
                mainWindow.RadioButtonBorderlessChecked = true;
            }
            else if ((bool)launcher.FullscreenMode)
            {
                mainWindow.RadioButtonFullscreenChecked = true;
            }
            else
            {
                mainWindow.RadioButtonWindowedChecked = true;
            }

            if (launcher.ResolutionHeight == 0 || launcher.ResolutionWidth == 0)
            {
                Rectangle bounds = mainWindow.GetCurrentScreenBounds();
                mainWindow.NumericWindowWidthValue  = bounds.Width;
                mainWindow.NumericWindowHeightValue = bounds.Height;
            }
            else
            {
                mainWindow.NumericWindowWidthValue  = (int)launcher.ResolutionWidth;
                mainWindow.NumericWindowHeightValue = (int)launcher.ResolutionHeight;
            }

            if (!File.Exists(launcher.EntryPointPath))
            {
                mainWindow.GroupBoxSettingsEnabled = false;
                mainWindow.ShowButtonDownload();
            }

            if (File.Exists(launcher.BackgroundPath))
            {
                mainWindow.BackgroundImage = Image.FromFile(launcher.BackgroundPath);
            }

            LoadAdditionalContentAsync(mainWindow, launcher);

            ui.Run(mainWindow);
        }

        private static async void LoadAdditionalContentAsync(IMainWindow mainWindow, Launcher launcher)
        {
            (string bgName, string bgMd5, Banner[] _, Post[] _, Stream? bgStream) = await launcher.GetAdditionalContent("en-us"); //TODO: load this from CultureInfo

            launcher.BackgroundMd5      = bgMd5;
            launcher.BackgroundFileName = bgName;
            launcher.SaveLauncherConfig();

            if (bgStream != null)
            {
                mainWindow.BackgroundImage = Image.FromStream(bgStream);
            }

            //TODO: implement banner and post viewers into UI
        }

        private static async void OnMainWindowOnButtonDownloadClick(IMainWindow mainWindow, Launcher launcher)
        {
            launcher.SaveLauncherConfig();

            mainWindow.ButtonLaunchEnabled = false;
            mainWindow.ShowButtonLaunch();
            //mainWindow.ShowProgressBar();

            await launcher.DownloadLatestVersion(mainWindow.RadioButtonGlobalVersionChecked);

            mainWindow.ShowInstallPath();
            mainWindow.ButtonLaunchEnabled = true;
        }
    }
}