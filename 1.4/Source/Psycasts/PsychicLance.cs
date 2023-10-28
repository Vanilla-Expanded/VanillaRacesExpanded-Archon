using RimWorld;
using RimWorld.Planet;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
using VFECore.Abilities;

namespace VREArchon
{
    public class PsychicLance : AbilityProjectile
    {
        protected override void DoImpact(Thing hitThing, Map map)
        {
            BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = new BattleLogEntry_RangedImpact(launcher, hitThing, intendedTarget.Thing, equipmentDef ?? ability.pawn.def, def, targetCoverDef);
            Find.BattleLog.Add(battleLogEntry_RangedImpact);
            ability.TargetEffects(new GlobalTargetInfo(base.Position, map));
            AbilityExtension_Projectile modExtension = ability.def.GetModExtension<AbilityExtension_Projectile>();
            if (modExtension != null && modExtension.soundOnImpact != null)
            {
                modExtension.soundOnImpact.PlayOneShot(new TargetInfo(base.Position, map));
            }

            float powerForPawn = new IntRange(8, 22).RandomInRange;
            if (hitThing != null)
            {
                DamageInfo dinfo = new DamageInfo(def.projectile.damageDef, powerForPawn, 
                    float.MaxValue, ExactRotation.eulerAngles.y, launcher, null, this.def, 
                    DamageInfo.SourceCategory.ThingOrUnknown, intendedTarget.Thing);
                hitThing.TakeDamage(dinfo).AssociateWithLog(battleLogEntry_RangedImpact);
                Pawn pawn = hitThing as Pawn;
                if (pawn != null)
                {
                    ability.ApplyHediffs(new GlobalTargetInfo(pawn));
                    if (pawn.stances != null && pawn.BodySize <= def.projectile.StoppingPower + 0.001f)
                    {
                        pawn.stances.stagger.StaggerFor(95);
                    }
                }

                if (def.projectile.extraDamages != null)
                {
                    foreach (ExtraDamage extraDamage in def.projectile.extraDamages)
                    {
                        if (Rand.Chance(extraDamage.chance))
                        {
                            DamageInfo dinfo2 = new DamageInfo(extraDamage.def, extraDamage.amount, extraDamage.AdjustedArmorPenetration(), ExactRotation.eulerAngles.y, launcher, null, equipmentDef, DamageInfo.SourceCategory.ThingOrUnknown, intendedTarget.Thing);
                            hitThing.TakeDamage(dinfo2).AssociateWithLog(battleLogEntry_RangedImpact);
                        }
                    }
                }
            }
            else
            {
                SoundDefOf.BulletImpact_Ground.PlayOneShot(new TargetInfo(base.Position, map));
                if (base.Position.GetTerrain(map).takeSplashes)
                {
                    FleckMaker.WaterSplash(ExactPosition, map, Mathf.Sqrt(powerForPawn) * 1f, 4f);
                }
                else
                {
                    FleckMaker.Static(ExactPosition, map, FleckDefOf.ShotHit_Dirt);
                }
            }
        }

        private Vector3 LookTowards => new Vector3(destination.x - origin.x, def.Altitude, destination.z - origin.z + ArcHeightFactor * (4f - 8f * base.DistanceCoveredFraction));

        private new float ArcHeightFactor
        {
            get
            {
                float num = def.projectile.arcHeightFactor;
                float num2 = (destination - origin).MagnitudeHorizontalSquared();
                if (num * num > num2 * 0.2f * 0.2f)
                {
                    num = Mathf.Sqrt(num2) * 0.2f;
                }

                return num;
            }
        }

        public override Quaternion ExactRotation => Quaternion.LookRotation(LookTowards);

        public override void Tick()
        {
            if (this.Map != null)
            {
                float num = ArcHeightFactor * GenMath.InverseParabola(DistanceCoveredFraction);
                Vector3 drawPos = DrawPos;
                Vector3 position = drawPos + new Vector3(0f, 0f, 1f) * num;
                if (Rand.Value < 0.09f)
                {
                    var mote = MoteMaker.MakeStaticMote(position, Map, VREA_DefOf.VREA_Warp_Mote, 
                        new FloatRange(0.7f, 0.9f).RandomInRange) as MoteThrown;
                    mote.exactPosition = position + Gen.RandomHorizontalVector(0.3f);
                    mote.SetVelocity(new FloatRange(10, 80).RandomInRange, 0.18f);
                }
            }
            base.Tick();
        }

        protected override void Impact(Thing hitThing, bool blockedByShield = false)
        {
            var map = this.Map;
            FleckMaker.Static(this.Position, map, VREA_DefOf.VREA_PsycastSkipFlashGreen, this.ability.GetRadiusForPawn());
            base.Impact(null, blockedByShield);
            var cells = GenRadial.RadialCellsAround(Position, this.ability.GetRadiusForPawn(), true)
                .Where(x => x.InBounds(map) && x.Walkable(map)).ToList();
            foreach (var pawn in GenRadial.RadialDistinctThingsAround(this.Position, map, this.ability.GetRadiusForPawn(), true).OfType<Pawn>().ToList())
            {
                if (cells.TryRandomElement(out var cell))
                {
                    SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(pawn.Position, map));
                    SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(cell, map));
                    this.ability.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(pawn.Position, map, 0.72f), pawn.Position, 60);
                    this.ability.AddEffecterToMaintain(VREA_DefOf.VREA_PsycastSkipFlashGreen_Effecter.Spawn(cell, map, 0.72f), cell, 60);
                    pawn.DeSpawn();
                    GenPlace.TryPlaceThing(pawn, cell, map, ThingPlaceMode.Near);
                }
                DoImpact(pawn, map);
            }
        }
    }
}
