using RimWorld;
using Verse;

namespace VREArchon
{
    public class IncidentWorker_ArchonRaid : IncidentWorker_RaidEnemy
    {
        public override bool FactionCanBeGroupSource(Faction f, Map map, bool desperate = false)
        {
            return f?.def == VREA_DefOf.VRE_Archons;
        }
        public override bool TryResolveRaidFaction(IncidentParms parms)
        {
            parms.faction = Find.FactionManager.FirstFactionOfDef(VREA_DefOf.VRE_Archons);
            return true;
        }

        public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
        {
            parms.raidStrategy = VREA_DefOf.VREA_ArchonRaidStrategy;
        }

        public override void ResolveRaidArriveMode(IncidentParms parms)
        {
            parms.raidArrivalMode = VREA_DefOf.VREA_ArchonRaidArrival;
        }

        protected virtual string GetLetterLabel(Pawn anyPawn, IncidentParms parms)
        {
            return def.letterLabel;
        }

        protected virtual string GetLetterText(Pawn anyPawn, IncidentParms parms)
        {
            return def.letterText;
        }

        public override bool CanFireNowSub(IncidentParms parms)
        {
            if (parms.target is Map map)
            {
                if (map.weatherManager.CurWeatherPerceived != VREA_DefOf.VREA_PsychicStorm)
                {
                    return false;
                }
            }
            return base.CanFireNowSub(parms);
        }
    }
}
