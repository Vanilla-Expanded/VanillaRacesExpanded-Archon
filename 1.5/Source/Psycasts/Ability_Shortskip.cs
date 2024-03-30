using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;
using Ability = VFECore.Abilities.Ability;

namespace VREArchon
{
    public class Ability_Shortskip : Ability
    {
        public FleckDef[] EffectSet => new[]
        {
            VREA_DefOf.VREA_PsycastSkipFlashGreen,
            FleckDefOf.PsycastSkipInnerExit,
            FleckDefOf.PsycastSkipOuterRingExit
        };

        public override void WarmupToil(Toil toil)
        {
            base.WarmupToil(toil);
            toil.AddPreTickAction(delegate
            {
                if (this.pawn.jobs.curDriver.ticksLeftThisToil != 5) return;
                FleckDef[] set = this.EffectSet;
                for (int i = 0; i < this.Comp.currentlyCastingTargets.Length; i += 2)
                    if (this.Comp.currentlyCastingTargets[i].Thing is { } t)
                    {
                        if (t is Pawn p)
                        {
                            FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(p, set[0], Vector3.zero);
                            dataAttachedOverlay.link.detachAfterTicks = 5;
                            p.Map.flecks.CreateFleck(dataAttachedOverlay);
                        }
                        else
                            FleckMaker.Static(t.TrueCenter(), t.Map, VREA_DefOf.VREA_PsycastSkipFlashGreen);

                        GlobalTargetInfo dest = this.Comp.currentlyCastingTargets[i + 1];
                        //FleckMaker.Static(dest.Cell, dest.Map, set[1]);
                        FleckMaker.Static(dest.Cell, dest.Map, set[0]);
                        SoundDefOf.Psycast_Skip_Entry.PlayOneShot(t);
                        SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(dest.Cell, dest.Map));
                        this.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(t, t.Map), t.Position, 60);
                        this.AddEffecterToMaintain(EffecterDefOf.Skip_Exit.Spawn(dest.Cell, dest.Map), dest.Cell, 60);
                    }
            });
        }

        public override void Cast(params GlobalTargetInfo[] targets)
        {
            for (int i = 0; i < targets.Length; i += 2)
                if (targets[i].Thing is { } t)
                {
                    t.TryGetComp<CompCanBeDormant>()?.WakeUp();
                    GlobalTargetInfo dest = targets[i + 1];
                    if (t.Map != dest.Map)
                    {
                        if (t is not Pawn p) continue;
                        p.teleporting = true;
                        p.ExitMap(true, Rot4.Invalid);
                        p.teleporting = false;
                        GenSpawn.Spawn(p, dest.Cell, dest.Map);
                    }

                    t.Position = dest.Cell;
                    AbilityUtility.DoClamor(t.Position, 10, this.pawn, ClamorDefOf.Ability);
                    AbilityUtility.DoClamor(dest.Cell, 10, this.pawn, ClamorDefOf.Ability);
                    (t as Pawn)?.Notify_Teleported();
                }

            base.Cast(targets);
        }
    }
}
