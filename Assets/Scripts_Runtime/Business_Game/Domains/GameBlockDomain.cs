using UnityEngine;

namespace Oshi {

    public static class GameBlockDomain {

        public static BlockEntity Spawn(GameBusinessContext ctx, int typeID, int index, Vector2Int pos, Vector2Int size) {
            var block = GameFactory.Block_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              typeID,
                                              index,
                                              pos,
                                              size);

            ctx.blockRepo.Add(block);

            var has = ctx.templateInfraContext.Block_TryGet(typeID, out var blockTM);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }

            var shape = block.shapeComponent.Get(block.shapeIndex);
            shape.ForEachCell((localPos) => {
                var cellPos = pos + localPos;
                var cell = GameCellDomain.Spawn(ctx, cellPos);
                block.cellSlotComponent.Add(cell);
                cell.SetSpr(blockTM.mesh);
                cell.SetSprColor(blockTM.meshColor);
                cell.SetSprMaterial(blockTM.meshMaterial);
            });

            return block;
        }

        public static void UnSpawn(GameBusinessContext ctx, BlockEntity block) {
            ctx.blockRepo.Remove(block);
            block.TearDown();
        }

        public static bool CheckInGoal(GameBusinessContext ctx, BlockEntity block) {
            var pos = block.PosInt;
            var inGoal = true;
            block.cellSlotComponent.ForEach((index, mod) => {
                inGoal &= (ctx.goalRepo.Has(mod.LocalPosInt + pos));
            });
            return inGoal;
        }

        static bool CheckInSpike(GameBusinessContext ctx, BlockEntity block) {
            var pos = block.PosInt;
            var inSpike = false;
            block.cellSlotComponent.ForEach((index, mod) => {
                inSpike |= (ctx.spikeRepo.Has(mod.LocalPosInt + pos));
            });
            return inSpike;
        }

        static void ResetBlock(GameBusinessContext ctx, BlockEntity block) {
            var originalPos = block.originalPos;
            var oldPos = block.PosInt;
            block.Pos_SetPos(originalPos);
            ctx.blockRepo.UpdatePos(oldPos, block);
        }

        public static void CheckAndResetBlock(GameBusinessContext ctx, BlockEntity block) {
            var owner = ctx.Role_GetOwner();
            if (owner == null || owner.fsmCom.status != RoleFSMStatus.Idle) {
                return;
            }

            var inSpike = CheckInSpike(ctx, block);
            if (inSpike) {
                ResetBlock(ctx, block);
            }

        }

    }

}