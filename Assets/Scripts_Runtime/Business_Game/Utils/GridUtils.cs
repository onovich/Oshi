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
        // 3. 有物体, 但是可进入(门, 目标点)
        public static bool TryGetNextWalkableGrid(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out Vector2Int grid) {
            grid = pos + axis;
            // Constraint
            var allow = GridUtils_Constraint.CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, pos, axis)
            &&
            // No Prop
            (GridUtils_Has.HasNoProp(ctx, grid)
            // Pushable Prop
            || GridUtils_Has.HasPushableBlock(ctx, grid, axis)
            || GridUtils_Has.HasPushableGoal(ctx, grid, axis)
            || GridUtils_Has.HasPushableGate(ctx, grid, axis)
            // Soft Prop
            || GridUtils_Has.HasUnblockedGate(ctx, grid, axis)
            || GridUtils_Has.HasStaticOrBlockedGoal(ctx, grid, axis));
            return allow;
        }

        public static bool TryGetLastWalkableGrid(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out Vector2Int grid) {
            var map = ctx.currentMapEntity;
            var size = map.mapSize;
            grid = pos;
            while (true) {
                // Constraint
                bool allow = GridUtils_Constraint.CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, pos, axis);
                grid += axis;
                allow
                &=
                // No Prop
                GridUtils_Has.HasNoProp(ctx, grid)
                // Pushable Prop
                || GridUtils_Has.HasPushableBlock(ctx, grid, axis)
                || GridUtils_Has.HasPushableGoal(ctx, grid, axis)
                || GridUtils_Has.HasPushableGate(ctx, grid, axis)
                // Soft Prop
                || GridUtils_Has.HasUnblockedGate(ctx, grid, axis)
                || GridUtils_Has.HasStaticOrBlockedGoal(ctx, grid, axis);
                if (!allow) {
                    grid -= axis;
                    return true;
                }
            }
        }

    }

}