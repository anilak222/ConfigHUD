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
            GameObject stances = __instance.gameObject.transform.Find("Stances")?.gameObject;
            GameObject stanceslider = __instance.gameObject.transform.Find("StanceSlider")?.gameObject;
            GameObject volumeslider = __instance.gameObject.transform.Find("SpeedSlider")?.gameObject;
            GameObject volumeicon = __instance.gameObject.transform.Find("SprintBar/NoiseLevel")?.gameObject;
            GameObject sprintbar = __instance.gameObject.transform.Find("SprintBar")?.gameObject;
            GameObject energybar = __instance.gameObject.transform.Find("HandsBar")?.gameObject;
            ConfigHUDController controller = __instance.GetOrAddComponent<ConfigHUDController>();
            controller.Initialize(stances, stanceslider, volumeslider, volumeicon, sprintbar, energybar);
        }
    }
}
