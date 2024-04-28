using UnityEngine;

namespace Oshi {

    public static class GameCellDomain {

        public static CellMod Spawn(GameBusinessContext ctx, Vector2Int pos) {
            var cell = GameFactory.Cell_Spawn(ctx.idRecordService,
                                              ctx.assetsInfraContext,
                                              pos);

            return cell;
        }

    }

}