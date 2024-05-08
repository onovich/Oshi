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
            var allow = CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, pos, axis);
            // Wall
            allow &= ctx.wallRepo.Has(pos + axis) == false;
            // Terrain Wall
            allow &= ctx.currentMapEntity.Terrain_HasWall(pos + axis) == false;
            // Pushable Block
            if (allow) {
                var hasBlock = TryGetNeighbourBlock(ctx, pos, axis, out var block);
                if (hasBlock) {
                    allow &= CheckNeighbourBlockPushable(ctx, pos, axis, block);
                }
            }
            return allow;
        }

        public static bool TryGetLastWalkableGrid(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out Vector2Int grid) {
            var map = ctx.currentMapEntity;
            var size = map.mapSize;
            grid = pos;

            if (TryGetNeighbourWalkableGrid(ctx, pos, axis, out var _grid) == false) {
                return false;
            }

            if (TryGetNeighbourBlock(ctx, pos, axis, out var block) &&
               (CheckNeighbourBlockPushable(ctx, pos, axis, block))) {
                grid = pos + axis;
                return true;
            }

            var allow = true;
            while (true) {

                // Constraint
                allow &= CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, grid, axis);
                grid += axis;

                // Wall
                allow &= ctx.wallRepo.Has(grid) == false;
                // Terrain Wall
                allow &= ctx.currentMapEntity.Terrain_HasWall(grid) == false;
                // Block
                allow &= ctx.blockRepo.Has(grid) == false;
                // Goal
                if (allow) {
                    var has = ctx.goalRepo.TryGetGoalByPos(grid, out var goal);
                    if (has) {
                        allow &= !CheckNeighbourGoalPushable(ctx, pos - axis, axis, goal);
                    }
                }

                if (allow == false) {
                    grid -= axis;
                    return true;
                }

            }
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

        public static bool TryGetNeighbourGate(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out GateEntity gate) {
            var has = ctx.gateRepo.TryGetGateByPos(pos + axis, out gate);
            return has;
        }

        public static bool CheckNeighbourGatePushable(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, GateEntity gate) {
            var allow = true;
            allow &= !CheckNextGateMovable(ctx, gate, axis);
            return allow;
        }

        public static bool CheckNextGateMovable(GameBusinessContext ctx, GateEntity gate, Vector2Int axis) {
            var allow = true;
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
                // Wall
                allow &= ctx.wallRepo.Has(cellPos) == false;
                // Block
                allow &= ctx.blockRepo.Has(cellPos) == false;
                // Goal
                allow &= ctx.goalRepo.Has(cellPos) == false;
                // Gate
                allow &= ctx.gateRepo.HasDifferent(cellPos, nextGate.entityIndex) == false;
                // Terrain Goal
                allow &= ctx.currentMapEntity.Terrain_HasGoal(cellPos) == false;
                // Terrain Wall
                allow &= ctx.currentMapEntity.Terrain_HasWall(cellPos) == false;
                // Constraint
                allow &= CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, cellPos - axis, axis);
            };
            return allow;
        }

        public static bool TryGetNeighbourGoal(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out GoalEntity goal) {
            var has = ctx.goalRepo.TryGetGoalByPos(pos + axis, out goal);
            return has;
        }

        public static bool CheckNeighbourGoalPushable(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, GoalEntity goal) {
            var allow = true;
            var target = goal.PosInt + axis;

            if (!goal.canPush) {
                return false;
            }

            var len = goal.cellSlotComponent.TakeAll(out var cells);
            for (int i = 0; i < len; i++) {
                var cell = cells[i];
                var cellPos = cell.LocalPosInt + target;
                // Wall
                allow &= ctx.wallRepo.Has(cellPos) == false;
                // Block
                allow &= ctx.blockRepo.Has(cellPos) == false;
                // Goal
                allow &= ctx.goalRepo.HasDifferent(cellPos, goal.entityIndex) == false;
                // Terrain Goal
                allow &= ctx.currentMapEntity.Terrain_HasGoal(cellPos) == false;
                // Terrain Wall
                allow &= ctx.currentMapEntity.Terrain_HasWall(cellPos) == false;
                // Constraint
                allow &= CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, cellPos - axis, axis);
            };
            return allow;
        }

        public static bool TryGetNeighbourBlock(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, out BlockEntity block) {
            var has = ctx.blockRepo.TryGetBlockByPos(pos + axis, out block);
            return has;
        }

        public static bool CheckNeighbourBlockPushable(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, BlockEntity block) {
            var allow = true;
            var target = block.PosInt + axis;

            var len = block.cellSlotComponent.TakeAll(out var cells);
            for (int i = 0; i < len; i++) {
                var cell = cells[i];
                var cellPos = cell.LocalPosInt + target;
                // Wall
                allow &= ctx.wallRepo.Has(cellPos) == false;
                // Block
                allow &= ctx.blockRepo.HasDifferent(cellPos, block.entityIndex) == false;
                // Terrain Wall
                allow &= ctx.currentMapEntity.Terrain_HasWall(cellPos) == false;
                // Constraint
                allow &= CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, cellPos - axis, axis);
            };
            return allow;
        }

    }

}