using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace VREArchon
{
    [StaticConstructorOnStartup]
    public class WeatherEvent_LightningStrike_PsychicStorm : WeatherEvent_LightningFlash
    {
        private static readonly Material LightningMat =
            MaterialPool.MatFrom("Weather/PsychicStormLightningBolt", ShaderDatabase.MoteGlow);

        private Mesh boltMesh;

        private IntVec3 strikeLoc = IntVec3.Invalid;

        public WeatherEvent_LightningStrike_PsychicStorm(Map map) : base(map)
        {
        }

        public WeatherEvent_LightningStrike_PsychicStorm(Map map, IntVec3 forcedStrikeLoc) : base(map)
        {
            strikeLoc = forcedStrikeLoc;
        }

        public override void FireEvent()
        {
            SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(map);
            if (!strikeLoc.IsValid)
            {
                strikeLoc = CellFinderLoose.RandomCellWith((IntVec3 sq) => sq.Standable(map) && !map.roofGrid.Roofed(sq), map);
            }
            boltMesh = LightningBoltMeshPool.RandomBoltMesh;
            if (!strikeLoc.Fogged(map))
            {
                GenExplosion.DoExplosion(strikeLoc, map, 1.9f, DamageDefOf.Bomb, null, chanceToStartFire: 0);
                Vector3 loc = strikeLoc.ToVector3Shifted();
                for (int i = 0; i < 4; i++)
                {
                    FleckMaker.ThrowSmoke(loc, map, 1.5f);
                    FleckMaker.ThrowMicroSparks(loc, map);
                    FleckMaker.ThrowLightningGlow(loc, map, 1.5f);
                }
            }
            SoundInfo info = SoundInfo.InMap(new TargetInfo(strikeLoc, map));
            SoundDefOf.Thunder_OnMap.PlayOneShot(info);
        }

        public override void WeatherEventDraw()
        {
            Graphics.DrawMesh(boltMesh, strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, 
                FadedMaterialPool.FadedVersionOf(LightningMat, LightningBrightness), 0, null);
        }
    }
}
