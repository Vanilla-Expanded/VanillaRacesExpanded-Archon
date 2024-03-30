
using System;
using System.Collections.Generic;

using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;
using static HarmonyLib.Code;
using static UnityEngine.GraphicsBuffer;

namespace VREArchon
{
    public class PawnJumper : PawnFlyer
    {
        private static readonly Func<float, float> FlightSpeed;

        private static readonly Func<float, float> FlightCurveHeight;

        private Material cachedShadowMaterial;

        private Effecter flightEffecter;

        private int positionLastComputedTick = -1;

        private Vector3 groundPos;

        private Vector3 effectivePos;

        private float effectiveHeight;

        [NoTranslate]
        private string shadowTexPath = "Things/Skyfaller/SkyfallerShadowCircle";

        private Material ShadowMaterial
        {
            get
            {
                if (cachedShadowMaterial == null && !shadowTexPath.NullOrEmpty())
                {
                    cachedShadowMaterial = MaterialPool.MatFrom(shadowTexPath, ShaderDatabase.Transparent);
                }
                return cachedShadowMaterial;
            }
        }

        public override Vector3 DrawPos
        {
            get
            {
                RecomputePosition();
                return effectivePos;
            }
        }

        static PawnJumper()
        {
            FlightCurveHeight = GenMath.InverseParabola;
            AnimationCurve animationCurve = new AnimationCurve();
            animationCurve.AddKey(0f, 0f);
            animationCurve.AddKey(0.1f, 0.15f);
            animationCurve.AddKey(1f, 1f);
            FlightSpeed = animationCurve.Evaluate;
        }

       

        private void RecomputePosition()
        {
            if (positionLastComputedTick != ticksFlying)
            {
                positionLastComputedTick = ticksFlying;
                float arg = (float)ticksFlying / (float)ticksFlightTime;
                float num = FlightSpeed(arg);
                effectiveHeight = FlightCurveHeight(num);
                groundPos = Vector3.Lerp(startVec, DestinationPos, num);
                Vector3 vector = new Vector3(0f, 0f, 2f);
                Vector3 vector2 = Altitudes.AltIncVect * effectiveHeight;
                Vector3 vector3 = vector * effectiveHeight;
                effectivePos = groundPos + vector2 + vector3;
            }
        }

        public override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            RecomputePosition();
            DrawShadow(groundPos, effectiveHeight);
            FlyingPawn.DrawAt(effectivePos, flip);
            if (CarriedThing != null && FlyingPawn != null)
            {
                PawnRenderUtility.DrawCarriedThing(FlyingPawn, effectivePos, CarriedThing);
            }
        }

        private void DrawShadow(Vector3 drawLoc, float height)
        {
            Material shadowMaterial = ShadowMaterial;
            if (!(shadowMaterial == null))
            {
                float num = Mathf.Lerp(1f, 0.6f, height);
                Vector3 s = new Vector3(num, 1f, num);
                Matrix4x4 matrix = default(Matrix4x4);
                matrix.SetTRS(drawLoc, Quaternion.identity, s);
                Graphics.DrawMesh(MeshPool.plane10, matrix, shadowMaterial, 0);
            }
        }

        public override void RespawnPawn()
        {
            LandingEffects();
            base.RespawnPawn();
        }

        public override void Tick()
        {
            if (flightEffecter == null && flightEffecterDef != null)
            {
                flightEffecter = flightEffecterDef.Spawn();
                flightEffecter.Trigger(this, TargetInfo.Invalid);
            }
            else
            {
                flightEffecter?.EffectTick(this, TargetInfo.Invalid);
            }
            base.Tick();
        }

        private void LandingEffects()
        {
            if (soundLanding != null)
            {
                soundLanding.PlayOneShot(new TargetInfo(Position, Map));
            }
            
            foreach (IntVec3 tile in GenRadial.RadialCellsAround(Position, 1.9f, useCenter: true)) 
            {
                FleckMaker.ThrowDustPuff(tile.ToVector3(), Map, 3f);
                List<Thing> listOfThings = tile.GetThingList(this.Map);
                List<Thing> tmpList = new List<Thing>();
                foreach(Thing thing in listOfThings)
                {
                    tmpList.Add(thing);
                }


                foreach (Thing thing in tmpList)
                {
                    Pawn pawn = thing as Pawn;
                    if (pawn != null)
                    {
                        IntRange damageRange = new IntRange(4,25);
                        pawn.TakeDamage(new DamageInfo(DamageDefOf.Blunt, damageRange.RandomInRange, float.MaxValue, instigator: this));

                    }
                }

            }
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            flightEffecter?.Cleanup();
            base.Destroy(mode);
        }
    }
}
