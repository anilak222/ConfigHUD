using System;
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
        }

        private void InitConfiguration()
        {
            ConfigHUDPlugin.checkAmmo = base.Config.Bind<KeyboardShortcut>("Keybinds", "Check Ammo", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "The keyboard shortcut that you want to use to check ammunition");
            ConfigHUDPlugin.stanceVisibility = base.Config.Bind<bool>("Enable/Disable", "Stance Visibility", true);
            ConfigHUDPlugin.volumeVisibility = base.Config.Bind<bool>("Enable/Disable", "Volume Visibility", true);
            ConfigHUDPlugin.ammotextVisibility = base.Config.Bind<bool>("Enable/Disable", "Ammo Text Visibility", true);
        }

        private const string KeybindSectionName = "Keybinds";

        internal static ConfigEntry<KeyboardShortcut> checkAmmo;

        internal static ConfigEntry<bool> stanceVisibility;

        internal static ConfigEntry<bool> volumeVisibility;

        internal static ConfigEntry<bool> ammotextVisibility;
    }
}