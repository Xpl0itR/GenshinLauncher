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
using System.Security.Cryptography;

namespace GenshinLauncher
{
    public static class Utils
    {
        // ReSharper disable InconsistentNaming
        private static MethodInfo? _extractRelativeToDirectory;
        // ReSharper restore InconsistentNaming

        [DynamicDependency(DynamicallyAccessedMemberTypes.NonPublicMethods, "System.IO.Compression.ZipFileExtensions", "System.IO.Compression.ZipFile")]
        public static void ExtractRelativeToDirectory(this ZipArchiveEntry source, string destinationDirectoryName, bool overwrite)
        {
            _extractRelativeToDirectory ??= typeof(ZipFileExtensions).GetMethod("ExtractRelativeToDirectory", BindingFlags.NonPublic | BindingFlags.Static)!;
            _extractRelativeToDirectory.Invoke(null, new object?[] { source, destinationDirectoryName, overwrite });
        }

        public static IntPtr GetMainWindowHandle(this Process process)
        {
            IntPtr handle = process.MainWindowHandle;
            while (handle == IntPtr.Zero)
            {
                handle = process.MainWindowHandle;
            }

            return handle;
        }

        public static void RemoveWindowTitlebar(IntPtr hWnd)
        {
            long style = WinApi.GetWindowLong(hWnd, WinApi.GWL_STYLE);
            WinApi.SetWindowLong(hWnd, WinApi.GWL_STYLE, style & ~WinApi.WS_CAPTION & ~WinApi.WS_THICKFRAME);
        }

        public static void ResizeWindowToFillScreen(IntPtr hWnd)
        {
            Rectangle bounds = System.Windows.Forms.Screen.FromHandle(hWnd).Bounds; //TODO: replace this with a method that doesn't rely on WinForms
            WinApi.SetWindowPos(hWnd, IntPtr.Zero, bounds.X, bounds.Y, bounds.Width, bounds.Height, WinApi.SWP_FRAMECHANGED);
        }

        public static bool VerifyMd5(Stream stream, string expectedHash)
        {
            using MD5 md5Alg = MD5.Create();
            return Verify(stream, md5Alg, expectedHash);
        }

        public static bool Verify(Stream stream, HashAlgorithm hashAlgorithm, string expectedHash)
        {
            stream.Seek(0, SeekOrigin.Begin);
            byte[] hash = hashAlgorithm.ComputeHash(stream);

            return Convert.ToHexString(hash).Equals(expectedHash, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsFolderPathValid(string path) //TODO: Come up with a better method of validating paths
        {
            if (string.IsNullOrWhiteSpace(path) || path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                return false;
            }

            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}