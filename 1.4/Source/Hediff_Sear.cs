using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;
using static RimWorld.FleshTypeDef;

namespace VREArchon
{
    public class Hediff_Sear : Hediff_Injury
    {
        public override void Tended(float quality, float maxQuality, int batchPosition = 0)
        {
            base.Tended(quality, maxQuality, batchPosition);
            var comp = this.TryGetComp<HediffComp_GetsPermanent>();
            if (!comp.IsPermanent)
            {
                comp.IsPermanent = true;
            }
        }
    }
}
