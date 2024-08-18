using EFT;
using EFT.UI;
using HarmonyLib;
using JetBrains.Annotations;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace ConfigHUD
{
    public class ConfigHUDPatch3 : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => AccessTools.Method(typeof(EftBattleUIScreen), nameof(EftBattleUIScreen.Show), new System.Type[] { typeof(GamePlayerOwner) });

        [PatchPostfix]
        public static void Postfix(EftBattleUIScreen __instance)
        {
            GameObject magnificationPanel = __instance.gameObject.transform.Find("OpticCratePanel/Value")?.gameObject;
            ConfigHUDController controller = __instance.GetOrAddComponent<ConfigHUDController>();
        }
    }
}