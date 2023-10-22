using RimWorld;
using Verse;
using VFECore.Abilities;

namespace VREArchon
{
    public class PsychicThrow : AbilityProjectile
    {
        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            if (hitThing is Pawn pawn)
            {
                var hediff = pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_PsychicallyWarped);
                if (hediff != null)
                {
                    GenExplosion.DoExplosion(pawn.Position, pawn.Map, 3.9f, DamageDefOf.Bomb, this, 22);
                    FleckMaker.Static(pawn.Position, pawn.Map, FleckDefOf.PsycastAreaEffect, 3.9f);
                    pawn.health.RemoveHediff(hediff);
                }
            }
            base.Impact(hitThing, blockedByShield);
        }
    }
}
