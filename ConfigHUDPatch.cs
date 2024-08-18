using System;
using UnityEngine;
using System.Collections;
using System.Reflection;
using Comfort.Common;
using EFT;
using EFT.UI;
using HarmonyLib;
using JetBrains.Annotations;
using SPT.Reflection.Patching;

namespace ConfigHUD
{
    public class ConfigHUDPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => AccessTools.Method(typeof(BattleStancePanel), nameof(BattleStancePanel.method_3));

        [PatchPostfix]
        public static void Postfix(BattleStancePanel __instance)
        {
            Transform stances = __instance.transform.Find("Stances");
            Transform stanceslider = __instance.transform.Find("StanceSlider");
            Transform volumeslider = __instance.transform.Find("SpeedSlider");
            Transform volumeicon = __instance.transform.Find("SprintBar/NoiseLevel");
            ConfigHUDController controller = __instance.GetOrAddComponent<ConfigHUDController>();
            controller.Initialize(stances, stanceslider, volumeslider, volumeicon);
        }
    }
}
