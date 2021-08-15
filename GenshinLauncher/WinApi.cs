// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Runtime.InteropServices;

namespace GenshinLauncher
{
    public static class WinApi
    {
        // ReSharper disable InconsistentNaming, IdentifierTypo
        public const int  GWL_STYLE        = -16;
        public const long WS_CAPTION       = 0x00C00000L;
        public const long WS_THICKFRAME    = 0x00040000L;
        public const uint SWP_FRAMECHANGED = 0x0020U;
        // ReSharper restore InconsistentNaming, IdentifierTypo

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
    }
}