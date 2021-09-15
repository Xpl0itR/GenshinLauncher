// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace GenshinLauncher
{
    public static class Utils
    {
        public const int DefaultFileStreamBufferSize = 0x1000;

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
            Task HashAndWrite(byte[] buffer, int bytesRead)
            {
                hashAlgorithm?.TransformBlock(buffer, 0, bytesRead, null, 0);
                return outStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
            }

            await (progress == null
                ? inStream.IterateAsync(HashAndWrite, cancellationToken)
                : inStream.IterateAsync(HashAndWrite, progress, cancellationToken));

            hashAlgorithm?.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        }

        public static async Task HashStreamAsync(this HashAlgorithm hashAlgorithm, Stream inStream, IProgress<double>? progress = null, CancellationToken cancellationToken = default, bool doFinal = true)
        {
            Task Hash(byte[] buffer, int bytesRead)
            {
                hashAlgorithm.TransformBlock(buffer, 0, bytesRead, null, 0);
                return Task.CompletedTask;
            }

            await (progress == null 
                ? inStream.IterateAsync(Hash, cancellationToken) 
                : inStream.IterateAsync(Hash, progress, cancellationToken));

            if (doFinal)
            {
                hashAlgorithm.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            }
        }

        public static async Task ExtractToDirectory(this ZipArchive zipArchive, string destinationPath, IProgress<double>? progress = null, CancellationToken cancellationToken = default)
        {
            destinationPath = Directory.CreateDirectory(destinationPath).FullName;
            long   uncompressedLength = progress == null ? 0 : zipArchive.Entries.Aggregate(0L, (current, entry) => current + entry.Length);
            double decompressedLength = 0;

            foreach (ZipArchiveEntry entry in zipArchive.Entries)
            {
                cancellationToken.ThrowIfCancellationRequested();

                string entryPath = Path.GetFullPath(Path.Combine(destinationPath, entry.FullName));

                if (!entryPath.StartsWith(destinationPath, StringComparison.OrdinalIgnoreCase))
                {
                    throw new IOException("Zip entry path is outside the destination directory");
                }

                if (entry.Name == string.Empty)
                {
                    if (entry.Length > 0)
                    {
                        throw new IOException("Zip entry is supposed to be a folder but contains data");
                    }

                    Directory.CreateDirectory(entryPath);
                }
                else
                {
                    await using (Stream outStream   = new FileStream(entryPath, FileMode.Create, FileAccess.Write, FileShare.None, DefaultFileStreamBufferSize, true))
                    await using (Stream entryStream = entry.Open())
                    {
                        await entryStream.IterateAsync((buffer, bytesRead) =>
                        {
                            if (progress != null)
                            {
                                decompressedLength += bytesRead;
                                progress.Report(decompressedLength / uncompressedLength);
                            }

                            return outStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                        }, cancellationToken);
                    }

                    File.SetLastWriteTime(entryPath, entry.LastWriteTime.DateTime);
                }
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

        private static Task IterateAsync(this Stream stream, Func<byte[], int, Task> func, IProgress<double> progress, CancellationToken cancellationToken)
        {
            long   totalBytes     = stream.Length;
            double totalBytesRead = 0;

            return stream.IterateAsync((buffer, bytesRead) =>
            {
                totalBytesRead += bytesRead;
                progress.Report(totalBytesRead / totalBytes);

                return func(buffer, bytesRead);
            }, cancellationToken);
        }

        private static async Task IterateAsync(this Stream stream, Func<byte[], int, Task> func, CancellationToken cancellationToken)
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(DefaultFileStreamBufferSize);
            try
            {
                int bytesRead;
                while ((bytesRead = await stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) > 0)
                {
                    await func(buffer, bytesRead);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
    }
}