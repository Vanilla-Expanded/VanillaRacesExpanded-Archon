using RimWorld;
using UnityEngine;
using Verse;

namespace VREArchon
{
    public class IncidentWorker_PsychicStorm : IncidentWorker_MakeGameCondition
    {
        public override bool TryExecuteWorker(IncidentParms parms)
        {
            var archonFaction = Find.FactionManager.FirstFactionOfDef(VREA_DefOf.VRE_Archons);
            if (archonFaction is null)
            {
                return false;
            }
            var map = (parms.target as Map);
            map.weatherManager.TransitionTo(VREA_DefOf.VREA_PsychicStorm);
            GameConditionManager gameConditionManager = parms.target.GameConditionManager;
            GameCondition gameCondition = GameConditionMaker.MakeCondition(duration: Mathf.RoundToInt(def.durationDays.RandomInRange * 60000f), def: def.gameCondition);
            gameConditionManager.RegisterCondition(gameCondition);
            if (!gameCondition.HiddenByOtherCondition(map) && !def.letterLabel.NullOrEmpty() && !gameCondition.def.letterText.NullOrEmpty())
            {
                parms.letterHyperlinkThingDefs = gameCondition.def.letterHyperlinks;
                SendStandardLetter(def.letterLabel, gameCondition.LetterText, def.letterDef, parms, LookTargets.Invalid);
            }

            if (Rand.Chance(VREArchonSettings.archonRaidSpawnChanceInPsychicStorm))
            {
                IncidentParms raidParms = StorytellerUtility.DefaultParmsNow(VREA_DefOf.VREA_ArchonRaid.category, parms.target);
                raidParms.faction = archonFaction;
                var ticksToFire = Find.TickManager.TicksGame + Mathf.Min(gameCondition.duration / 2, (GenDate.TicksPerHour * new IntRange(4, 8).RandomInRange));
                Find.Storyteller.incidentQueue.Add(VREA_DefOf.VREA_ArchonRaid, ticksToFire, raidParms);
            }
            return true;
        }
    }
}
