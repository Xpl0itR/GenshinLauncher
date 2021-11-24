// Copyright © 2021 Xpl0itR
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Text;
using IniParser;
using IniParser.Model;

namespace GenshinLauncher.FileParsers;

public abstract class Ini
{
    protected readonly IniData           Data;
    protected readonly FileIniDataParser Parser;

    protected Ini(string? path = null)
    {
        Parser = new FileIniDataParser();
        Data   = path == null
            ? new IniData()
            : Parser.ReadFile(path, Encoding.UTF8);
    }

    public void WriteFile(string path) =>
        Parser.WriteFile(path, Data, Encoding.UTF8);
}