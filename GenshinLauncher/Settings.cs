// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;
using System.IO;

namespace GenshinLauncher
{
    public class Settings
    {
        // Registry settings
        public Setting<int>  ResolutionWidth  { get; }
        public Setting<int>  ResolutionHeight { get; }
        public Setting<bool> FullscreenMode   { get; }
        public Setting<int>  MonitorIndex     { get; }

        // ini settings
        public Setting<string> InstallPath       { get; }
        public Setting<string> BackgroundName    { get; }
        public Setting<string> EntryPoint        { get; }
        public Setting<bool>   CloseToTray       { get; }
        public Setting<bool>   BorderlessMode    { get; }
        public Setting<bool>   ExitOnLaunch      { get; }

        private readonly GenshinIni _genshinIni;
        private readonly string     _iniPath;

        public Settings()
        {
            _iniPath    = Path.Combine(AppContext.BaseDirectory, "config.ini");
            _genshinIni = new GenshinIni(_iniPath);

            // Load registry settings
            using (GenshinRegistry genshinRegistry = new GenshinRegistry(false))
            {
                FullscreenMode   = new Setting<bool>(genshinRegistry.FullscreenMode);
                ResolutionHeight = new Setting<int>(genshinRegistry.ResolutionHeight);
                ResolutionWidth  = new Setting<int>(genshinRegistry.ResolutionWidth);
                MonitorIndex     = new Setting<int>(genshinRegistry.MonitorIndex);
            }

            // Load ini settings
            InstallPath    = new Setting<string>(_genshinIni.GameInstallPath);
            BackgroundName = new Setting<string>(_genshinIni.GameDynamicBgName);
            EntryPoint     = new Setting<string>(_genshinIni.GameStartName);
            CloseToTray    = new Setting<bool>(_genshinIni.ExitType == "1");
            BorderlessMode = new Setting<bool>(Convert.ToBoolean(_genshinIni.BorderlessMode));
            ExitOnLaunch   = new Setting<bool>(Convert.ToBoolean(_genshinIni.ExitOnLaunch));
        }

        public void Save()
        {
            // Save registry settings
            using (GenshinRegistry genshinRegistry = new GenshinRegistry(true))
            {
                if (FullscreenMode.NewValue != FullscreenMode.CurrentValue)
                {
                    genshinRegistry.FullscreenMode = FullscreenMode.CurrentValue = FullscreenMode.NewValue;
                }

                if (ResolutionHeight.NewValue != ResolutionHeight.CurrentValue)
                {
                    genshinRegistry.ResolutionHeight = ResolutionHeight.CurrentValue = ResolutionHeight.NewValue;
                }

                if (ResolutionWidth.NewValue != ResolutionWidth.CurrentValue)
                {
                    genshinRegistry.ResolutionWidth = ResolutionWidth.CurrentValue = ResolutionWidth.NewValue;
                }

                if (MonitorIndex.NewValue != MonitorIndex.CurrentValue)
                {
                    genshinRegistry.MonitorIndex = MonitorIndex.CurrentValue = MonitorIndex.NewValue;
                }
            }

            // Save ini settings
            if (InstallPath.NewValue != InstallPath.CurrentValue)
            {
                _genshinIni.GameInstallPath = InstallPath.CurrentValue = InstallPath.NewValue;
            }

            if (BackgroundName.NewValue != BackgroundName.CurrentValue)
            {
                _genshinIni.GameDynamicBgName = BackgroundName.CurrentValue = BackgroundName.NewValue;
            }

            if (EntryPoint.NewValue != EntryPoint.CurrentValue)
            {
                _genshinIni.GameStartName = EntryPoint.CurrentValue = EntryPoint.NewValue;
            }

            if (CloseToTray.NewValue != CloseToTray.CurrentValue)
            {
                // ReSharper disable once AssignmentInConditionalExpression
                _genshinIni.ExitType = (CloseToTray.CurrentValue = CloseToTray.NewValue) ? "1" : "2";
            }

            if (BorderlessMode.NewValue != BorderlessMode.CurrentValue)
            {
                // ReSharper disable once AssignmentInConditionalExpression
                _genshinIni.BorderlessMode = (BorderlessMode.CurrentValue = BorderlessMode.NewValue) ? "true" : "false";
            }

            if (ExitOnLaunch.NewValue != ExitOnLaunch.CurrentValue)
            {
                // ReSharper disable once AssignmentInConditionalExpression
                _genshinIni.ExitOnLaunch = (ExitOnLaunch.CurrentValue = ExitOnLaunch.NewValue) ? "true" : "false";
            }

            _genshinIni.WriteFile(_iniPath);
        }

        public class Setting<T>
        {
            public T CurrentValue { get; internal set; } //TODO: This shouldn't be accessible from outside of the Settings class
            public T NewValue     { get; set; }

            public Setting(T currentValue)
            {
                CurrentValue = NewValue = currentValue;
            }
        }
    }
}