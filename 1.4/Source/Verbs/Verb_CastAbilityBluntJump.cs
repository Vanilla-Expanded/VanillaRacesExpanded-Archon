
using RimWorld;
using UnityEngine;
using Verse;
namespace VREArchon
{
    public class Verb_CastAbilityBluntJump : Verb_CastAbilityJump
    {
       
    

        public override bool TryCastShot()
        {
            if (base.TryCastShot())
            {
                return DoJump(CasterPawn, currentTarget, ReloadableCompSource, verbProps);
            }
            return false;
        }

        public static bool DoJump(Pawn pawn, LocalTargetInfo currentTarget, CompReloadable comp, VerbProperties verbProps)
        {
           
            IntVec3 position = pawn.Position;
            IntVec3 cell = currentTarget.Cell;
            Map map = pawn.Map;
            bool flag = Find.Selector.IsSelected(pawn);
            PawnFlyer pawnFlyer = PawnFlyer.MakeFlyer(VREA_DefOf.VREA_PawnJumper, pawn, cell, verbProps.flightEffecterDef, verbProps.soundLanding, verbProps.flyWithCarriedThing);
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






    }
}
