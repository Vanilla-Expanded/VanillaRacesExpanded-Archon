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
            GameComponent_TranscendentPawns.Instance.listOfPawnsThatDied.Add(this.pawn);

            
            


            base.Notify_PawnDied();
        }
    }
}
