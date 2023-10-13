using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;


namespace VREArchon
{
    public class Hediff_Transcendent : HediffWithComps
    {
        public override void Notify_PawnDied()
        {
            ResurrectionUtility.Resurrect(this.pawn);
            GameComponent_TranscendentPawns.Instance.TryAcceptThing(this.pawn);



            base.Notify_PawnDied();
        }
    }
}
