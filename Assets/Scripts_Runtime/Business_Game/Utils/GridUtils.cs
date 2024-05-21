using System;
using UnityEngine;

namespace Oshi {

    public static class GridUtils {

        public static Vector2Int[] temp = new Vector2Int[100];

        public static void ForEachGridBySize(Vector2Int pos, Vector2Int size, Action<Vector2Int> action) {
            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    var grid = new Vector2Int(pos.x + x, pos.y + y);
                    action(grid);
                }
            }
        }

        public static int GetPathCoveredGrid(Vector2Int start, Vector2Int end, Vector2Int[] result) {
            var dir = end - start;
            for (int i = 0; i < dir.x; i++) {
                result[i] = new Vector2Int(start.x + i, start.y);
            }
            for (int i = 0; i < dir.y; i++) {
                result[i] = new Vector2Int(start.x, start.y + i);
            }
            return dir.x + dir.y;
        }

        // 格子允许走的条件:
        // 1. 全空, 或:
        // 2. 有物体, 但是可推(门, 箱子, 目标点), 或:
        // 3. 有物体, 但是可进入(门, 没别的东西的目标点), 或:
        // 4. 有物体, 是刺
        public static bool TryGetNextWalkableGrid(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out Vector2Int grid) {
            grid = pos + axis;
            // Constraint
            var inConstraint = GridUtils_Constraint.CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, pos, axis);
            // No Prop
            var noProp = GridUtils_Has.HasNoProp(ctx, grid);
            // Pushable Prop
            var isPushable = GridUtils_Has.HasPushableBlock(ctx, grid, axis)
            || GridUtils_Has.HasPushableGoal(ctx, grid, axis)
            || GridUtils_Has.HasPushableGate(ctx, grid, axis);
            // Soft Prop
            var isSoft = GridUtils_Has.HasStaticOrBlockedGoal(ctx, grid, axis)
            || GridUtils_Has.HasUnblockedGate(ctx, grid, axis)
            || GridUtils_Has.HasStaticOrBlockedGoal(ctx, grid, axis)
            && !GridUtils_Has.HasBlock(ctx, grid + axis);
            // Spike
            var isSpike = GridUtils_Has.HasSpike(ctx, grid);
            var allow = inConstraint && (noProp || isPushable || isSoft || isSpike);
            return allow;
        }

        // 滑冰终点检测(不考虑邻近格可推的情况):
        // 1. 被可推动的物体 或墙 阻挡, 抵达物体前一格
        // 2. 遇到未阻塞的门, 抵达门所在格
        // 3. 遇到阻塞的门, 抵达门前一格
        // 4. 被边界阻挡, 抵达边界前一格
        public static bool TryGetLastWalkableGrid(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out Vector2Int grid) {
            grid = pos;
            while (true) {
                grid += axis;
                // (Not Next) Grid Is Blocked By Wall / Block / Pushable Goal / Pushable Gate
                var isBlock = GridUtils_Has.HasBlock(ctx, grid);
                var isPushableGoal = GridUtils_Has.HasPushableGoal(ctx, grid, axis);
                var isPushableGate = GridUtils_Has.HasPushableGate(ctx, grid, axis);
                var isWall = GridUtils_Has.HasWall(ctx, grid);
                var blocked = isBlock || isPushableGoal || isPushableGate || isWall;
                if (blocked) {
                    grid -= axis;
                    return true;
                }

                // Next Grid Is In Unblocked Gate
                var inGate = GridUtils_Has.HasUnblockedGate(ctx, grid, axis);
                if (inGate) {
                    return true;
                }

                // Next Grid Is In Blocked Gate
                var inBlockedGate = GridUtils_Has.HasBlockedGate(ctx, grid, axis);
                if (inBlockedGate) {
                    grid -= axis;
                    return true;
                }

                // Constraint
                bool allow = GridUtils_Constraint.CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, grid - axis, axis);
                if (!allow) {
                    grid -= axis;
                    return true;
                }
            }
        }

    }

}