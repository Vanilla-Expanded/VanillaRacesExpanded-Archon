using RimWorld;
using Verse;

namespace VREArchon
{
    public class IncidentWorker_PsychicStorm : IncidentWorker
    {
        public override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = (parms.target as Map);
            map.weatherManager.TransitionTo(VREA_DefOf.VREA_PsychicStorm);
            if (Rand.Chance(0.5f))
            {
                IncidentParms raidParms = StorytellerUtility.DefaultParmsNow(VREA_DefOf.VREA_ArchonRaid.category, map);
                Find.Storyteller.incidentQueue.Add(VREA_DefOf.VREA_ArchonRaid, Find.TickManager.TicksGame
                    + (GenDate.TicksPerHour * new IntRange(4, 8).RandomInRange), raidParms);
            }
            return true;
        }
    }
}
