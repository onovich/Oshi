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

        public static bool HasNoPropButGoal(GameBusinessContext ctx, Vector2Int pos) {
            var has = ctx.wallRepo.Has(pos)
            || ctx.blockRepo.Has(pos)
            || ctx.gateRepo.Has(pos)

            || ctx.currentMapEntity.Terrain_HasWall(pos)
            || ctx.currentMapEntity.Terrain_HasSpike(pos);
            return !has;
        }

        public static bool HasDifferentGoal(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, int goalIndex) {
            var has =
            ctx.goalRepo.HasDifferent(pos, goalIndex)

            || ctx.currentMapEntity.Terrain_HasGoal(pos);
            return has;
        }

        public static bool HasDifferentBlock(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, int blockIndex) {
            var has =
            ctx.blockRepo.HasDifferent(pos, blockIndex);
            return has;
        }

        public static bool HasHardProp(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has = ctx.wallRepo.Has(pos)
            || ctx.currentMapEntity.Terrain_HasWall(pos)

            || ctx.blockRepo.TryGetBlockByPos(pos, out var block)
            && GridUtils_Pushable.CheckBlockPushable(ctx, pos, Vector2Int.zero, block);
            return has;
        }

        public static bool HasHardPropWithoutBlock(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has = ctx.wallRepo.Has(pos)
            || ctx.currentMapEntity.Terrain_HasWall(pos);
            return has;
        }

        public static bool HasSoftProp(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has =
            ctx.goalRepo.TryGetGoalByPos(pos, out var goal)
            && (!goal.canPush
            || !GridUtils_Pushable.CheckGoalPushable(ctx, pos, axis, goal))

            || ctx.currentMapEntity.Terrain_HasGoal(pos)

            || (ctx.gateRepo.TryGetGateByPos(pos, out var gate)
            && GridUtils_Movable.CheckNextGateMovable(ctx, gate, axis));
            return has;
        }

        public static bool HasSoftPropWithoutGoal(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has = (ctx.gateRepo.TryGetGateByPos(pos, out var gate)
            && GridUtils_Movable.CheckNextGateMovable(ctx, gate, axis));
            return has;
        }

        public static bool HasPushableProp(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has = (ctx.blockRepo.TryGetBlockByPos(pos, out var block)
            && GridUtils_Pushable.CheckBlockPushable(ctx, pos, axis, block))

            || (ctx.goalRepo.TryGetGoalByPos(pos, out var goal)
            && GridUtils_Pushable.CheckGoalPushable(ctx, pos, axis, goal))

            || (ctx.gateRepo.TryGetGateByPos(pos, out var gate)
            && GridUtils_Pushable.CheckGatePushable(ctx, pos, axis, gate));
            return has;
        }

    }

}