// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GenshinLauncher
{
    public static class Utils
    {
        public const int DefaultFileStreamBufferSize = 0x1000;

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
                        byte[] buffer = ArrayPool<byte>.Shared.Rent(DefaultFileStreamBufferSize);
                        try
                        {
                            int bytesRead;
                            while ((bytesRead = await entryStream.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) > 0)
                            {
                                await outStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);

                                if (progress != null)
                                {
                                    decompressedLength += bytesRead;
                                    progress.Report(decompressedLength / uncompressedLength);
                                }
                            }
                        }
                        finally
                        {
                            ArrayPool<byte>.Shared.Return(buffer);
                        }
                    }

                    File.SetLastWriteTime(entryPath, entry.LastWriteTime.DateTime);
                }
            }
        }

        public static bool IsFolderPathValid(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

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