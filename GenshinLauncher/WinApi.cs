// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GenshinLauncher;

public static class WinApi
{
    private const string DllNameUser32 = "User32.dll";

    // ReSharper disable InconsistentNaming, IdentifierTypo
    public const int GWL_STYLE                = -16;
    public const int SM_CMONITORS             = 80;
    public const int SWP_FRAMECHANGED         = 0x0020;
    public const int WS_CAPTION               = 0x00C00000;
    public const int WS_THICKFRAME            = 0x00040000;
    public const int MONITOR_DEFAULTTONEAREST = 0x00000002;

    // ReSharper disable UnassignedField.Global, UnusedMember.Global
    [StructLayout(LayoutKind.Sequential)]
    public class MONITORINFO
    {
        public int       cbSize = Marshal.SizeOf(typeof(MONITORINFO));
        public int       dwFlags;
        public Rectangle rcMonitor;
        public Rectangle rcWork;
    }
    // ReSharper restore InconsistentNaming, IdentifierTypo, UnassignedField.Global, UnusedMember.Global

    [DllImport(DllNameUser32, EntryPoint = "GetWindowLongPtr")]
    public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport(DllNameUser32, EntryPoint = "SetWindowLongPtr")]
    public static extern long SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

    [DllImport(DllNameUser32)] public static extern int GetSystemMetrics(int nIndex);

    [DllImport(DllNameUser32)] public static extern IntPtr MonitorFromWindow(IntPtr hWnd, int dwFlags);

    [DllImport(DllNameUser32, SetLastError = true)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

    [DllImport(DllNameUser32, SetLastError = true)]
    public static extern bool GetMonitorInfo(IntPtr hMonitor, [In, Out] MONITORINFO mi);

    public static MONITORINFO GetMonitorInfo(IntPtr hMonitor)
    {
        MONITORINFO monitorInfo = new();

        if (!GetMonitorInfo(hMonitor, monitorInfo))
        {
            throw new Win32Exception();
        }

        return monitorInfo;
    }
}