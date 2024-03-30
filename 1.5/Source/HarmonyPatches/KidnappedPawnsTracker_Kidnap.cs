using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;



namespace VREArchon
{


    [HarmonyPatch(typeof(KidnappedPawnsTracker))]
    [HarmonyPatch("Kidnap")]
    public static class VREArchon_KidnappedPawnsTracker_Kidnap_Patch
    {
        [HarmonyPostfix]
        static void TurnIntoArchon(Pawn pawn, Faction ___faction)
        {


            if (___faction.def == VREA_DefOf.VRE_Archons) {

                pawn.genes.SetXenotype(VREA_DefOf.VRE_Archon);
            
            }

        }
    }








}
