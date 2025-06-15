using HarmonyLib;
using Verse;
namespace VREArchon
{
    [HarmonyPatch(typeof(ArmorUtility), "ApplyArmor")]
    public static class ArmorUtility_ApplyArmor_Patch
    {
        public static void Prefix(ref float armorRating, Pawn pawn)
        {
            if (pawn.health.hediffSet.HasHediff(VREA_DefOf.VREA_PsychicallyWarped))
                armorRating = 0f;
        }
    }
}
