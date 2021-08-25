// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;

namespace GenshinLauncher
{
    public static class Utils
    {
        private static MethodInfo? _extractRelativeToDirectory;

        [DynamicDependency(DynamicallyAccessedMemberTypes.NonPublicMethods, "System.IO.Compression.ZipFileExtensions", "System.IO.Compression.ZipFile")]
        public static void ExtractRelativeToDirectory(this ZipArchiveEntry source, string destinationDirectoryName, bool overwrite)
        {
            _extractRelativeToDirectory ??= typeof(ZipFileExtensions).GetMethod("ExtractRelativeToDirectory", BindingFlags.NonPublic | BindingFlags.Static)!;
            _extractRelativeToDirectory.Invoke(null, new object[] { source, destinationDirectoryName, overwrite });
        }

        public static async Task<IntPtr> GetMainWindowHandle(this Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            while (handle == IntPtr.Zero)
            {
                await Task.Delay(300);
                handle = process.MainWindowHandle;
            }

            return handle;
        }

        public static void RemoveWindowTitlebar(IntPtr hWnd)
        {
            long style = WinApi.GetWindowLong(hWnd, WinApi.GWL_STYLE) 
                       & ~WinApi.WS_CAPTION
                       & ~WinApi.WS_THICKFRAME;

            WinApi.SetWindowLong(hWnd, WinApi.GWL_STYLE, style);
        }

        public static void ResizeWindowToFillScreen(IntPtr hWnd)
        {
            IntPtr    hMonitor = WinApi.MonitorFromWindow(hWnd, WinApi.MONITOR_DEFAULTTONEAREST);
            Rectangle bounds   = WinApi.GetMonitorInfo(hMonitor).rcMonitor;

            WinApi.SetWindowPos(hWnd, IntPtr.Zero, bounds.X, bounds.Y, bounds.Width, bounds.Height, WinApi.SWP_FRAMECHANGED);
        }

        public static bool IsFolderPathValid(string path)
        {
            try
            {
                _ = new DirectoryInfo(path).Attributes;
                return true;
            }
            catch (Exception exception) when (exception is ArgumentNullException or IOException)
            {
                return false;
            }
        }
    }
}