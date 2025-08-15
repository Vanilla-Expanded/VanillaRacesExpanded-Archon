
using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Collections.Generic;
using VREArchon;


namespace VREArchon
{
    [HarmonyPatch(typeof(EquipmentUtility), "CanEquip", new Type[] { typeof(Thing), typeof(Pawn), typeof(string), typeof(bool) }, new ArgumentType[]
        {0,0,ArgumentType.Out,0})]
    internal class VREArchon_EquipmentUtility_CanEquip_Postfix
    {

        public static HashSet<ThingDef> blockedWeapons = new HashSet<ThingDef>() { VREA_DefOf.VREA_MeleeWeapon_ArchobladeBladelink,VREA_DefOf.VREA_Apparel_Archoplate
    };

        [HarmonyPostfix]
        private static void PostFix(ref bool __result, Thing thing, Pawn pawn, ref string cantReason)
        {
            if (blockedWeapons.Contains(thing.def) && pawn.genes?.HasActiveGene(VREA_DefOf.VRE_Transcendent)!=true)
            {
                __result = false;
                cantReason = "VRE_NeedsTranscendentToWield".Translate();
            }


        }
    }
}