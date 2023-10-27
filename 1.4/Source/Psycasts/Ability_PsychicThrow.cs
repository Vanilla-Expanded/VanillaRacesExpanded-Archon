using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using VanillaPsycastsExpanded.Graphics;
using Verse;
using Verse.Sound;
using VFECore.Abilities;

namespace VREArchon
{
    public class Ability_PsychicThrow : Ability_ShootProjectile
    {
        public int countToFire;
        public int ticksToFire;
        public GlobalTargetInfo mainTarget;
        public List<GlobalTargetInfo> curTargets;
        public bool doNotFireExtraProjectiles;

        protected override Projectile ShootProjectile(GlobalTargetInfo target)
        {
            if (doNotFireExtraProjectiles is false)
            {
                curTargets = new List<GlobalTargetInfo>();
                var extraSensitivity = pawn.GetStatValue(StatDefOf.PsychicSensitivity) - 1f;
                if (extraSensitivity > 0)
                {
                    var amount = (int)(extraSensitivity / 0.5f);
                    if (amount > 0)
                    {
                        countToFire += amount;
                        ticksToFire = Find.TickManager.TicksGame + 8;
                        curTargets.Add(target);
                        mainTarget = target;
                    }
                }
            }
            return base.ShootProjectile(target);
        }

        public override void Tick()
        {
            base.Tick();
            if (countToFire > 0 && Find.TickManager.TicksGame >= ticksToFire) 
            {
                countToFire--;
                ticksToFire = Find.TickManager.TicksGame + 8;
                doNotFireExtraProjectiles = true;
                var nearbyOtherEnemy = pawn.Map.attackTargetsCache.GetPotentialTargetsFor(pawn)
                    .Where(x => curTargets.Any(y => y.Thing == x) is false 
                    && x.Thing.Position.DistanceTo(pawn.Position) <= this.GetRangeForPawn())
                    .OrderBy(x => x.Thing.Position.DistanceTo(pawn.Position)).FirstOrDefault();
                if (nearbyOtherEnemy?.Thing != null)
                {
                    curTargets.Add(nearbyOtherEnemy.Thing);
                    this.ShootProjectile(nearbyOtherEnemy.Thing);
                }
                else
                {
                    this.ShootProjectile(mainTarget);
                }
                def.castSound.PlayOneShot(new TargetInfo(pawn.Position, pawn.MapHeld));
                doNotFireExtraProjectiles = false;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref countToFire, "countToFire");
            Scribe_Values.Look(ref ticksToFire, "ticksToFire");
            Scribe_TargetInfo.Look(ref mainTarget, "mainTarget");
            Scribe_Collections.Look(ref curTargets, "curTargets", LookMode.GlobalTargetInfo);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                curTargets ??= new List<GlobalTargetInfo>();
            }
        }
    }
}
