using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Alter {

    public static class GameRoleDomain {

        public static RoleEntity Spawn(GameBusinessContext ctx, int typeID, Vector2 pos) {
            var role = GameFactory.Role_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              ctx.idRecordService,
                                              typeID,
                                              pos);

            ctx.roleRepo.Add(role);
            return role;
        }

        public static void CheckAndUnSpawn(GameBusinessContext ctx, RoleEntity role) {
            if (role.needTearDown) {
                UnSpawn(ctx, role);
            }
        }

        public static void UnSpawn(GameBusinessContext ctx, RoleEntity role) {
            ctx.roleRepo.Remove(role);
            role.TearDown();
        }

        public static void ApplyEasingMove(GameBusinessContext ctx, RoleEntity role, float dt, Action onEnd) {
            var fsm = role.fsmCom;
            var start = fsm.moving_start;
            var end = fsm.moving_end;
            var durationSec = fsm.moving_durationSec;
            var currentSec = fsm.moving_currentSec;
            var currentPos = EasingHelper.Easing2D(start, end, currentSec, durationSec, role.moveEasingType, role.moveEasingMode);
            role.Pos_SetPos(currentPos);
            fsm.Moving_IncTimer(dt);
            if (currentSec >= durationSec) {
                role.Pos_SetPos(end);
                role.State_IncStageCounter();
                onEnd.Invoke();
            }
        }

        public static bool CheckMovable(GameBusinessContext ctx, RoleEntity role) {
            var allow = role.Move_CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos);
            allow &= ctx.blockRepo.Has(role.Pos_GetNextGrid()) == false;
            allow &= ctx.wallRepo.Has(role.Pos_GetNextGrid()) == false;
            return allow;
        }

    }

}