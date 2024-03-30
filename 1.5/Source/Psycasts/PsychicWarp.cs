using RimWorld;
using Verse;
using VFECore.Abilities;

namespace VREArchon
{
    public class PsychicWarp : AbilityProjectile
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            if (hitThing is Pawn pawn)
            {
                pawn.health.AddHediff(VREA_DefOf.VREA_PsychicallyWarped);
                FleckMaker.Static(pawn.Position, pawn.Map, VREA_DefOf.VREA_PsycastAreaEffectFast, 1.9f);
            }
            base.Impact(hitThing, blockedByShield);
        }
    }
}
