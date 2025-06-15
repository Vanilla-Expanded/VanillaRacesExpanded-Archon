using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace VREArchon
{
    public class LordToil_ArchonRaid : LordToil
    {
        private bool attackDownedIfStarving;

        private bool canPickUpOpportunisticWeapons;

        public override bool ForceHighStoryDanger => true;

        public override bool AllowSatisfyLongNeeds => false;

        public LordToil_ArchonRaid(bool attackDownedIfStarving = false, bool canPickUpOpportunisticWeapons = false)
        {
            this.attackDownedIfStarving = attackDownedIfStarving;
            this.canPickUpOpportunisticWeapons = canPickUpOpportunisticWeapons;
        }

        public override void Init()
        {
            base.Init();
            LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
        }

        public override void UpdateAllDuties()
        {
            for (int i = 0; i < lord.ownedPawns.Count; i++)
            {
                lord.ownedPawns[i].mindState.duty = new PawnDuty(VREA_DefOf.VREA_CaptureDownedVictimAndLeaveMap);
                lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = attackDownedIfStarving;
                lord.ownedPawns[i].mindState.duty.pickupOpportunisticWeapon = canPickUpOpportunisticWeapons;
            }
        }
    }
}
