using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

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

        public static void ApplyEasingMoveAndPush(GameBusinessContext ctx, RoleEntity role, float dt, Action onEnd) {
            var isEnd = false;
            ApplyEasingMove(ctx, role, dt, () => {
                isEnd = true;
            });
            var fsm = role.fsmCom;
            var push = fsm.moving_pushTarget;
            if (!push) {
                if (isEnd) {
                    onEnd.Invoke();
                }
                return;
            }
            var entityType = fsm.moving_pushTargetType;
            if (entityType == EntityType.Block) {
                PushBlock(ctx, role, isEnd, onEnd);
            } else if (entityType == EntityType.Goal) {
                PushGoal(ctx, role, isEnd, onEnd);
            }

        }

        static void PushGoal(GameBusinessContext ctx, RoleEntity role, bool isEnd, Action onEnd) {
            var fsm = role.fsmCom;
            var goalIndex = fsm.moving_pushTargetIndex;
            var has = ctx.goalRepo.TryGetGoal(goalIndex, out var goalEntity);
            if (!has) {
                GLog.LogError($"Goal Not Found With Index: {goalIndex}");
                return;
            }
            ApplyPushGoal(ctx, role, goalEntity);
            if (isEnd) {
                var oldPos = fsm.moving_pushTargetStartPos;
                ctx.goalRepo.UpdatePos(oldPos, goalEntity);
                onEnd.Invoke();
            }
        }

        static void PushBlock(GameBusinessContext ctx, RoleEntity role, bool isEnd, Action onEnd) {
            var fsm = role.fsmCom;
            var blockIndex = fsm.moving_pushTargetIndex;
            var has = ctx.blockRepo.TryGetBlock(blockIndex, out var blockEntity);
            if (!has) {
                GLog.LogError($"Block Not Found With Index: {blockIndex}");
                return;
            }
            ApplyPushBlock(ctx, role, blockEntity);
            if (isEnd) {
                var oldPos = fsm.moving_pushTargetStartPos;
                ctx.blockRepo.UpdatePos(oldPos, blockEntity);
                onEnd.Invoke();
            }
        }

        static void ApplyEasingMove(GameBusinessContext ctx, RoleEntity role, float dt, Action onEnd) {
            var fsm = role.fsmCom;
            var start = fsm.moving_start;
            var end = fsm.moving_end;
            var durationSec = fsm.moving_durationSec;
            fsm.Moving_IncTimer(dt);
            var currentSec = fsm.moving_currentSec;
            var currentPos = EasingHelper.Easing2D(start, end, currentSec, durationSec, role.moveEasingType, role.moveEasingMode);
            role.Pos_SetPos(currentPos);
            if (currentSec >= durationSec) {
                role.Pos_SetPos(end);
                onEnd.Invoke();
            }
        }

        static void ApplyPushGoal(GameBusinessContext ctx, RoleEntity role, GoalEntity goalEntity) {
            var lastPos = role.lastFramePos;
            var offset = role.Pos - lastPos;
            var pos = goalEntity.Pos;
            pos += offset;
            goalEntity.Pos_SetPos(pos);
        }

        static void ApplyPushBlock(GameBusinessContext ctx, RoleEntity role, BlockEntity blockEntity) {
            var lastPos = role.lastFramePos;
            var offset = role.Pos - lastPos;
            var pos = blockEntity.Pos;
            pos += offset;
            blockEntity.Pos_SetPos(pos);
        }

        public static void CheckAndApplyAllRoleDead(GameBusinessContext ctx) {
            var roleLen = ctx.roleRepo.TakeAll(out var roleArr);
            for (int i = 0; i < roleLen; i++) {
                var role = roleArr[i];
                CheckAndApplyDead(ctx, role);
            }
        }

        public static void CheckAndApplyDead(GameBusinessContext ctx, RoleEntity role) {
            var die = CheckInSpike(ctx, role);
            if (die) {
                role.FSM_EnterDead();
                return;
            }
        }

        static bool CheckInSpike(GameBusinessContext ctx, RoleEntity role) {
            var inSpike = false;
            GridUtils.ForEachGridBySize(role.PosInt, role.size, (grid) => {
                // Spike
                inSpike |= (ctx.spikeRepo.Has(grid));
                // Terrain Spike
                inSpike |= ctx.currentMapEntity.Terrain_HasSpike(grid);
            });
            return inSpike;
        }

    }

}