using Codice.CM.Client.Differences;
using MortiseFrame.Swing;
using UnityEngine;

namespace Alter {

    public static class GameRoleFSMController {

        public static void FixedTickFSM(GameBusinessContext ctx, RoleEntity role, float fixdt) {

            FixedTickFSM_Any(ctx, role, fixdt);

            RoleFSMStatus status = role.FSM_GetStatus();
            if (status == RoleFSMStatus.Idle) {
                FixedTickFSM_Idle(ctx, role, fixdt);
            } else if (status == RoleFSMStatus.Moving) {
                FixedTickFSM_Moving(ctx, role, fixdt);
            } else if (status == RoleFSMStatus.Dead) {
                FixedTickFSM_Dead(ctx, role, fixdt);
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
            var succ = GameRoleDomain.CheckMovable(ctx, role);
            if (!succ) {
                return;
            }

            // Push
            succ = GameRoleDomain.CheckPushNeed(ctx, role);
            if (!succ) {
                role.FSM_EnterMoving(role.moveDurationSec);
            }

            succ = GameRoleDomain.CheckPushable(ctx, role, out var block);
            if (!succ) {
                return;
            }
            role.FSM_EnterMoving(role.moveDurationSec, true, block.entityIndex, block.PosInt);
        }

        static void FixedTickFSM_Moving(GameBusinessContext ctx, RoleEntity role, float fixdt) {
            RoleFSMComponent fsm = role.FSM_GetComponent();
            if (fsm.moving_isEntering) {
                fsm.moving_isEntering = false;
            }

            // Move & Push
            GameRoleDomain.ApplyEasingMoveAndPush(ctx, role, fixdt, () => {
                role.FSM_EnterIdle();
            });

        }

        static void FixedTickFSM_Dead(GameBusinessContext ctx, RoleEntity role, float fixdt) {
            RoleFSMComponent fsm = role.FSM_GetComponent();
            if (fsm.dead_isEntering) {
                fsm.dead_isEntering = false;
            }

            // VFX
            VFXApp.AddVFXToWorld(ctx.vfxContext, role.deadVFXName, role.deadVFXDuration, role.Pos);

            // Camera
            GameCameraDomain.ShakeOnce(ctx);
            role.needTearDown = true;
        }

    }

}