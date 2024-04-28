using UnityEngine;

namespace Oshi {

    public static class GameCellDomain {

        public static CellMod Spawn(GameBusinessContext ctx, Vector2Int pos) {
            var cell = GameFactory.Cell_Spawn(ctx.idRecordService,
                                              ctx.assetsInfraContext,
                                              pos);

            return cell;
        }

        public static void SpawnCellArrFromShape(GameBusinessContext ctx, ShapeModel shape, int typeID, Vector2Int pos, CellSlotComponent slot, Sprite mesh, Color color, Material material) {
            var map = ctx.currentMapEntity;
            shape.ForEachCell((localPos) => {
                var cellPos = pos + localPos;
                var cell = Spawn(ctx, cellPos);
                slot.Add(cell);
                cell.SetSpr(mesh);
                cell.SetSprColor(color);
                cell.SetSprMaterial(material);
            });
        }

    }

}