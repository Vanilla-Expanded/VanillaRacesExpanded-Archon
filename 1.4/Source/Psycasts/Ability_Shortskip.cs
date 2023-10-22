using RimWorld;
using System.Collections.Generic;
using VanillaPsycastsExpanded;
using VanillaPsycastsExpanded.Skipmaster;
using Verse;

namespace VREArchon
{
    public class Ability_Shortskip : Ability_Teleport
    {
        public override FleckDef[] EffectSet => new[]
        {
            VREA_DefOf.VREA_PsycastSkipFlashGreen,
            FleckDefOf.PsycastSkipInnerExit,
            FleckDefOf.PsycastSkipOuterRingExit
        };
    }
}
