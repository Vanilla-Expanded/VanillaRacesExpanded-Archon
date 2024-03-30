using RimWorld;
using Verse;

namespace VREArchon
{
    public class StatPart_PsychicStormWeather : StatPart
    {
        public override string ExplanationPart(StatRequest req)
        {
            if (req.Thing is not null && req.Thing.Spawned && req.Thing.Map?.weatherManager?.CurWeatherPerceived == VREA_DefOf.VREA_PsychicStorm)
            {
                return "VREA.PsychicStormWeatherBonusStatExplanation".Translate() + ": x" + 2f.ToStringPercent();
            }
            return null;
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.Thing is not null && req.Thing.Spawned && req.Thing.Map?.weatherManager?.CurWeatherPerceived == VREA_DefOf.VREA_PsychicStorm)
            {
                val *= 2f;
            }
        }
    }
}
