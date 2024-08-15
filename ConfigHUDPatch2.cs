using EFT;
using EFT.UI;
using HarmonyLib;
using JetBrains.Annotations;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace ConfigHUD
{
    public class ConfigHUDPatch2 : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => AccessTools.Method(typeof(EftBattleUIScreen), nameof(EftBattleUIScreen.Show), new System.Type[] { typeof(GamePlayerOwner) });

        [PatchPostfix]
        public static void Postfix(EftBattleUIScreen __instance)
        {
            Transform firemodetext = __instance.transform.Find("AmmoPanel/Ammo");
            ConfigHUDController controller = __instance.GetOrAddComponent<ConfigHUDController>();
            controller.Initialize2(firemodetext);
        }
    }
}
