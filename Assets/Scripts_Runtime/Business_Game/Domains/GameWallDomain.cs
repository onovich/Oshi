using UnityEngine;

namespace Oshi {

    public static class GameWallDomain {

        public static WallEntity Spawn(GameBusinessContext ctx, int typeID, int index, Vector2Int pos) {
            var wall = GameFactory.Wall_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              typeID,
                                              index,
                                              pos);

            var has = ctx.templateInfraContext.Wall_TryGet(typeID, out var wallTM);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }

            var shape = wall.shapeComponent.Get(wall.shapeIndex);
            int cellIndex = 0;
            shape.ForEachCell((localPos) => {
                cellIndex++;
                var cellPos = pos + localPos;
                var cell = GameCellDomain.Spawn(ctx, cellPos);
                if (wallTM.mesh != null) {
                    cell.SetSpr(wallTM.mesh);
                    cell.SetSortingLayer(SortingLayerConst.Wall);
                    cell.SetSprColor(wallTM.meshColor);
                    cell.SetSprMaterial(wallTM.meshMaterial);
                }
                cell.index = cellIndex;
                wall.cellSlotComponent.Add(cell);
                cell.SetParent(wall.transform);
            });

            ctx.wallRepo.Add(wall);
            return wall;
        }

        public static void UnSpawn(GameBusinessContext ctx, WallEntity wall) {
            ctx.wallRepo.Remove(wall);
            wall.TearDown();
        }

    }

}