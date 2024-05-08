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

        public static int GetCoveredStraightGrid(Vector2Int start, Vector2Int end, Vector2Int[] result) {
            var dir = end - start;
            for (int i = 0; i < dir.x; i++) {
                result[i] = new Vector2Int(start.x + i, start.y);
            }
            for (int i = 0; i < dir.y; i++) {
                result[i] = new Vector2Int(start.x, start.y + i);
            }
            return dir.x + dir.y;
        }

        public static bool TryGetNeighbourWalkableGrid(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out Vector2Int grid) {
            grid = pos + axis;
            // Constraint
            var allow = GridUtils_Constraint.CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, pos, axis)
            &&
            // No Hard Prop
            (!GridUtils_Has.HasWall(ctx, grid)
            // Has Soft Prop OR Empty
            && GridUtils_Has.HasSoftProp(ctx, grid, axis)
            // Has Pushable Prop
            || GridUtils_Has.HasPushableProp(ctx, grid, axis)
            // Empty
            || GridUtils_Has.HasNoProp(ctx, grid));
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
                allow &=
                // No Hard Prop
                (!GridUtils_Has.HasWall(ctx, grid)
                // Has Soft Prop OR Empty
                && GridUtils_Has.HasSoftProp(ctx, grid, axis)
                // Has Pushable Prop
                || GridUtils_Has.HasPushableProp(ctx, grid, axis)
                // Empty
                || GridUtils_Has.HasNoProp(ctx, grid));
                if (!allow) {
                    grid -= axis;
                    return true;
                }
            }
        }

        public static bool TryGetNeighbourGate(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out GateEntity gate) {
            var has = ctx.gateRepo.TryGetGateByPos(pos + axis, out gate);
            return has;
        }

        public static bool TryGetNeighbourGoal(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out GoalEntity goal) {
            var has = ctx.goalRepo.TryGetGoalByPos(pos + axis, out goal);
            return has;
        }

        public static bool TryGetNeighbourBlock(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out BlockEntity block) {
            var has = ctx.blockRepo.TryGetBlockByPos(pos + axis, out block);
            return has;
        }

    }

}