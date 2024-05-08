using System;
using UnityEngine;

namespace Oshi {

    public static class GridUtils_Pushable {

        public static bool CheckGatePushable(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, GateEntity gate) {
            var allow = true;
            allow &= !GridUtils_Movable.CheckNextGateMovable(ctx, gate, axis);
            return allow;
        }

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
                // No Wall
                && (GridUtils_Has.HasHardProp(ctx, cellPos, axis) == false)
                // No Soft Goal 
                && GridUtils_Has.HasDifferentGoal(ctx, cellPos, axis, goal.entityIndex) == false
                // No Soft Prop
                && GridUtils_Has.HasSoftPropWithoutGoal(ctx, cellPos, axis) == false
                // No Prop
                || GridUtils_Has.HasNoProp(ctx, cellPos);
                if (!allow) {
                    return false;
                }
            };
            return true;
        }

        public static bool CheckBlockPushable(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, BlockEntity block) {
            var target = block.PosInt + axis;

            var len = block.cellSlotComponent.TakeAll(out var cells);
            for (int i = 0; i < len; i++) {
                var cell = cells[i];
                var cellPos = cell.LocalPosInt + target;
                // No Prop
                var allow = GridUtils_Has.HasNoProp(ctx, cellPos)
                // No Wall
                || (GridUtils_Has.HasHardProp(ctx, cellPos, axis) == false)
                // No Block
                && GridUtils_Has.HasDifferentBlock(ctx, cellPos, axis, block.entityIndex) == false
                // No Soft
                && GridUtils_Has.HasSoftProp(ctx, cellPos, axis) == false
                // Constraint
                && GridUtils_Constraint.CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, cellPos - axis, axis);
                if (!allow) {
                    return false;
                }
            };
            return true;
        }

    }

}