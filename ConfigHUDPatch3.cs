using EFT;
using EFT.UI;
using HarmonyLib;
using JetBrains.Annotations;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace ConfigHUD
{
    internal class ConfigHUDPatch3 : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => AccessTools.Method(typeof(LocalGame), "Stop");

        public static bool GameHasStopped = false;

        [PatchPrefix]
        public static void Prefix()
        {
            GameHasStopped = true;
        }
    }
}
