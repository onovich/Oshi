using UnityEngine;

namespace Oshi {

    public static class GameBlockDomain {

        public static BlockEntity Spawn(GameBusinessContext ctx, int typeID, int index, Vector2Int pos, int number, bool isFake) {
            var block = GameFactory.Block_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              typeID,
                                              index,
                                              pos,
                                              isFake,
                                              number);

            var has = ctx.templateInfraContext.Block_TryGet(typeID, out var blockTM);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }

            var shape = block.shapeComponent.Get(block.shapeIndex);
            int cellIndex = 0;
            shape.ForEachCell((localPos) => {
                cellIndex++;
                var cellPos = pos + localPos;
                var cell = GameCellDomain.Spawn(ctx, number != 0, cellPos, block.cellRoot);
                cell.SetSpr(isFake ? blockTM.fakeMesh : blockTM.mesh);
                cell.SetSortingLayer(SortingLayerConst.Block);
                cell.SetSprColor(isFake ? blockTM.fakeColor : blockTM.meshColor);
                cell.SetSprMaterial(isFake ? blockTM.fakeMaterial : blockTM.meshMaterial_bloom);
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
            var len = block.cellSlotComponent.TakeAll(out var cells);
            for (int i = 0; i < len; i++) {
                var cell = cells[i];
                cell.SetNumber(number);
                cell.SetNumberMaterial(block.numberMaterial);
                cell.SetNumberColor(block.numberColor);
            }
        }

        public static void ApplyBloom(GameBusinessContext ctx, BlockEntity block) {
            block.Render_Bloom((pos) => {
                var allow = false;
                // Goal
                allow |= CheckAllInGoal(ctx, block);
                // Terrain Goal
                allow |= (ctx.currentMapEntity.Terrain_HasGoal(pos));
                // Fake
                allow &= !block.isFake;
                return allow;
            });
        }

        public static bool CheckAllInGoal(GameBusinessContext ctx, BlockEntity block) {
            var pos = block.PosInt;
            var len = block.cellSlotComponent.TakeAll(out var cells);
            for (int i = 0; i < len; i++) {
                var cell = cells[i];
                var allow = true;
                allow &=
                // Goal
                (ctx.goalRepo.TryGetGoalByPos(cell.LocalPosInt + pos, out var goal)
                && goal != null && goal.number == block.number
                // Terrain Goal
                || (ctx.currentMapEntity.Terrain_HasGoal(cell.LocalPosInt + pos)));
                if (!allow) {
                    return false;
                }
            }
            return true;
        }

        static bool CheckInSpike(GameBusinessContext ctx, BlockEntity block) {
            var pos = block.PosInt;
            var inSpike = false;
            var len = block.cellSlotComponent.TakeAll(out var cells);
            for (int i = 0; i < len; i++) {
                var cell = cells[i];
                // Spike
                inSpike |= (ctx.spikeRepo.Has(cell.LocalPosInt + pos));
                // Terrain Spike
                inSpike |= (ctx.currentMapEntity.Terrain_HasSpike(cell.LocalPosInt + pos));
                if (inSpike) {
                    break;
                }
            }
            return inSpike;
        }

        static void ResetBlock(GameBusinessContext ctx, BlockEntity block) {
            var originalPos = block.originalPos;
            var oldPos = block.PosInt;
            block.Pos_SetPos(originalPos);
            ctx.blockRepo.UpdatePos(oldPos, block);

            // VFX
            var vfxTable = ctx.templateInfraContext.VFXTable_Get();
            VFXApp.AddVFXToWorld(ctx.vfxContext, vfxTable.blockDeadVFX.name, vfxTable.blockDeadVFXDuration, block.Pos);

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