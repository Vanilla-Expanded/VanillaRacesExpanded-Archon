
using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Collections.Generic;


namespace VREArchon
{
    [HarmonyPatch(typeof(JobGiver_OptimizeApparel), "ApparelScoreGain")]
    internal class VREArchon_JobGiver_OptimizeApparel_ApparelScoreGain_Postfix
    {


        [HarmonyPostfix]
        private static void PostFix(ref float __result, Pawn pawn, Apparel ap)
        {
            if (VREArchon_EquipmentUtility_CanEquip_Postfix.blockedWeapons.Contains(ap.def) && pawn.genes?.HasActiveGene(VREA_DefOf.VRE_Transcendent)!=true)
            {
                __result = -1000;

            }


        }
    }
}