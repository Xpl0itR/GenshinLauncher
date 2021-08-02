// Copyright © 2021 Xpl0itR
//
// SPDX-License-Identifier: MPL-2.0

using System;

namespace GenshinLauncher
{
    public class Settings
    {
        //Registry settings
        public Setting<int>  ResolutionWidth  { get; }
        public Setting<int>  ResolutionHeight { get; }
        public Setting<bool> FullscreenMode   { get; }

        // ini settings
        public Setting<string> InstallPath    { get; }
        public Setting<string> EntryPoint     { get; }
        public Setting<bool>   BorderlessMode { get; }
        public Setting<bool>   CloseToTray    { get; }
        public Setting<bool>   ExitOnLaunch   { get; }

        private readonly SettingsIni _settingsIni;

        public Settings()
        {
            _settingsIni = new SettingsIni("config.ini");

            // Load registry settings
            using (GenshinRegistry genshinRegistry = new GenshinRegistry(false))
            {
                ResolutionWidth  = new Setting<int>(genshinRegistry.ResolutionWidth);
                ResolutionHeight = new Setting<int>(genshinRegistry.ResolutionHeight);
                FullscreenMode   = new Setting<bool>(genshinRegistry.FullscreenMode);
            }

            // Load ini settings
            InstallPath    = new Setting<string>(_settingsIni.GameInstallPath);
            EntryPoint     = new Setting<string>(_settingsIni.GameStartName);
            BorderlessMode = new Setting<bool>(Convert.ToBoolean(_settingsIni.BorderlessMode));
            CloseToTray    = new Setting<bool>(_settingsIni.ExitType == "1");
            ExitOnLaunch   = new Setting<bool>(Convert.ToBoolean(_settingsIni.ExitOnLaunch));
        }

        public void Save()
        {
            // Save registry settings
            using (GenshinRegistry genshinRegistry = new GenshinRegistry(true))
            {
                if (ResolutionWidth.NewValue != ResolutionWidth.CurrentValue)
                {
                    genshinRegistry.ResolutionWidth = ResolutionWidth.CurrentValue = ResolutionWidth.NewValue;
                }

                if (ResolutionHeight.NewValue != ResolutionHeight.CurrentValue)
                {
                    genshinRegistry.ResolutionHeight = ResolutionHeight.CurrentValue = ResolutionHeight.NewValue;
                }

                if (FullscreenMode.NewValue != FullscreenMode.CurrentValue)
                {
                    genshinRegistry.FullscreenMode = FullscreenMode.CurrentValue = FullscreenMode.NewValue;
                }
            }

            // Save ini settings
            if (InstallPath.NewValue != InstallPath.CurrentValue)
            {
                _settingsIni.GameInstallPath = InstallPath.CurrentValue = InstallPath.NewValue;
            }

            if (EntryPoint.NewValue != EntryPoint.CurrentValue)
            {
                _settingsIni.GameStartName = EntryPoint.CurrentValue = EntryPoint.NewValue;
            }

            if (BorderlessMode.NewValue != BorderlessMode.CurrentValue)
            {
                // ReSharper disable once AssignmentInConditionalExpression
                _settingsIni.BorderlessMode = (BorderlessMode.CurrentValue = BorderlessMode.NewValue) ? "true" : "false";
            }

            if (CloseToTray.NewValue != CloseToTray.CurrentValue)
            {
                // ReSharper disable once AssignmentInConditionalExpression
                _settingsIni.ExitType = (CloseToTray.CurrentValue = CloseToTray.NewValue) ? "1" : "2";
            }

            if (ExitOnLaunch.NewValue != ExitOnLaunch.CurrentValue)
            {
                // ReSharper disable once AssignmentInConditionalExpression
                _settingsIni.ExitOnLaunch = (ExitOnLaunch.CurrentValue = ExitOnLaunch.NewValue) ? "true" : "false";
            }

            _settingsIni.WriteFile("config.ini");
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