using System;
using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using JetBrains.Annotations;
using UnityEngine;

namespace ConfigHUD
{
    [BepInPlugin("config.hud", "Config.HUD", "0.2")]
    public class ConfigHUDPlugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger { get; private set; }
        internal void Start()
        {
            this.InitConfiguration();
            new ConfigHUDPatch().Enable();
            new ConfigHUDPatch2().Enable();
            new ConfigHUDPatch3().Enable();
        }

        private void InitConfiguration()
        {
            ConfigHUDPlugin.checkAmmo = base.Config.Bind<KeyboardShortcut>("Optional Keybinds", "Check Ammo", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "Set this to the key bind you usually use to check ammunition");
            ConfigHUDPlugin.checkFireMode = base.Config.Bind<KeyboardShortcut>("Optional Keybinds", "Check Fire Mode", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "Set this to the key bind you usually use to check fire mode");
            ConfigHUDPlugin.switchFireMode = base.Config.Bind<KeyboardShortcut>("Optional Keybinds", "Switch Fire Mode", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "Set this to the key bind you usually use to switch fire mode");
            ConfigHUDPlugin.aimDownSight1 = base.Config.Bind<KeyboardShortcut>("Optional Keybinds", "Aim on Continuous", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "Only use this if you have aim on continuous");
            ConfigHUDPlugin.aimDownSight2 = base.Config.Bind<KeyboardShortcut>("Optional Keybinds", "Aim on Press", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "Only use this if you have aim on press");
            ConfigHUDPlugin.stanceVisibility = base.Config.Bind<bool>("Enable/Disable", "Stance Panel Visibility", true);
            ConfigHUDPlugin.volumeVisibility = base.Config.Bind<bool>("Enable/Disable", "Volume Slider Visibility", true);
            ConfigHUDPlugin.ammopanelVisibility = base.Config.Bind<bool>("Enable/Disable", "Ammo Panel Visibility", true);
            ConfigHUDPlugin.magnificationpanelVisibility = base.Config.Bind<bool>("Enable/Disable", "Magnification Panel Visibility", true);
            ConfigHUDPlugin.bodypartsVisibility = base.Config.Bind<bool>("Enable/Disable", "Body Parts Panel Visibility", true);
        }

        private const string KeybindSectionName = "Optional Keybinds";

        internal static ConfigEntry<KeyboardShortcut> checkAmmo;

        internal static ConfigEntry<KeyboardShortcut> checkFireMode;

        internal static ConfigEntry<KeyboardShortcut> switchFireMode;

        internal static ConfigEntry<KeyboardShortcut> aimDownSight1;

        internal static ConfigEntry<KeyboardShortcut> aimDownSight2;

        internal static ConfigEntry<bool> stanceVisibility;

        internal static ConfigEntry<bool> volumeVisibility;

        internal static ConfigEntry<bool> ammopanelVisibility;

        internal static ConfigEntry<bool> magnificationpanelVisibility;

        internal static ConfigEntry<bool> bodypartsVisibility;

    }
}