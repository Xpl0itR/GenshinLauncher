// Copyright © 2022 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.ComponentModel;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;

namespace Windows.Win32
{
    internal static partial class PInvoke
    {
        internal static RECT GetMonitorRect(HMONITOR hMonitor)
        {
            MONITORINFO monitorInfo = new();

            if (!GetMonitorInfo(hMonitor, ref monitorInfo))
            {
                throw new Win32Exception("Failed to retrieve information about the display monitor.");
            }

            return monitorInfo.rcMonitor;
        }
    }
}

namespace Windows.Win32.Foundation
{
    internal readonly partial struct HWND
    {
        public static readonly HWND Zero = new(IntPtr.Zero);
    }
}