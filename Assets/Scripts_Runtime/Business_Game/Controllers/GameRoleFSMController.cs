using System;
using Codice.CM.Client.Differences;
using MortiseFrame.Swing;
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
                succ = GridUtils.TryGetLastWalkableGrid(ctx, role.PosInt, role.Pos_GetDir(), out target);
                if (!succ) {
                    return;
                }
            } else {
                succ = GridUtils.TryGetNeighbourWalkableGrid(ctx, role.PosInt, role.Pos_GetDir(), out target);
                if (!succ) {
                    return;
                }
            }


            // No Push
            succ = GridUtils.CheckPushable(ctx, role.PosInt, role.Pos_GetDir(), out var block);
            if (!succ) {
                role.FSM_EnterMoving(target, role.moveDurationSec);
                return;
            }
            // Push
            role.FSM_EnterMoving(role.PosInt + role.Pos_GetDir(), role.moveDurationSec, true, block.entityIndex, block.PosInt);
        }

        static void FixedTickFSM_Moving(GameBusinessContext ctx, RoleEntity role, float fixdt, Action onEnd) {
            RoleFSMComponent fsm = role.FSM_GetComponent();
            if (fsm.moving_isEntering) {
                fsm.moving_isEntering = false;
            }

            // Move & Push
            GameRoleDomain.ApplyEasingMoveAndPush(ctx, role, fixdt, () => {
                role.State_IncStageCounter();
                role.FSM_EnterIdle();
                onEnd?.Invoke();
            });
        }

        static void FixedTickFSM_Dead(GameBusinessContext ctx, RoleEntity role, float fixdt, Action onEnd) {
            RoleFSMComponent fsm = role.FSM_GetComponent();
            if (fsm.dead_isEntering) {
                fsm.dead_isEntering = false;

                // VFX
                VFXApp.AddVFXToWorld(ctx.vfxContext, role.deadVFXName, role.deadVFXDuration, role.Pos);

                // Camera
                GameCameraDomain.ShakeOnce(ctx);
                role.needTearDown = true;

                onEnd?.Invoke();
            }
        }

    }

}