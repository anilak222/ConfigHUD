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
            GameObject ammoPanel = __instance.gameObject.transform.Find("AmmoPanel/Ammo")?.gameObject;
            GameObject magnificationPanel = __instance.gameObject.transform.Find("OpticCratePanel/Value")?.gameObject;
            GameObject bodyparts = __instance.gameObject.transform.Find("CharacterHealthPanel/BodyParts")?.gameObject;
            GameObject bodypartsbg = __instance.gameObject.transform.Find("CharacterHealthPanel/Background")?.gameObject;
            GameObject effectspanel = __instance.gameObject.transform.Find("CharacterHealthPanel/EffectsPanel")?.gameObject;
            ConfigHUDController controller = __instance.GetOrAddComponent<ConfigHUDController>();
            controller.Initialize2(ammoPanel, magnificationPanel, bodyparts, bodypartsbg, effectspanel);
        }
    }
}