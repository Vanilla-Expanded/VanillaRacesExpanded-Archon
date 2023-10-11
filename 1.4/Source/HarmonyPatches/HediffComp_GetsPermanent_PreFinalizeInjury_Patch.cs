using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;
using static RimWorld.FleshTypeDef;

namespace VREArchon
{
    [HarmonyPatch(typeof(HediffComp_GetsPermanent), "PreFinalizeInjury")]
    public static class HediffComp_GetsPermanent_PreFinalizeInjury_Patch
    {
        public static bool Prefix(HediffComp_GetsPermanent __instance)
        {
            if (__instance.Def == VREA_DefOf.VREA_Sear)
            {
                __instance.IsPermanent = true;
                return false;
            }
            return true;
        }
    }
}
