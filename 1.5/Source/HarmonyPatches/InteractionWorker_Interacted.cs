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


    [HarmonyPatch(typeof(InteractionWorker))]
    [HarmonyPatch("Interacted")]
    public static class VREArchon_InteractionWorker_Interacted_Patch
    {
        [HarmonyPostfix]
        static void GoNuts(Pawn initiator, Pawn recipient, InteractionWorker __instance)
        {


            if (recipient.HasActiveGene(VREA_DefOf.VRE_Aggression_ExtremelyAggressive))
            {
                if(__instance.interaction == InteractionDefOf.Insult || __instance.interaction == VREA_DefOf.Slight) {

                    recipient.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.SocialFighting,null,false,false,false,initiator);

                }
            }

        }
    }








}
