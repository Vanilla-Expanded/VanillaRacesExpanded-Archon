using RimWorld;
using UnityEngine;
using Verse;

namespace VREArchon
{
    public class IncidentWorker_PsychicStorm : IncidentWorker_MakeGameCondition
    {
        public override bool TryExecuteWorker(IncidentParms parms)
        {
            var map = (parms.target as Map);
            map.weatherManager.TransitionTo(VREA_DefOf.VREA_PsychicStorm);
            GameConditionManager gameConditionManager = parms.target.GameConditionManager;
            GameCondition gameCondition = GameConditionMaker.MakeCondition(duration: Mathf.RoundToInt(def.durationDays.RandomInRange * 60000f), def: def.gameCondition);
            gameConditionManager.RegisterCondition(gameCondition);
            if (!gameCondition.HiddenByOtherCondition && !def.letterLabel.NullOrEmpty() && !gameCondition.def.letterText.NullOrEmpty())
            {
                parms.letterHyperlinkThingDefs = gameCondition.def.letterHyperlinks;
                SendStandardLetter(def.letterLabel, gameCondition.LetterText, def.letterDef, parms, LookTargets.Invalid);
            }

            if (Rand.Chance(0.5f))
            {
                IncidentParms raidParms = StorytellerUtility.DefaultParmsNow(VREA_DefOf.VREA_ArchonRaid.category, parms.target);
                Find.Storyteller.incidentQueue.Add(VREA_DefOf.VREA_ArchonRaid, Find.TickManager.TicksGame
                    + Mathf.Min(gameCondition.duration / 2, (GenDate.TicksPerHour * new IntRange(4, 8).RandomInRange)), raidParms);
            }
            return true;
        }
    }
}
