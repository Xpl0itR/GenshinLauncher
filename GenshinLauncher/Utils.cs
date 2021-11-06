// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.IO;

namespace GenshinLauncher;

public static class Utils
{
    public const int DefaultFileStreamBufferSize = 0x1000;

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