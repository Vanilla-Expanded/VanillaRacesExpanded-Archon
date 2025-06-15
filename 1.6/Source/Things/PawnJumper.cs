
using System;
using System.Collections.Generic;

using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;


namespace VREArchon
{
    public class PawnJumper : PawnFlyer
    {

        public override void RespawnPawn()
        {
            ExtraLandingEffects();
            base.RespawnPawn();
           
        }

        public void ExtraLandingEffects()
        {

          

            foreach (IntVec3 tile in GenRadial.RadialCellsAround(DestinationPos.ToIntVec3(), 1.9f, useCenter: true))
            {
            
                List<Thing> listOfThings = tile.GetThingList(this.Map);
                List<Thing> tmpList = new List<Thing>();
                foreach (Thing thing in listOfThings)
                {
                    tmpList.Add(thing);
                }


                foreach (Thing thing in tmpList)
                {
                    Pawn pawn = thing as Pawn;
                    if (pawn != null)
                    {
                        IntRange damageRange = new IntRange(4, 25);
                        pawn.TakeDamage(new DamageInfo(DamageDefOf.Blunt, damageRange.RandomInRange, float.MaxValue, instigator: this));

                    }
                }

            }

        }


    }
}










