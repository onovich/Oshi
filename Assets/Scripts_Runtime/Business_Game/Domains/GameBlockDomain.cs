using UnityEngine;

namespace Oshi {

    public static class GameBlockDomain {

        public static BlockEntity Spawn(GameBusinessContext ctx, int typeID, int index, Vector2Int pos) {
            var block = GameFactory.Block_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              typeID,
                                              index,
                                              pos);

            var has = ctx.templateInfraContext.Block_TryGet(typeID, out var blockTM);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }

            var shape = block.shapeComponent.Get(block.shapeIndex);
            int cellIndex = 0;
            shape.ForEachCell((localPos) => {
                cellIndex++;
                var cellPos = pos + localPos;
                var cell = GameCellDomain.Spawn(ctx, blockTM.showNumber, cellPos, block.cellRoot);
                cell.SetSpr(blockTM.mesh);
                cell.SetSortingLayer(SortingLayerConst.Block);
                cell.SetSprColor(blockTM.meshColor);
                cell.SetSprMaterial(blockTM.meshMaterial_bloom);
                cell.index = cellIndex;
                block.cellSlotComponent.Add(cell);
                cell.SetParent(block.transform);
            });

            ApplyBloom(ctx, block);
            SetNumber(ctx, block);
            ctx.blockRepo.Add(block);
            return block;
        }

        public static void UnSpawn(GameBusinessContext ctx, BlockEntity block) {
            ctx.blockRepo.Remove(block);
            block.TearDown();
        }

        static void SetNumber(GameBusinessContext ctx, BlockEntity block) {
            if (!block.showNumber) {
                return;
            }
            var number = block.number;
            block.cellSlotComponent.ForEach((index, mod) => {
                mod.SetNumber(number);
                mod.SetNumberMaterial(block.numberMaterial);
                mod.SetNumberColor(block.numberColor);
            });
        }

        public static void ApplyBloom(GameBusinessContext ctx, BlockEntity block) {
            block.Render_Bloom((pos) => {
                var allow = false;
                // Goal
                allow |= (ctx.goalRepo.Has(pos));
                // Terrain Goal
                allow |= (ctx.currentMapEntity.Terrain_HasGoal(pos));
                // Fake
                allow &= !block.isFake;
                return allow;
            });
        }

        public static bool CheckAllInGoal(GameBusinessContext ctx, BlockEntity block) {
            var pos = block.PosInt;
            var inGoal = true;
            block.cellSlotComponent.ForEach((index, mod) => {
                var allow = false;
                // Goal
                allow |= (ctx.goalRepo.TryGetGoalByPos(mod.LocalPosInt + pos, out var goal));
                // Terrain Goal
                allow |= (ctx.currentMapEntity.Terrain_HasGoal(mod.LocalPosInt + pos));
                inGoal &= allow;
                if (inGoal) {
                    inGoal &= goal.number == block.number;
                }
            });
            return inGoal;
        }

        static bool CheckInSpike(GameBusinessContext ctx, BlockEntity block) {
            var pos = block.PosInt;
            var inSpike = false;
            block.cellSlotComponent.ForEach((index, mod) => {
                // Spike
                inSpike |= (ctx.spikeRepo.Has(mod.LocalPosInt + pos));
                // Terrain Spike
                inSpike |= (ctx.currentMapEntity.Terrain_HasSpike(mod.LocalPosInt + pos));
            });
            return inSpike;
        }

        static void ResetBlock(GameBusinessContext ctx, BlockEntity block) {
            var originalPos = block.originalPos;
            var oldPos = block.PosInt;
            block.Pos_SetPos(originalPos);
            ctx.blockRepo.UpdatePos(oldPos, block);

            // VFX
            VFXApp.AddVFXToWorld(ctx.vfxContext, block.deadVFXName, block.deadVFXDuration, block.Pos);

            // Camera
            GameCameraDomain.ShakeOnce(ctx);
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