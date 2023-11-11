using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public static class GameFactory {

        public static BlockEntity BlockEntity_Spawn(GameBusinessContext ctx, int typeId, Vector2Int pos) {

            var prefab = ctx.assetsCoreContext.Entity_GetBlock();

            var blockEntity = GameObject.Instantiate(prefab).GetComponent<BlockEntity>();
            blockEntity.Ctor();

            blockEntity.typeID = typeId;
            blockEntity.entityID = ++ctx.idrecord_block;
            blockEntity.Pos_SetPos(pos);

            var has = ctx.templateCoreContext.BlockTM_TryGet(typeId, out var blockTM);
            if (!has) {
                AlterLog.LogError($"BlockEntity_Spawn: blockTM not found: {typeId}");
                return null;
            }

            var unitModPrefab = ctx.assetsCoreContext.Entity_GetBlockUnitMod();
            for (int i = 0; i < blockTM.shape.GetLength(1); i++) {
                for (int j = 0; j < blockTM.shape.GetLength(0); j++) {
                    if (!blockTM.shape[j, i]) {
                        continue;
                    }
                    var unitMod = GameObject.Instantiate(unitModPrefab, blockEntity.transform);
                    unitMod.transform.localPosition = new Vector3(j, i, 0);
                }
            }

            return blockEntity;
        }

    }

}