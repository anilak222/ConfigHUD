using System;
using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using JetBrains.Annotations;
using UnityEngine;

namespace ConfigHUD
{
    [BepInPlugin("config.hud", "Config.HUD", "0.4")]
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
            ConfigHUDPlugin.stanceVisibility = base.Config.Bind<bool>("Enable/Disable", "Stance Panel", true);
            ConfigHUDPlugin.heightVisibility = base.Config.Bind<bool>("Enable/Disable", "Height Slider", true);
            ConfigHUDPlugin.volumeVisibility = base.Config.Bind<bool>("Enable/Disable", "Volume Slider", true);
            ConfigHUDPlugin.ammopanelVisibility = base.Config.Bind<bool>("Enable/Disable", "Ammo Panel", true);
            ConfigHUDPlugin.magnificationpanelVisibility = base.Config.Bind<bool>("Enable/Disable", "Magnification Panel", true);
            ConfigHUDPlugin.bodypartsVisibility = base.Config.Bind<bool>("Enable/Disable", "Body Parts Panel", true);
            ConfigHUDPlugin.gesturespanelVisibility = base.Config.Bind<bool>("Enable/Disable", "Quick Gestures Panel", true);

            ConfigHUDPlugin.elevationUp = base.Config.Bind<KeyboardShortcut>("Optional Keybinds", "Elevation Up", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "Set this to the keybind you usually use for elevation up");
            ConfigHUDPlugin.elevationDown = base.Config.Bind<KeyboardShortcut>("Optional Keybinds", "Elevation Down", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "Set this to the keybind you usually use for elevation down");
            ConfigHUDPlugin.switchFireMode = base.Config.Bind<KeyboardShortcut>("Optional Keybinds", "Switch Fire Mode", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "Set this to the key bind you usually use to switch fire mode");
            ConfigHUDPlugin.checkFireMode = base.Config.Bind<KeyboardShortcut>("Optional Keybinds", "Check Fire Mode", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "Set this to the key bind you usually use to check fire mode");
            ConfigHUDPlugin.checkAmmo = base.Config.Bind<KeyboardShortcut>("Optional Keybinds", "Check Ammo", new KeyboardShortcut(KeyCode.None, Array.Empty<KeyCode>()), "Set this to the key bind you usually use to check ammunition");

            ConfigHUDPlugin.heightSliderPosX = base.Config.Bind("Positioning Settings", "Height Slider Position X", 44.2f, new ConfigDescription("The X position of the height slider.", new AcceptableValueRange<float>(30f, 1930f)));
            ConfigHUDPlugin.heightSliderPosY = base.Config.Bind("Positioning Settings", "Height Slider Position Y", -181.3f, new ConfigDescription("The Y position of the height slider.", new AcceptableValueRange<float>(-260f, 665f)));
            ConfigHUDPlugin.volumeSliderPosX = base.Config.Bind("Positioning Settings", "Volume Slider Position X", 51.2f, new ConfigDescription("The X position of the volume slider.", new AcceptableValueRange<float>(5f, 1770f)));
            ConfigHUDPlugin.volumeSliderPosY = base.Config.Bind("Positioning Settings", "Volume Slider Position Y", -189.5f, new ConfigDescription("The Y position of the volume slider.", new AcceptableValueRange<float>(-245f, 1000f)));
            ConfigHUDPlugin.volumeIconPosX = base.Config.Bind("Positioning Settings", "Volume Icon Position X", -91f, new ConfigDescription("The X position of the volume icon.", new AcceptableValueRange<float>(-100f, 1790f)));
            ConfigHUDPlugin.volumeIconPosY = base.Config.Bind("Positioning Settings", "Volume Icon Position Y", 20.3f, new ConfigDescription("The Y position of the volume icon.", new AcceptableValueRange<float>(-30f, 1030f)));
            ConfigHUDPlugin.sprintBarPosX = base.Config.Bind("Positioning Settings", "Stamina Bar Position X", 33.6f, new ConfigDescription("The X position of the stamina bar.", new AcceptableValueRange<float>(-10f, 1750f)));
            ConfigHUDPlugin.sprintBarPosY = base.Config.Bind("Positioning Settings", "Stamina Bar Position Y", -222.5f, new ConfigDescription("The Y position of the stamina bar.", new AcceptableValueRange<float>(-260f, 810f)));
            ConfigHUDPlugin.energyBarPosX = base.Config.Bind("Positioning Settings", "Energy Bar Position X", 33.6f, new ConfigDescription("The X position of the energy bar.", new AcceptableValueRange<float>(-10f, 1750f)));
            ConfigHUDPlugin.energyBarPosY = base.Config.Bind("Positioning Settings", "Energy Bar Position Y", -213.6f, new ConfigDescription("The Y position of the energy bar.", new AcceptableValueRange<float>(-260f, 810f)));

