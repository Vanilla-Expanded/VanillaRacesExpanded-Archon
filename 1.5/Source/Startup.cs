using RimWorld;
using System.Collections.Generic;
using Verse;
using static RimWorld.FleshTypeDef;

namespace VREArchon
{
    [StaticConstructorOnStartup]
    public static class Startup
    {
        static Startup()
        {
            foreach (var def in DefDatabase<FleshTypeDef>.AllDefs)
            {
                def.hediffWounds ??= new List<HediffWound>();
                def.hediffWounds.Add(new HediffWound
                {
                    hediff = VREA_DefOf.VREA_Sear,
                    onlyHumanlikes = false,
                    wounds = new List<Wound>
                    {
                        new Wound
                        {
                            texture = "Things/Pawn/Overlays/WoundSearA",
                        },
                        new Wound
                        {
                            texture = "Things/Pawn/Overlays/WoundSearB"
                        },
                        new Wound
                        {
                            texture = "Things/Pawn/Overlays/WoundSearC"
                        },
                    }
                });
            }
        }
    }
}
