using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace VREArchon
{
    public class VREArchonMod : Mod
    {
        public static VREArchonSettings settings;
        public VREArchonMod(ModContentPack pack) : base(pack)
        {
            settings = GetSettings<VREArchonSettings>();
            new Harmony("VREArchonMod").PatchAll();
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            settings.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return Content.Name;
        }
    }

    public class VREArchonSettings : ModSettings
    {
        public static float archonRaidSpawnChanceInPsychicStorm = 0.5f;
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref archonRaidSpawnChanceInPsychicStorm, "archonRaidSpawnChanceInPsychicStorm", 0.5f);
        }

        private Vector2 scrollPos;
        private float scrollHeight = 99999999;

        public void DoSettingsWindowContents(Rect inRect)
        {
            var viewRect = new Rect(inRect.x, inRect.y, inRect.width - 16, scrollHeight);
            scrollHeight = 0;
            Widgets.BeginScrollView(inRect, ref scrollPos, viewRect);
            var ls = new Listing_Standard();
            ls.Begin(viewRect);
            var initY = ls.curY;
            archonRaidSpawnChanceInPsychicStorm = ls.SliderLabeled("Archon raid incident spawn chance during psychic storm: " + archonRaidSpawnChanceInPsychicStorm.ToStringPercent(), archonRaidSpawnChanceInPsychicStorm, 0, 1);
            ls.End();
            Widgets.EndScrollView();
            scrollHeight = ls.curY - initY;
        }
    }
}
