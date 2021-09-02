// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace GenshinLauncher
{
    public static class Utils
    {
        public const int DefaultFileStreamBufferSize = 4096;
        
        private static MethodInfo? _extractRelativeToDirectory;

        [DynamicDependency(DynamicallyAccessedMemberTypes.NonPublicMethods, "System.IO.Compression.ZipFileExtensions", "System.IO.Compression.ZipFile")]
        public static void ExtractToDirectory(this ZipArchive zipArchive, string destination)
        {
            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                _extractRelativeToDirectory ??= typeof(ZipFileExtensions).GetMethod("ExtractRelativeToDirectory", BindingFlags.NonPublic | BindingFlags.Static)!;
                _extractRelativeToDirectory.Invoke(null, new object[] { entry, destination, true });
            }
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

        public static async Task<Stream> OpenStreamAsync(this HttpContent content, CancellationToken cancellationToken = default)
        {
            Stream contentStream = await content.ReadAsStreamAsync(cancellationToken);
            return new LengthStream(contentStream, content.Headers.ContentLength);
        }

        public static async Task CopyToAsync(this Stream inStream, Stream outStream, HashAlgorithm? hashAlgorithm = null, IProgress<double>? progress = null, CancellationToken cancellationToken = default)
        {
            await inStream.IterateAsync((buffer, bytesRead) =>
            {
                hashAlgorithm?.TransformBlock(buffer, 0, bytesRead, null, 0);
                return outStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
            }, progress, cancellationToken);

            hashAlgorithm?.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        }

        public static async Task HashStreamAsync(this HashAlgorithm hashAlgorithm, Stream inStream, IProgress<double>? progress = null, CancellationToken cancellationToken = default, bool doFinal = true)
        {
            await inStream.IterateAsync((buffer, bytesRead) =>
            {
                hashAlgorithm.TransformBlock(buffer, 0, bytesRead, null, 0);
                return Task.CompletedTask;
            }, progress, cancellationToken);
            
            if (doFinal)
            {
                hashAlgorithm.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            }
        }

        public static async Task IterateAsync(this Stream stream, Func<byte[], int, Task> func, IProgress<double>? progress = null, CancellationToken cancellationToken = default)
        {
            long   totalBytes     = progress == null ? 0 : stream.Length;
            double totalBytesRead = 0;

            byte[] buffer = ArrayPool<byte>.Shared.Rent(DefaultFileStreamBufferSize);
            try
            {
                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken)) > 0)
                {
                    await func(buffer, bytesRead);

                    if (progress != null)
                    {
                        totalBytesRead += bytesRead;
                        progress.Report(totalBytesRead / totalBytes);
                    }
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
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