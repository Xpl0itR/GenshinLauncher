// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Reflection;

namespace GenshinLauncher
{
    public static class Reflection
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
    }
}