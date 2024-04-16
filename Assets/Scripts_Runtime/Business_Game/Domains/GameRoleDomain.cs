using UnityEngine;

namespace Chouten {

    public static class GameRoleDomain {

        public static RoleEntity Spawn(GameBusinessContext ctx, int typeID, Vector2 pos, Vector2 dir) {
            var role = GameFactory.Role_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              ctx.idRecordService,
                                              typeID,
                                              pos,
                                              dir);

            ctx.roleRepo.Add(role);
            return role;
        }

        public static void CheckAndUnSpawn(GameBusinessContext ctx, RoleEntity role) {
            if (role.needTearDown) {
                UnSpawn(ctx, role);
            }
        }

        public static void ApplyDamage(GameBusinessContext ctx, RoleEntity role) {
            RoleEntity target;
            if (role.allyStatus == AllyStatus.Enemy) {
                target = ctx.Role_GetOwner();
            } else {
                target = ctx.Role_GetNearestEnemy(role);
            }

            if (target == null) {
                return;
            }

            var distSqr = (target.Pos - role.Pos).sqrMagnitude;
            if (distSqr > role.attackDistance * role.attackDistance) {
                return;
            }

            target.hp -= 1;
            GameCameraDomain.ShakeOnce(ctx);

            if (target.hp <= 0) {
                target.FSM_EnterDead();
            }
        }

        public static void ApplyStage(GameBusinessContext ctx, RoleEntity role) {
            if (role.allyStatus == AllyStatus.Player) {
                return;
            }
            var faceDir = role.faceDir;
            var map = ctx.currentMapEntity;

            var middleBound = map.middlePos;
            var fsm = role.FSM_GetComponent();
            if (fsm.status == RoleFSMStatus.Leaving) {
                return;
            }

            var has = ctx.templateInfraContext.Role_TryGet(role.typeID, out var roleTM);
            if (!has) {
                GLog.LogError($"Role {role.entityID} has no template");
                return;
            }

            if (faceDir.x > 0 && role.Pos.x > middleBound.x || faceDir.x < 0 && role.Pos.x < middleBound.x) {
                fsm.EnterLeaving(roleTM.leavingTotalFrame);
            }
            var leaveBound = faceDir.x > 0 ? map.rightBound : map.leftBound;
            if (faceDir.x > 0 && role.Pos.x > leaveBound.x || faceDir.x < 0 && role.Pos.x < leaveBound.x) {
                role.needTearDown = true;
            }
        }

        public static void ApplyAutoCast(GameBusinessContext ctx, RoleEntity role) {
            if (role.allyStatus == AllyStatus.Player) {
                return;
            }
            if (role.fsmCom.status == RoleFSMStatus.Leaving) {
                return;
            }
            var targetPos = ctx.Role_GetOwner().Pos;
            var rolePos = role.Pos;
            var distSqr = (targetPos - rolePos).sqrMagnitude;
            if (distSqr < role.attackDistance * role.attackDistance) {
                role.Cast();
            }
            return;
        }

        public static void UnSpawn(GameBusinessContext ctx, RoleEntity role) {
            ctx.roleRepo.Remove(role);
            role.TearDown();
        }

        public static void ApplyMove(GameBusinessContext ctx, RoleEntity role, float dt) {
            role.Move_ApplyMove(dt);
        }

        public static void ApplyCast(GameBusinessContext ctx, RoleEntity role) {
            role.Cast_ApplyCast();
        }

    }

}