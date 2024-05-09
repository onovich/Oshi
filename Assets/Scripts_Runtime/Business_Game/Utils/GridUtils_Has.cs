using System;
using UnityEngine;

namespace Oshi {

    public static class GridUtils_Has {

        public static bool HasNoProp(GameBusinessContext ctx, Vector2Int pos) {
            var has = ctx.wallRepo.Has(pos)
            || ctx.blockRepo.Has(pos)
            || ctx.goalRepo.Has(pos)
            || ctx.gateRepo.Has(pos)

            || ctx.currentMapEntity.Terrain_HasWall(pos)
            || ctx.currentMapEntity.Terrain_HasGoal(pos)
            || ctx.currentMapEntity.Terrain_HasSpike(pos);
            return !has;
        }

        public static bool HasPushableGate(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has = ctx.gateRepo.TryGetGateByPos(pos, out var gate)
            && GridUtils_Pushable.CheckGatePushable(ctx, pos, axis, gate);
            return has;
        }

        public static bool HasPushableGoal(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has = ctx.goalRepo.TryGetGoalByPos(pos, out var goal)
            && GridUtils_Pushable.CheckGoalPushable(ctx, pos, axis, goal);
            return has;
        }

        public static bool HasGoal(GameBusinessContext ctx, Vector2Int pos) {
            var has = ctx.goalRepo.Has(pos)
            || ctx.currentMapEntity.Terrain_HasGoal(pos);
            return has;
        }

        public static bool HasPushableBlock(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has = ctx.blockRepo.TryGetBlockByPos(pos, out var block)
             && GridUtils_Pushable.CheckBlockPushable(ctx, pos, axis, block);
            return has;
        }

        public static bool HasStaticOrBlockedGoal(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has =
            ctx.goalRepo.TryGetGoalByPos(pos, out var goal)
            && (!goal.canPush
            || !GridUtils_Pushable.CheckGoalPushable(ctx, pos, axis, goal))

            || ctx.currentMapEntity.Terrain_HasGoal(pos);
            return has;
        }

        public static bool HasUnblockedGate(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has = ctx.gateRepo.TryGetGateByPos(pos, out var gate)
            && GridUtils_Movable.CheckNextGateMovable(ctx, gate, axis);
            return has;
        }

        public static bool HasBlockedGate(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has = ctx.gateRepo.TryGetGateByPos(pos, out var gate)
            && !GridUtils_Movable.CheckNextGateMovable(ctx, gate, axis);
            return has;
        }

    }

}