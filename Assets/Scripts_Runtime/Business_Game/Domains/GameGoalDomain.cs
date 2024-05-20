using UnityEngine;

namespace Oshi {

    public static class GameGoalDomain {

        public static GoalEntity Spawn(GameBusinessContext ctx, int typeID, int index, Vector2Int pos, int number, bool canPush) {
            var goal = GameFactory.Goal_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              typeID,
                                              index,
                                              pos,
                                              canPush,
                                              number);

            var has = ctx.templateInfraContext.Goal_TryGet(typeID, out var goalTM);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }

            var shape = goal.shapeComponent.Get(goal.shapeIndex);
            int cellIndex = 0;
            shape.ForEachCell((localPos) => {
                cellIndex++;
                var cellPos = pos + localPos;
                var cell = GameCellDomain.Spawn(ctx, number != 0, cellPos, goal.cellRoot);
                cell.SetSpr(canPush ? goalTM.canPushMesh : goalTM.mesh);
                cell.SetSortingLayer(SortingLayerConst.Goal);
                cell.SetSprColor(canPush ? goalTM.canPushColor : goalTM.meshColor);
                cell.SetSprMaterial(canPush ? goalTM.canPushMaterial : goalTM.meshMaterial);
                cell.index = cellIndex;
                goal.cellSlotComponent.Add(cell);
                cell.SetParent(goal.transform);
            });

            ctx.goalRepo.Add(goal);
            SetNumber(ctx, goal);
            return goal;
        }

        public static void UnSpawn(GameBusinessContext ctx, GoalEntity goal) {
            ctx.goalRepo.Remove(goal);
            goal.TearDown();
        }

        static void SetNumber(GameBusinessContext ctx, GoalEntity goal) {
            if (!goal.showNumber) {
                return;
            }
            var number = goal.number;
            var len = goal.cellSlotComponent.TakeAll(out var cells);
            for (int i = 0; i < len; i++) {
                var cell = cells[i];
                cell.SetNumber(number);
                cell.SetNumberMaterial(goal.numberMaterial);
                cell.SetNumberColor(goal.numberColor);
            }
        }

        static bool CheckInSpike(GameBusinessContext ctx, GoalEntity goal) {
            var pos = goal.PosInt;
            var inSpike = false;
            var len = goal.cellSlotComponent.TakeAll(out var cells);
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

        static void ResetGoal(GameBusinessContext ctx, GoalEntity goal) {
            var originalPos = goal.originalPos;
            var oldPos = goal.PosInt;
            goal.Pos_SetPos(originalPos);
            ctx.goalRepo.UpdatePos(oldPos, goal);

            // VFX
            var vfxTable = ctx.templateInfraContext.VFXTable_Get();
            VFXApp.AddVFXToWorld(ctx.vfxContext, vfxTable.goalDeadVFX.name, vfxTable.goalDeadVFXDuration, goal.Pos);

            // Camera
            GameCameraDomain.ShakeOnce(ctx);
        }

        public static void CheckAndResetGoal(GameBusinessContext ctx, GoalEntity goal) {
            var owner = ctx.Role_GetOwner();
            if (owner == null || owner.fsmCom.status != RoleFSMStatus.Idle) {
                return;
            }

            var inSpike = CheckInSpike(ctx, goal);
            if (inSpike) {
                ResetGoal(ctx, goal);
            }
        }

    }

}