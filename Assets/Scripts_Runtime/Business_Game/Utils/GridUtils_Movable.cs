using System;
using UnityEngine;

namespace Oshi {

    public static class GridUtils_Movable {

        // 门可穿越的条件
        // 1. 下一个门前方没有物体, 但是可以有目标点或刺
        public static bool CheckNextGateMovable(GameBusinessContext ctx, GateEntity gate, Vector2Int axis) {
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
                // Constraint
                var inConstraint = GridUtils_Constraint.CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, cellPos - axis, axis);
                // No Prop But Goal Or Spike
                var noProp = GridUtils_Has.HasNoProp(ctx, cellPos);
                var isGoal = GridUtils_Has.HasGoal(ctx, cellPos);
                var isSpike = GridUtils_Has.HasSpike(ctx, cellPos);
                var allow = inConstraint && (noProp || isGoal || isSpike);
                if (!allow) {
                    return false;
                }
            };
            return true;
        }

    }

}