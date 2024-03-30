using HarmonyLib;
using RimWorld;
using Verse;
namespace VREArchon
{
    [HarmonyPatch(typeof(IncidentWorker_RaidEnemy), nameof(IncidentWorker_RaidEnemy.FactionCanBeGroupSource))]
    public static class IncidentWorker_RaidEnemy_FactionCanBeGroupSource_Patch
    {
        public static void Postfix(IncidentWorker_RaidEnemy __instance, ref bool __result, Faction f, Map map, bool desperate = false)
        {
            if (__result && f.def == VREA_DefOf.VRE_Archons && __instance is not IncidentWorker_ArchonRaid)
            {
                __result = false;
            }
        }
    }
}
