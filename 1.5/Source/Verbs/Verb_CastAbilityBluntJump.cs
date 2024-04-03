
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;
using static UnityEngine.GraphicsBuffer;

namespace VREArchon
{
    public class Verb_CastAbilityBluntJump : Verb_CastAbility
    {
       
    

        public override bool TryCastShot()
        {
            if (base.TryCastShot())
            {
                return DoJump(CasterPawn, currentTarget, ReloadableCompSource, verbProps);
            }
            return false;
        }

        public static bool DoJump(Pawn pawn, LocalTargetInfo currentTarget, CompApparelReloadable comp, VerbProperties verbProps,Ability triggeringAbility = null, LocalTargetInfo target = default(LocalTargetInfo), ThingDef pawnFlyerOverride = null)
        {
           
            IntVec3 position = pawn.Position;
            IntVec3 cell = currentTarget.Cell;
            Map map = pawn.Map;
            bool flag = Find.Selector.IsSelected(pawn);
            PawnJumper pawnFlyer = (PawnJumper)PawnFlyer.MakeFlyer(VREA_DefOf.VREA_PawnJumper, pawn, cell, verbProps.flightEffecterDef, verbProps.soundLanding, verbProps.flyWithCarriedThing, null, triggeringAbility, target);

            //PawnJumper pawnFlyer = MakeFlyer(VREA_DefOf.VREA_PawnJumper, pawn, cell, verbProps.flightEffecterDef, verbProps.soundLanding, verbProps.flyWithCarriedThing);
            if (pawnFlyer != null)
            {
                FleckMaker.ThrowDustPuff(position.ToVector3Shifted() + Gen.RandomHorizontalVector(0.5f), map, 2f);
                GenSpawn.Spawn(pawnFlyer, cell, map);
                if (flag)
                {
                    Find.Selector.Select(pawn, playSound: false, forceDesignatorDeselect: false);
                }
                return true;
            }
            return false;
        }

        public static PawnJumper MakeFlyer(ThingDef flyingDef, Pawn pawn, IntVec3 destCell, EffecterDef flightEffecterDef, SoundDef landingSound, bool flyWithCarriedThing = false)
        {
            PawnJumper pawnFlyer = (PawnJumper)ThingMaker.MakeThing(flyingDef);
           
            pawnFlyer.startVec = pawn.TrueCenter();
            pawnFlyer.flightDistance = pawn.Position.DistanceTo(destCell);
            pawnFlyer.pawnWasDrafted = pawn.Drafted;
            pawnFlyer.flightEffecterDef = flightEffecterDef;
            pawnFlyer.soundLanding = landingSound;
            if (pawn.drafter != null)
            {
                pawnFlyer.pawnCanFireAtWill = pawn.drafter.FireAtWill;
            }
            if (pawn.CurJob != null)
            {
                if (pawn.CurJob.def == JobDefOf.CastJump)
                {
                    pawn.jobs.EndCurrentJob(JobCondition.Succeeded);
                }
                else
                {
                    pawn.jobs.SuspendCurrentJob(JobCondition.InterruptForced);
                }
            }
            pawnFlyer.jobQueue = pawn.jobs.CaptureAndClearJobQueue();
            if (flyWithCarriedThing && pawn.carryTracker.CarriedThing != null && pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Direct, out pawnFlyer.carriedThing))
            {
                if (pawnFlyer.carriedThing.holdingOwner != null)
                {
                    pawnFlyer.carriedThing.holdingOwner.Remove(pawnFlyer.carriedThing);
                }
                pawnFlyer.carriedThing.DeSpawn();
            }
            pawn.DeSpawn(DestroyMode.WillReplace);
            if (!pawnFlyer.innerContainer.TryAdd(pawn))
            {
                Log.Error("Could not add " + pawn.ToStringSafe() + " to a flyer.");
                pawn.Destroy();
            }
            if (pawnFlyer.carriedThing != null && !pawnFlyer.innerContainer.TryAdd(pawnFlyer.carriedThing))
            {
                Log.Error("Could not add " + pawnFlyer.carriedThing.ToStringSafe() + " to a flyer.");
            }
            return pawnFlyer;
        }






    }
}
