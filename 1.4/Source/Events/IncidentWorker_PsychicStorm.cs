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
            if (Rand.Chance(0.5f) || true)
            {
                IncidentParms raidParms = StorytellerUtility.DefaultParmsNow(VREA_DefOf.VREA_ArchonRaid.category, map);
                Find.Storyteller.incidentQueue.Add(VREA_DefOf.VREA_ArchonRaid, Find.TickManager.TicksGame + 3000, raidParms);
                    //+ (GenDate.TicksPerHour * new IntRange(8, 14).RandomInRange), raidParms);
            }
            return true;
        }
    }
}
