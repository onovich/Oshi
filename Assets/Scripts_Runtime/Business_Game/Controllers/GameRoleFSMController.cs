using System;
using UnityEngine;

namespace Oshi {

    public static class GameRoleFSMController {

        public static void FixedTickFSM(GameBusinessContext ctx, RoleEntity role, float fixdt, Action onEnd) {

            if (role == null) {
                return;
            }

            FixedTickFSM_Any(ctx, role, fixdt);

            RoleFSMStatus status = role.FSM_GetStatus();
            if (status == RoleFSMStatus.Idle) {
                FixedTickFSM_Idle(ctx, role, fixdt);
            } else if (status == RoleFSMStatus.Moving) {
                FixedTickFSM_Moving(ctx, role, fixdt, onEnd);
            } else if (status == RoleFSMStatus.Dead) {
                FixedTickFSM_Dead(ctx, role, fixdt, onEnd);
            } else {
                GLog.LogError($"GameRoleFSMController.FixedTickFSM: unknown status: {status}");
            }

        }

        static void FixedTickFSM_Any(GameBusinessContext ctx, RoleEntity role, float fixdt) {
            role.Pos_RecordLastFramePos();
        }

        static void FixedTickFSM_Idle(GameBusinessContext ctx, RoleEntity role, float fixdt) {
            RoleFSMComponent fsm = role.FSM_GetComponent();
            if (fsm.idle_isEntering) {
                fsm.idle_isEntering = false;
            }

            // Move
            if (role.inputCom.moveAxis.x == 0 && role.inputCom.moveAxis.y == 0) {
                return;
            }

            var map = ctx.currentMapEntity;
            var succ = false;
            var target = Vector2Int.zero;
            if (map.weatherType == WeatherType.Rain) {

                var pushable = GridUtils_Has.HasPushableBlock(ctx, role.PosInt + role.Pos_GetDir(), role.inputCom.moveAxis)
                            || GridUtils_Has.HasPushableGoal(ctx, role.PosInt + role.Pos_GetDir(), role.inputCom.moveAxis)
                            || GridUtils_Has.HasPushableGate(ctx, role.PosInt + role.Pos_GetDir(), role.inputCom.moveAxis);
                // Next Is Pushable
                if (pushable) {
                    succ = GridUtils.TryGetNextWalkableGrid(ctx, role.PosInt, role.Pos_GetDir(), out target);
                    if (!succ) {
                        return;
                    }
                } else {
                    // Next Is Not Pushable
                    succ = GridUtils.TryGetLastWalkableGrid(ctx, role.PosInt, role.Pos_GetDir(), out target);
                    if (!succ) {
                        return;
                    }
                }
            } else {
                succ = GridUtils.TryGetNextWalkableGrid(ctx, role.PosInt, role.Pos_GetDir(), out target);
                if (!succ) {
                    return;
                }
            }

            // Push
            //// - Push Block
            var hasBlock = ctx.blockRepo.TryGetBlockByPos(target, out var block);
            succ = hasBlock && GridUtils_Has.HasPushableBlock(ctx, target, role.Pos_GetDir());
            if (succ) {
                role.FSM_EnterMovingWithPush(target, role.moveDurationSec, EntityType.Block, block.entityIndex, block.PosInt);
                return;
            }
            //// - Push Goal
            var hasGoal = ctx.goalRepo.TryGetGoalByPos(target, out var goal);
            succ = hasGoal && GridUtils_Has.HasPushableGoal(ctx, target, role.Pos_GetDir());
            if (succ) {
                role.FSM_EnterMovingWithPush(target, role.moveDurationSec, EntityType.Goal, goal.entityIndex, goal.PosInt);
                return;
            }
            //// - Push Gate
            var hasGate = ctx.gateRepo.TryGetGateByPos(target, out var gate);
            succ = hasGate && GridUtils_Has.HasPushableGate(ctx, target, role.Pos_GetDir());
            if (succ) {
                role.FSM_EnterMovingWithPush(target, role.moveDurationSec, EntityType.Gate, gate.entityIndex, gate.PosInt);
                return;
            }

            // No Push
            if (!succ) {
                role.FSM_EnterMovingWithOutPush(target, role.moveDurationSec);
            }
        }

        static void FixedTickFSM_Moving(GameBusinessContext ctx, RoleEntity role, float fixdt, Action onEnd) {
            RoleFSMComponent fsm = role.FSM_GetComponent();
            if (fsm.moving_isEntering) {
                fsm.moving_isEntering = false;
                GameRecordDomain.RecordCurrentState(ctx);
            }

            // Move & Push
            GameRoleDomain.ApplyEasingMoveAndPush(ctx, role, fixdt, () => {
                role.State_IncStageCounter();
                role.FSM_EnterIdle();
                var start = role.fsmCom.moving_start.RoundToVector2Int();
                onEnd?.Invoke();
            });
        }

        static void FixedTickFSM_Dead(GameBusinessContext ctx, RoleEntity role, float fixdt, Action onEnd) {
            RoleFSMComponent fsm = role.FSM_GetComponent();
            if (fsm.dead_isEntering) {
                fsm.dead_isEntering = false;

                // VFX
                var vfxTable = ctx.templateInfraContext.VFXTable_Get();
                VFXApp.AddVFXToWorld(ctx.vfxContext, vfxTable.roleDeadVFX.name, vfxTable.roleDeadVFXDuration, role.Pos);

                // Camera
                GameCameraDomain.ShakeOnce(ctx);
                role.needTearDown = true;

                onEnd?.Invoke();
            }
        }

    }

}