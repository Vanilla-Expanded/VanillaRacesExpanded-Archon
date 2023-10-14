using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Reflection.Emit;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Verse.AI;
using System.Security.Cryptography;

namespace VREArchon
{



    [HarmonyPatch(typeof(MentalStateHandler))]
    [HarmonyPatch("TryStartMentalState")]
    public static class VREArchon_MentalStateHandler_TryStartMentalState_Patch
    {
        [HarmonyPostfix]
        public static void SwapSocialFight(MentalStateDef stateDef, string reason, Pawn ___pawn, bool causedByMood, bool forceWake, bool __result)

        {

            if (__result)
            {
                if (stateDef == MentalStateDefOf.SocialFighting && ___pawn.HasActiveGene(VREA_DefOf.VRE_Aggression_ExtremelyAggressive))
                {
                  
                    ___pawn.mindState.mentalStateHandler.TryStartMentalState(VREA_DefOf.VRE_SocialFighting, reason, forceWake, causedByMood);
                }

            }




        }
    }













}

