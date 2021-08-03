// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GenshinLauncher
{
    public class GenshinProcess : IDisposable
    {
        private readonly Process _genshinProcess;
        
        public GenshinProcess(string installDirectory, string entryPoint)
        {
            if (!Directory.Exists(installDirectory))
            {
                throw new DirectoryNotFoundException();
            }

            string exePath = Path.Join(installDirectory, entryPoint);

            if (!File.Exists(exePath))
            {
                throw new FileNotFoundException();
            }

            _genshinProcess = new Process();
            _genshinProcess.StartInfo.FileName = exePath;
        }

        public void Start()
        {
            string genshinProcessName = Path.GetFileNameWithoutExtension(_genshinProcess.StartInfo.FileName);
            if (Process.GetProcesses().Any(process => process.ProcessName == genshinProcessName))
            {
                throw new InvalidOperationException();
            }

            _genshinProcess.Start();
        }

        public void RemoveWindowsTitlebar()
        {
            IntPtr hWnd  = GetMainWindowHandle();
            long   style = WinApi.GetWindowLong(hWnd, WinApi.GWL_STYLE);

            WinApi.SetWindowLong(hWnd, WinApi.GWL_STYLE, style & ~WinApi.WS_CAPTION & ~WinApi.WS_THICKFRAME);
        }

        public void ResizeToFillScreen(Screen screen = null)
        {
            IntPtr    hWnd   = GetMainWindowHandle();
            Rectangle bounds = screen?.Bounds ?? Screen.FromHandle(hWnd).Bounds;

            WinApi.SetWindowPos(hWnd, IntPtr.Zero, bounds.X, bounds.Y, bounds.Width, bounds.Height, WinApi.SWP_FRAMECHANGED);
        }

        private IntPtr GetMainWindowHandle()
        {
            IntPtr handle = _genshinProcess.MainWindowHandle;
            while (handle == IntPtr.Zero)
            {
                handle = _genshinProcess.MainWindowHandle;
            }

            return handle;
        }

        public void Dispose()
        {
            _genshinProcess?.Dispose();
        }
    }
}