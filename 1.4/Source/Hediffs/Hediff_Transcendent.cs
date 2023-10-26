using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;
using System.Collections.Generic;


namespace VREArchon
{
    public class Hediff_Transcendent : HediffWithComps
    {
        public override void Notify_PawnDied()
        {
            ResurrectionUtility.Resurrect(this.pawn);
            foreach (IntVec3 tile in this.pawn.CellsAdjacent8WayAndInside())
            {
                List<Thing> listOfThings = tile.GetThingList(this.pawn.Map);
                foreach(Thing thing in listOfThings)
                {
                    if(thing.def == VREA_DefOf.VREA_MeleeWeapon_ArchobladeBladelink)
                    {
                        thing.Destroy();
                    }
                }

            }
            GameComponent_TranscendentPawns.Instance.TryAcceptThing(this.pawn);
            


            base.Notify_PawnDied();
        }
    }
}
