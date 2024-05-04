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
            var push = fsm.moving_pushBlock;
            if (!push) {
                if (isEnd) {
                    onEnd.Invoke();
                }
                return;
            }
            var blockIndex = fsm.moving_pushBlockIndex;
            var has = ctx.blockRepo.TryGetBlock(blockIndex, out var blockEntity);
            if (!has) {
                GLog.LogError($"Block Not Found With Index: {blockIndex}");
                return;
            }
            ApplyPush(ctx, role, blockEntity, fsm.moving_pushStartPos, fsm.moving_pushEnd, fsm.moving_durationSec, fsm.moving_currentSec);
            if (isEnd) {
                var oldPos = fsm.moving_pushStartPos;
                ctx.blockRepo.UpdatePos(oldPos, blockEntity);
                blockEntity.Pos_SetPos(fsm.moving_pushEnd);
                onEnd.Invoke();
            }
        }

        static void ApplyEasingMove(GameBusinessContext ctx, RoleEntity role, float dt, Action onEnd) {
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
                onEnd.Invoke();
            }
        }

        static void ApplyPush(GameBusinessContext ctx, RoleEntity role, BlockEntity blockEntity, Vector2Int start, Vector2Int end, float duration, float current) {
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