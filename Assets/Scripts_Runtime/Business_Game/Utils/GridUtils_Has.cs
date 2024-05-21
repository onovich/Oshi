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

        public static bool HasNoPropAndDifferentBlock(GameBusinessContext ctx, Vector2Int pos, int blockIndex) {
            var has = ctx.wallRepo.Has(pos)
            || ctx.blockRepo.HasDifferent(pos, blockIndex)
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

        public static bool HasNoPropButGoalAndSelf(GameBusinessContext ctx, Vector2Int pos) {
            var has = ctx.goalRepo.Has(pos)
            || ctx.currentMapEntity.Terrain_HasGoal(pos)

            && !ctx.wallRepo.Has(pos)
            && !ctx.currentMapEntity.Terrain_HasWall(pos)

            && !ctx.blockRepo.Has(pos)
            && !ctx.gateRepo.Has(pos)

            && !ctx.spikeRepo.Has(pos)
            && !ctx.currentMapEntity.Terrain_HasSpike(pos);
            return has;
        }

        public static bool HasWall(GameBusinessContext ctx, Vector2Int pos) {
            var has = ctx.wallRepo.Has(pos)
            || ctx.currentMapEntity.Terrain_HasWall(pos);
            return has;
        }

        public static bool HasBlock(GameBusinessContext ctx, Vector2Int pos) {
            var has = ctx.blockRepo.Has(pos);
            return has;
        }

        public static bool HasDifferentBlock(GameBusinessContext ctx, Vector2Int pos, int blockIndex) {
            var has = ctx.blockRepo.HasDifferent(pos, blockIndex);
            return has;
        }

        public static bool HasPushableBlock(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var has = ctx.blockRepo.TryGetBlockByPos(pos, out var block)
             && GridUtils_Pushable.CheckBlockPushable(ctx, pos, axis, block);
            return has;
        }

        public static bool HasSpike(GameBusinessContext ctx, Vector2Int pos) {
            var has = ctx.currentMapEntity.Terrain_HasSpike(pos)
            || ctx.currentMapEntity.Terrain_HasSpike(pos);
            return has;
        }

        // 静态或被阻塞的目标点
        // 1. 静态目标点, 且没被物体压着
        // 2. 动态目标点, 被阻塞, 且没被物体压着
        // 3. 地形目标点, 且没被物体压着
        public static bool HasStaticOrBlockedGoal(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            var isStaticGoal =
            ctx.goalRepo.TryGetGoalByPos(pos, out var goal)
            && !goal.canPush
            && !HasBlock(ctx, pos);

            var isBlockedPushableGoal = goal != null && goal.canPush
            && !GridUtils_Pushable.CheckGoalPushable(ctx, pos, axis, goal)
            && !HasBlock(ctx, pos);

            var isTerrainGoal = ctx.currentMapEntity.Terrain_HasGoal(pos)
            && !HasBlock(ctx, pos);

            var allow = isStaticGoal || isBlockedPushableGoal || isTerrainGoal;
            return allow;
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

        public static bool HasGate(GameBusinessContext ctx, Vector2Int pos) {
            var has = ctx.gateRepo.Has(pos);
            return has;
        }

    }

}