using UnityEngine;

namespace Oshi {

    public static class GameWallDomain {

        public static WallEntity Spawn(GameBusinessContext ctx, int typeID, int index, Vector2Int pos, Vector2Int size) {
            var wall = GameFactory.Wall_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              typeID,
                                              index,
                                              pos,
                                              size);

            ctx.wallRepo.Add(wall);
            return wall;
        }

        public static void UnSpawn(GameBusinessContext ctx, WallEntity wall) {
            ctx.wallRepo.Remove(wall);
            wall.TearDown();
        }

    }

}