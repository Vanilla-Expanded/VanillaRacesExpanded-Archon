using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VREArchon
{
    public class PawnsArrivalModeWorker_ArchonRaidArrival : PawnsArrivalModeWorker
    {
        public override bool CanUseWith(IncidentParms parms)
        {
            if (parms.faction is null || parms.faction.def != VREA_DefOf.VRE_Archons)
            {
                return false;
            }
            return true;
        }

        public override void Arrive(List<Pawn> pawns, IncidentParms parms)
        {
            Map map = (Map)parms.target;
            var pawnsToTeleport = pawns.InRandomOrder().Take(new IntRange(2, 4).RandomInRange).ToList();
            var pawnsToArrive = pawns.Where(x => pawnsToTeleport.Contains(x) is false).ToList();
            for (int i = 0; i < pawnsToTeleport.Count; i++)
            {
                IntVec3 loc = map.AllCells.Where(x => x.Walkable(map) && map.reachability.CanReachColony(x)).RandomElement();
                GenSpawn.Spawn(pawns[i], loc, map, parms.spawnRotation);
                FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(pawns[i], 
                    VREA_DefOf.VREA_PsycastSkipFlashGreen, Vector3.zero);
                dataAttachedOverlay.link.detachAfterTicks = 5;
                map.flecks.CreateFleck(dataAttachedOverlay);
            }

            for (int i = 0; i < pawnsToArrive.Count; i++)
            {
                IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8);
                GenSpawn.Spawn(pawns[i], loc, map, parms.spawnRotation);
            }
        }

        public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
        {
            Map map = (Map)parms.target;
            if (parms.attackTargets != null && parms.attackTargets.Count > 0 && !RCellFinder.TryFindEdgeCellFromThingAvoidingColony(parms.attackTargets[0], map, predicate, out parms.spawnCenter))
            {
                CellFinder.TryFindRandomEdgeCellWith((IntVec3 p) => !map.roofGrid.Roofed(p) && p.Walkable(map), map, CellFinder.EdgeRoadChance_Hostile, out parms.spawnCenter);
            }
            if (!parms.spawnCenter.IsValid && !RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Hostile))
            {
                return false;
            }
            parms.spawnRotation = Rot4.FromAngleFlat((map.Center - parms.spawnCenter).AngleFlat);
            return true;
            bool predicate(IntVec3 from, IntVec3 to)
            {
                if (!map.roofGrid.Roofed(from) && from.Walkable(map))
                {
                    return map.reachability.CanReach(from, to, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Some);
                }
                return false;
            }
        }
    }
}
