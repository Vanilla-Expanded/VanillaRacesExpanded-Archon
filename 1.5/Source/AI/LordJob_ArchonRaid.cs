using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace VREArchon
{
    public class LordJob_ArchonRaid : LordJob
    {
        private Faction assaulterFaction;

        private static readonly IntRange AssaultTimeBeforeGiveUp = new IntRange(26000, 38000);

        public override bool GuiltyOnDowned => true;

        public LordJob_ArchonRaid()
        {
        }

        public LordJob_ArchonRaid(SpawnedPawnParams parms)
        {
            assaulterFaction = parms.spawnerThing.Faction;
        }

        public LordJob_ArchonRaid(Faction assaulterFaction, bool canKidnap = true, bool canTimeoutOrFlee = true, bool sappers = false, bool useAvoidGridSmart = false, bool canSteal = true, bool breachers = false, bool canPickUpOpportunisticWeapons = false)
        {
            this.assaulterFaction = assaulterFaction;
        }

        public override StateGraph CreateGraph()
        {
            StateGraph stateGraph = new StateGraph();
            List<LordToil> list = new List<LordToil>();
            LordToil lordToil3 = new LordToil_ArchonRaid(attackDownedIfStarving: false, true);
            lordToil3.useAvoidGrid = true;
            stateGraph.AddToil(lordToil3);
            LordToil_ExitMap lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, canDig: false, interruptCurrentJob: true);
            lordToil_ExitMap.useAvoidGrid = true;
            stateGraph.AddToil(lordToil_ExitMap);
            if (assaulterFaction != null && assaulterFaction.def.humanlikeFaction)
            {
                Transition transition3 = new Transition(lordToil3, lordToil_ExitMap);
                transition3.AddSources(list);
                transition3.AddTrigger(new Trigger_TicksPassed(AssaultTimeBeforeGiveUp.RandomInRange));
                transition3.AddPreAction(new TransitionAction_Message("MessageRaidersGivenUpLeaving".Translate(assaulterFaction.def.pawnsPlural.CapitalizeFirst(), assaulterFaction.Name)));
                stateGraph.AddTransition(transition3);
                Transition transition4 = new Transition(lordToil3, lordToil_ExitMap);
                transition4.AddSources(list);
                float randomInRange = new FloatRange(0.25f, 0.35f).RandomInRange;
                transition4.AddTrigger(new Trigger_FractionColonyDamageTaken(randomInRange, 900f));
                transition4.AddPreAction(new TransitionAction_Message("MessageRaidersSatisfiedLeaving".Translate(assaulterFaction.def.pawnsPlural.CapitalizeFirst(), assaulterFaction.Name)));
                stateGraph.AddTransition(transition4);

                LordToil startingToil = stateGraph.AttachSubgraph(new LordJob_Kidnap().CreateGraph()).StartingToil;
                Transition transition5 = new Transition(lordToil3, startingToil);
                transition5.AddSources(list);
                transition5.AddPreAction(new TransitionAction_Message("MessageRaidersKidnapping".Translate(assaulterFaction.def.pawnsPlural.CapitalizeFirst(), assaulterFaction.Name)));
                transition5.AddTrigger(new Trigger_KidnapVictimPresent());
                stateGraph.AddTransition(transition5);
            }
            if (assaulterFaction != null)
            {
                Transition transition7 = new Transition(lordToil3, lordToil_ExitMap);
                transition7.AddSources(list);
                transition7.AddTrigger(new Trigger_BecameNonHostileToPlayer());
                transition7.AddPreAction(new TransitionAction_Message("MessageRaidersLeaving".Translate(assaulterFaction.def.pawnsPlural.CapitalizeFirst(), assaulterFaction.Name)));
                stateGraph.AddTransition(transition7);
            }
            return stateGraph;
        }

        public override void ExposeData()
        {
            Scribe_References.Look(ref assaulterFaction, "assaulterFaction");
        }
    }
}
