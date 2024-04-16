using MortiseFrame.Swing;
using UnityEngine;

namespace Chouten {

    public static class GameRoleFSMController {

        public static void FixedTickFSM(GameBusinessContext ctx, RoleEntity role, float fixdt) {

            FixedTickFSM_Any(ctx, role, fixdt);

            RoleFSMStatus status = role.FSM_GetStatus();
            if (status == RoleFSMStatus.Idle) {
                FixedTickFSM_Idle(ctx, role, fixdt);
            } else if (status == RoleFSMStatus.Dead) {
                FixedTickFSM_Dead(ctx, role, fixdt);
            } else if (status == RoleFSMStatus.Casting) {
                FixedTickFSM_Casting(ctx, role, fixdt);
            } else if (status == RoleFSMStatus.Leaving) {
                FixedTickFSM_Leaving(ctx, role, fixdt);
            } else {
                GLog.LogError($"GameRoleFSMController.FixedTickFSM: unknown status: {status}");
            }

        }

        static void FixedTickFSM_Any(GameBusinessContext ctx, RoleEntity role, float fixdt) {

        }

        static void FixedTickFSM_Idle(GameBusinessContext ctx, RoleEntity role, float fixdt) {
            RoleFSMComponent fsm = role.FSM_GetComponent();
            if (fsm.idle_isEntering) {
                fsm.idle_isEntering = false;
            }

            // Move
            GameRoleDomain.ApplyMove(ctx, role, fixdt);

            // Cast
            if (role.allyStatus == AllyStatus.Player) {
                GameRoleDomain.ApplyCast(ctx, role);
            } else {
                GameRoleDomain.ApplyAutoCast(ctx, role);
            }

            // Stage
            GameRoleDomain.ApplyStage(ctx, role);

        }

        static void FixedTickFSM_Casting(GameBusinessContext ctx, RoleEntity role, float fixdt) {
            RoleFSMComponent fsm = role.FSM_GetComponent();
            if (fsm.casting_isEntering) {
                fsm.casting_isEntering = false;
            }

            // Move
            GameRoleDomain.ApplyMove(ctx, role, fixdt);

            // Damage
            if (fsm.casting_currentFrame == fsm.casting_damageFrame) {
                GameRoleDomain.ApplyDamage(ctx, role);
            }

            // Stage
            GameRoleDomain.ApplyStage(ctx, role);

            // Frame
            fsm.Casting_IncFrame();

            // CD
            if (role.allyStatus == AllyStatus.Enemy) {
                return;
            }
            if (fsm.casting_currentFrame >= fsm.casting_totalFrame) {
                fsm.EnterIdle();
            }
        }

        static void FixedTickFSM_Leaving(GameBusinessContext ctx, RoleEntity role, float fixdt) {
            RoleFSMComponent fsm = role.FSM_GetComponent();
            if (fsm.leaving_isEntering) {
                fsm.idle_isEntering = false;
            }

            // Move
            GameRoleDomain.ApplyMove(ctx, role, fixdt);

            // Stage
            GameRoleDomain.ApplyStage(ctx, role);

            // Leaving
            var startAlpha = 1.0f;
            var endAlpha = 0.0f;
            var duration = fsm.leaving_totalFrame;
            var current = fsm.leaving_currentFrame;
            if (current >= duration) {
                return;
            }
            var alpha = EasingHelper.Easing(startAlpha, endAlpha, current, duration, EasingType.Linear);
            role.Color_SetAlpha(alpha);
            fsm.Leaving_IncFrame();
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