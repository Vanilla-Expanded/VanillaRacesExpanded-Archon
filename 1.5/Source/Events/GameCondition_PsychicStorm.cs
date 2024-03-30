using RimWorld;
using Verse;

namespace VREArchon
{
    public class GameCondition_PsychicStorm : GameCondition
    {
        public override WeatherDef ForcedWeather()
        {
            return def.weatherDef;
        }

        public override void End()
        {
            base.End();
            foreach (var map in AffectedMaps)
            {
                map.weatherDecider.StartNextWeather();
            }
        }
    }
}