            ConfigHUDPlugin.heightSliderScaleX = base.Config.Bind("Resizing Settings", "Height Slider Width", 1f, new ConfigDescription("The width of the height slider.", new AcceptableValueRange<float>(1f, 2.5f)));
            ConfigHUDPlugin.heightSliderScaleY = base.Config.Bind("Resizing Settings", "Height Slider Height", 1f, new ConfigDescription("The height of the height slider.", new AcceptableValueRange<float>(1f, 2.5f)));
            ConfigHUDPlugin.volumeSliderScaleX = base.Config.Bind("Resizing Settings", "Volume Slider Width", 1f, new ConfigDescription("The width of the volume slider.", new AcceptableValueRange<float>(1f, 2.5f)));
            ConfigHUDPlugin.volumeSliderScaleY = base.Config.Bind("Resizing Settings", "Volume Slider Height", 1f, new ConfigDescription("The height of the volume slider.", new AcceptableValueRange<float>(1f, 2.5f)));
            ConfigHUDPlugin.volumeIconScaleX = base.Config.Bind("Resizing Settings", "Volume Icon Width", 1f, new ConfigDescription("The width of the volume icon.", new AcceptableValueRange<float>(1f, 1.5f)));
            ConfigHUDPlugin.volumeIconScaleY = base.Config.Bind("Resizing Settings", "Volume Icon Height", 1f, new ConfigDescription("The height of the volume icon.", new AcceptableValueRange<float>(1f, 1.5f)));
            ConfigHUDPlugin.energyBarWidth = base.Config.Bind("Resizing Settings", "Energy Bar Width", 155.8f, new ConfigDescription("The width of the energy bar.", new AcceptableValueRange<float>(155.8f, 350f)));
            ConfigHUDPlugin.energyBarHeight = base.Config.Bind("Resizing Settings", "Energy Bar Height", 12.9f, new ConfigDescription("The height of the energy bar.", new AcceptableValueRange<float>(12.9f, 20f)));
            ConfigHUDPlugin.sprintBarWidth = base.Config.Bind("Resizing Settings", "Stamina Bar Width", 155.8f, new ConfigDescription("The width of the stamina bar.", new AcceptableValueRange<float>(155.8f, 350f)));
            ConfigHUDPlugin.sprintBarHeight = base.Config.Bind("Resizing Settings", "Stamina Bar Height", 12.9f, new ConfigDescription("The height of the stamina bar.", new AcceptableValueRange<float>(12.9f, 20f)));
        }

        private const string KeybindSectionName = "Optional Keybinds";

        private const string PositioningSettingsName = "Positioning Settings";

        private const string ResizingSettingsName = "Resizing Settings";

        internal static ConfigEntry<KeyboardShortcut> checkAmmo;

        internal static ConfigEntry<KeyboardShortcut> checkFireMode;

        internal static ConfigEntry<KeyboardShortcut> switchFireMode;

        internal static ConfigEntry<KeyboardShortcut> elevationUp;

        internal static ConfigEntry<KeyboardShortcut> elevationDown;

        internal static ConfigEntry<bool> stanceVisibility;

        internal static ConfigEntry<bool> heightVisibility;

        internal static ConfigEntry<bool> volumeVisibility;

        internal static ConfigEntry<bool> ammopanelVisibility;

        internal static ConfigEntry<bool> magnificationpanelVisibility;

        internal static ConfigEntry<bool> bodypartsVisibility;

        internal static ConfigEntry<bool> gesturespanelVisibility;

        internal static ConfigEntry<float> heightSliderPosX;

        internal static ConfigEntry<float> heightSliderPosY;

        internal static ConfigEntry<float> heightSliderScaleX;

        internal static ConfigEntry<float> heightSliderScaleY;

        internal static ConfigEntry<float> volumeSliderPosX;

        internal static ConfigEntry<float> volumeSliderPosY;

        internal static ConfigEntry<float> volumeSliderScaleX;

        internal static ConfigEntry<float> volumeSliderScaleY;

        internal static ConfigEntry<float> volumeIconPosX;

        internal static ConfigEntry<float> volumeIconPosY;

        internal static ConfigEntry<float> volumeIconScaleX;

        internal static ConfigEntry<float> volumeIconScaleY;

        internal static ConfigEntry<float> sprintBarPosX;

        internal static ConfigEntry<float> sprintBarPosY;

        internal static ConfigEntry<float> sprintBarWidth;

        internal static ConfigEntry<float> sprintBarHeight;

        internal static ConfigEntry<float> energyBarPosX;

        internal static ConfigEntry<float> energyBarPosY;

        internal static ConfigEntry<float> energyBarWidth;

        internal static ConfigEntry<float> energyBarHeight;

    }
}