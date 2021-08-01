// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System.IO;
using System.Text;
using IniParser;
using IniParser.Model;

namespace GenshinLauncher
{
    public class SettingsIni
    {
        public string GameInstallPath
        {
            get => Launcher["game_install_path"];
            set => Launcher["game_install_path"] = value;
        }

        public string GameStartName
        {
            get => Launcher["game_start_name"];
            set => Launcher["game_start_name"] = value;
        }

        public string ExitType
        {
            get => Launcher["exit_type"];
            set => Launcher["exit_type"] = value;
        }

        public string BorderlessMode
        {
            get => CustomLauncher["borderless_mode"];
            set => CustomLauncher["borderless_mode"] = value;
        }

        public string ExitOnLaunch
        {
            get => CustomLauncher["exit_on_launch"];
            set => CustomLauncher["exit_on_launch"] = value;
        }

        private KeyDataCollection Launcher       => _data["launcher"];
        private KeyDataCollection CustomLauncher => _data["GenshinLauncher"]; //TODO: rename after v1

        private readonly FileIniDataParser _parser;
        private readonly IniData           _data;

        public SettingsIni(string path)
        {
            _parser = new FileIniDataParser();
            _data   = File.Exists(path) 
                ? _parser.ReadFile(path, Encoding.UTF8) 
                : new IniData();
        }

        public void WriteFile(string path) => _parser.WriteFile(path, _data, Encoding.UTF8);
    }
}