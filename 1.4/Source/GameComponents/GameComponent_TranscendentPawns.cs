using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;


namespace VREArchon
{
    public class GameComponent_TranscendentPawns : GameComponent, IThingHolder
    {

        public static GameComponent_TranscendentPawns Instance;
        public ThingOwner innerContainer = null;
        protected bool contentsKnown;
        public int tickCounter = 0;
        public int tickInterval = 2000;

        public IThingHolder ParentHolder => null;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[] { this });

        }
        public GameComponent_TranscendentPawns()
        {
            this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
            Instance = this;
        }

        public GameComponent_TranscendentPawns(Game game): base()
        {
            this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
            Instance = this;
        }

        public ThingOwner GetDirectlyHeldThings()
        {         
            return this.innerContainer;
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
        }

        public bool Accepts(Thing thing)
        {
            return this.innerContainer.CanAcceptAnyOf(thing, false);
        }

        public bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
        {
            if (!this.Accepts(thing))
            {
                return false;
            }
            bool flag;
            if (thing.holdingOwner != null)
            {
                thing.holdingOwner.Remove(thing);
                this.innerContainer.TryAdd(thing, thing.stackCount, false);
                flag = true;
            }
            else
            {
                flag = this.innerContainer.TryAdd(thing, true);
            }
            if (flag)
            {
                if (thing.Faction != null && thing.Faction.IsPlayer)
                {
                    this.contentsKnown = true;
                }
                return true;
            }
            return false;
        }

        public override void GameComponentTick()
        {
            tickCounter++;
            if ((tickCounter > tickInterval))
            {
                if (innerContainer.Count > 0)
                {
                    Log.Message(innerContainer.ToStringSafeEnumerable());
                }

                tickCounter = 0;
            }

        }










    }


}
