// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GenshinLauncher
{
    public static class WinApiUtils
    {
        // ReSharper disable InconsistentNaming
        private const int  GWL_STYLE        = -16;
        private const long WS_CAPTION       = 0x00C00000L;
        private const long WS_THICKFRAME    = 0x00040000L;
        private const uint SWP_FRAMECHANGED = 0x0020U;
        // ReSharper restore InconsistentNaming

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        public static bool RemoveTitleBar(IntPtr hWnd)
        {
            long style = GetWindowLong(hWnd, GWL_STYLE);
            return SetWindowLong(hWnd, GWL_STYLE, style & ~WS_CAPTION & ~WS_THICKFRAME) == 0;
        }

        public static bool ResizeToFullscreen(IntPtr hWnd)
        {
            Rectangle bounds = Screen.FromHandle(hWnd).Bounds;
            return SetWindowPos(hWnd, IntPtr.Zero, bounds.X, bounds.Y, bounds.Width, bounds.Height, SWP_FRAMECHANGED);
        }
    }
}