using System;
using UnityEngine;

namespace Oshi {

    public static class GridUtils_Movable {

        public static bool CheckNextGateMovable(GameBusinessContext ctx, GateEntity gate, Vector2Int axis) {
            var allow = true;
            var nextIndex = gate.nextGateIndex;
            if (nextIndex == -1) {
                return true;
            }
            var has = ctx.gateRepo.TryGetGate(nextIndex, out var nextGate);
            if (!has) {
                GLog.LogError($"Gate {nextIndex} not found");
                return true;
            }

            var target = nextGate.PosInt + axis;
            var len = nextGate.cellSlotComponent.TakeAll(out var cells);
            for (int i = 0; i < len; i++) {
                var cell = cells[i];
                var cellPos = cell.LocalPosInt + target;
                // Wall
                allow &= ctx.wallRepo.Has(cellPos) == false;
                // Block
                allow &= ctx.blockRepo.Has(cellPos) == false;
                // Goal
                allow &= ctx.goalRepo.Has(cellPos) == false;
                // Gate
                allow &= ctx.gateRepo.HasDifferent(cellPos, nextGate.entityIndex) == false;
                // Terrain Goal
                allow &= ctx.currentMapEntity.Terrain_HasGoal(cellPos) == false;
                // Terrain Wall
                allow &= ctx.currentMapEntity.Terrain_HasWall(cellPos) == false;
                // Constraint
                allow &= GridUtils_Constraint.CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, cellPos - axis, axis);
            };
            return allow;
        }

    }

}