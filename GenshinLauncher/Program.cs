// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;
using System.Windows.Forms;
using DarkUI.Win32;

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
            Settings settings = new Settings();
            
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.AddMessageFilter(new ControlScrollFilter());
            Application.Run(new MainForm(settings));
        }
    }
}