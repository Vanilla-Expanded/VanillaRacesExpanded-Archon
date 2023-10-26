using RimWorld;
using Verse;

namespace VREArchon
{
    public class IncidentWorker_PsychicStorm : IncidentWorker
    {
        public override bool TryExecuteWorker(IncidentParms parms)
        {
            (parms.target as Map).weatherManager.TransitionTo(VREA_DefOf.VREA_PsychicStorm);
            return true;
        }
    }
}
