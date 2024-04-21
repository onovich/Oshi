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
            return block;
        }

        public static void UnSpawn(GameBusinessContext ctx, BlockEntity block) {
            ctx.blockRepo.Remove(block);
            block.TearDown();
        }

        public static bool CheckInGoal(GameBusinessContext ctx, BlockEntity block) {
            var pos = block.PosInt;
            var size = block.sizeInt;
            var inGoal = true;
            GridUtils.ForEachGridBySize(pos, size, (grid) => {
                inGoal &= (ctx.goalRepo.Has(grid));
            });
            return inGoal;
        }

        static bool CheckInSpike(GameBusinessContext ctx, BlockEntity block) {
            var pos = block.PosInt;
            var size = block.sizeInt;
            var inSpike = false;
            GridUtils.ForEachGridBySize(pos, size, (grid) => {
                inSpike |= (ctx.spikeRepo.Has(grid));
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