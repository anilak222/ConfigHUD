using EFT;
using EFT.UI;
using HarmonyLib;
using JetBrains.Annotations;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace ConfigHUD
{
    public class ConfigHUDPatch4 : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => AccessTools.Method(typeof(EftBattleUIScreen), nameof(EftBattleUIScreen.Show), new System.Type[] { typeof(GamePlayerOwner) });

        [PatchPostfix]
        public static void Postfix(EftBattleUIScreen __instance)
        {
            GameObject bodyparts = __instance.gameObject.transform.Find("CharacterHealthPanel/BodyParts")?.gameObject;
            GameObject bodypartsbg = __instance.gameObject.transform.Find("CharacterHealthPanel/Background")?.gameObject;
            GameObject effectspanel = __instance.gameObject.transform.Find("CharacterHealthPanel/EffectsPanel")?.gameObject;
            ConfigHUDController controller = __instance.GetOrAddComponent<ConfigHUDController>();
        }
    }
}
