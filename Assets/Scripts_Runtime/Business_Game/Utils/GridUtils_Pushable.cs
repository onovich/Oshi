using System;
using UnityEngine;

namespace Oshi {

    public static class GridUtils_Pushable {

        // 门可推的条件:
        // 1. 下一个门被阻塞
        // 2. 门的下一个位置没有物体, 但是可以有目标点或刺
        public static bool CheckGatePushable(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, GateEntity gate) {
            var target = pos + axis;
            // Next Gate Not Movable
            var nextBlocked = !GridUtils_Movable.CheckNextGateMovable(ctx, gate, axis);
            if (!nextBlocked) {
                return false;
            }
            var len = gate.cellSlotComponent.TakeAll(out var cells);
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

        // 目标点可推的条件:
        // 1. 目标点是可推类型
        // 2. 目标点的下一个位置为空或刺
        public static bool CheckGoalPushable(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, GoalEntity goal) {
            var target = goal.PosInt + axis;

            if (!goal.canPush) {
                return false;
            }

            var len = goal.cellSlotComponent.TakeAll(out var cells);
            for (int i = 0; i < len; i++) {
                var cell = cells[i];
                var cellPos = cell.LocalPosInt + target;
                // Constraint
                var allow = GridUtils_Constraint.CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, cellPos - axis, axis)
                // No Prop But Spike
                && GridUtils_Has.HasNoProp(ctx, cellPos)
                || GridUtils_Has.HasSpike(ctx, cellPos);
                if (!allow) {
                    return false;
                }
            };
            return true;
        }

        // 箱子可推的条件:
        // 1. 箱子的下一个位置为空, 但是可以有目标点或刺
        public static bool CheckBlockPushable(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, BlockEntity block) {
            var target = block.PosInt + axis;

            var len = block.cellSlotComponent.TakeAll(out var cells);
            for (int i = 0; i < len; i++) {
                var cell = cells[i];
                var cellPos = cell.LocalPosInt + target;
                // Constraint
                var inConstraint = GridUtils_Constraint.CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, cellPos - axis, axis);

                // 如果 Block 的下一格还是自己，则跳过检测
                if (GridUtils_Has.HasBlock(ctx, cellPos) && !GridUtils_Has.HasDifferentBlock(ctx, cellPos, block.entityIndex)) {
                    continue;
                }

                // No Prop But Goal Or Spike
                var noProp = GridUtils_Has.HasNoPropAndDifferentBlock(ctx, cellPos, block.entityIndex);
                var hasNothingButGoal = GridUtils_Has.HasNoPropButGoal(ctx, cellPos);
                var hasSpike = GridUtils_Has.HasSpike(ctx, cellPos);
                var allow = inConstraint && (noProp || hasNothingButGoal || hasSpike);
                if (!allow) {
                    return false;
                }
            };
            return true;
        }

    }

}