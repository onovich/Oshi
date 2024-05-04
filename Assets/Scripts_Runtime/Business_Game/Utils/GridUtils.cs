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

        public static bool TryGetNeighbourWalkableGrid(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out Vector2 grid) {
            grid = pos + axis;
            // Constraint
            var allow = CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, pos, axis);
            // Wall
            allow &= ctx.wallRepo.Has(pos + axis) == false;
            // Terrain Wall
            allow &= ctx.currentMapEntity.Terrain_HasWall(pos + axis) == false;
            // Pushable Block
            if (allow) {
                var has = ctx.blockRepo.TryGetBlockByPos(pos + axis, out var block);
                if (has) {
                    allow &= CheckPushable(ctx, pos, axis, out var _);
                }
            }
            return allow;
        }

        public static bool TryGetFirstWalkableGrid(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out Vector2 grid) {
            var map = ctx.currentMapEntity;
            var size = map.mapSize;
            grid = pos;
            while (true) {
                grid += axis;
                if (grid.x < 0 || grid.x >= size.x) {
                    return false;
                }
                // Constraint
                var allow = CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, pos, axis);
                // Wall
                allow &= ctx.wallRepo.Has(pos + axis) == false;
                // Terrain Wall
                allow &= ctx.currentMapEntity.Terrain_HasWall(pos + axis) == false;
                if (allow) {
                    return true;
                }
            }
        }

        public static bool CheckMovable(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis) {
            // Constraint
            var allow = CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, pos, axis);
            // Wall
            allow &= ctx.wallRepo.Has(pos + axis) == false;
            // Terrain Wall
            allow &= ctx.currentMapEntity.Terrain_HasWall(pos + axis) == false;
            return allow;
        }

        public static bool CheckConstraint(Vector2 constraintSize, Vector2 constraintCenter, Vector2 pos, Vector2 axis) {
            var offset = Vector2.zero;
            offset.x = 1 - constraintSize.x % 2;
            offset.y = 1 - constraintSize.y % 2;
            var min = constraintCenter - constraintSize / 2 + constraintCenter - offset;
            var max = constraintCenter + constraintSize / 2 + constraintCenter;
            if (pos.x + axis.x >= max.x || pos.x + axis.x <= min.x) {
                return false;
            }
            if (pos.y + axis.y >= max.y || pos.y + axis.y <= min.y) {
                return false;
            }
            return true;
        }

        public static bool CheckPushable(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out BlockEntity block) {
            var has = ctx.blockRepo.TryGetBlockByPos(pos + axis, out block);
            if (has == false) {
                return false;
            }

            var allow = true;
            var _block = block;
            block.cellSlotComponent.ForEach((index, mod) => {
                // Wall
                allow &= ctx.wallRepo.Has(_block.PosInt + mod.LocalPosInt + axis) == false;
                // Block
                allow &= ctx.blockRepo.HasDifferent(_block.PosInt + mod.LocalPosInt + axis, _block.entityIndex) == false;
                // Terrain Wall
                allow &= ctx.currentMapEntity.Terrain_HasWall(_block.PosInt + mod.LocalPosInt + axis) == false;
                // Constraint
                allow &= CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, _block.PosInt + mod.LocalPosInt, axis);
            });
            return allow;
        }

    }

}