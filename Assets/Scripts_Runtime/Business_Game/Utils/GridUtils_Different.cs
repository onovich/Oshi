using System;
using UnityEngine;

namespace Oshi {

    public static class GridUtils_Different {

        public static bool Goal(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, int goalIndex) {
            var has =
            ctx.goalRepo.HasDifferent(pos, goalIndex)

            || ctx.currentMapEntity.Terrain_HasGoal(pos);
            return has;
        }

        public static bool Block(GameBusinessContext ctx, Vector2Int pos, Vector2Int axis, int blockIndex) {
            var has =
            ctx.blockRepo.HasDifferent(pos, blockIndex);
            return has;
        }

    }

}