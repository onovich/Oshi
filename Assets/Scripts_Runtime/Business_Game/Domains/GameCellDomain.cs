using UnityEngine;

namespace Oshi {

    public static class GameCellDomain {

        public static CellMod Spawn(GameBusinessContext ctx, bool showNumber, Vector2Int pos, Transform root) {
            var cell = GameFactory.Cell_Spawn(showNumber,
                                              ctx.idRecordService,
                                              ctx.assetsInfraContext,
                                              pos,
                                              root);

            return cell;
        }

    }

}