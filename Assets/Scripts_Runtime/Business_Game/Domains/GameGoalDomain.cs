using UnityEngine;

namespace Oshi {

    public static class GameGoalDomain {

        public static GoalEntity Spawn(GameBusinessContext ctx, int typeID, int index, Vector2Int pos) {
            var goal = GameFactory.Goal_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              typeID,
                                              index,
                                              pos);

            var has = ctx.templateInfraContext.Goal_TryGet(typeID, out var goalTM);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }

            var shape = goal.shapeComponent.Get(goal.shapeIndex);
            int cellIndex = 0;
            shape.ForEachCell((localPos) => {
                cellIndex++;
                var cellPos = pos + localPos;
                var cell = GameCellDomain.Spawn(ctx, goalTM.showNumber, cellPos, goal.cellRoot);
                cell.SetSpr(goalTM.mesh);
                cell.SetSortingLayer(SortingLayerConst.Goal);
                cell.SetSprColor(goalTM.meshColor);
                cell.SetSprMaterial(goalTM.meshMaterial);
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
            goal.cellSlotComponent.ForEach((index, mod) => {
                mod.SetNumber(number);
                mod.SetNumberMaterial(goal.numberMaterial);
                mod.SetNumberColor(goal.numberColor);
            });
        }

        static bool CheckInSpike(GameBusinessContext ctx, GoalEntity goal) {
            var pos = goal.PosInt;
            var inSpike = false;
            goal.cellSlotComponent.ForEach((index, mod) => {
                // Spike
                inSpike |= (ctx.spikeRepo.Has(mod.LocalPosInt + pos));
                // Terrain Spike
                inSpike |= (ctx.currentMapEntity.Terrain_HasSpike(mod.LocalPosInt + pos));
            });
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