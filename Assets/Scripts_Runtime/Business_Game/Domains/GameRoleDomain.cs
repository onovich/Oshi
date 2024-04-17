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
                GLog.LogError($"Block Not Found At {role.Pos_GetNextGrid()}");
                return;
            }
            ApplyPush(ctx, role, blockEntity);
            if (isEnd) {
                var oldPos = fsm.moving_pushBlockOldPos;
                ctx.blockRepo.UpdatePos(oldPos, blockEntity);
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
                role.State_IncStageCounter();
                onEnd.Invoke();
            }
        }

        static void ApplyPush(GameBusinessContext ctx, RoleEntity role, BlockEntity blockEntity) {
            var lastPos = role.lastFramePos;
            var offset = role.Pos - lastPos;
            var pos = blockEntity.Pos;
            var oldPos = pos;
            pos += offset;
            blockEntity.Pos_SetPos(pos);
        }

        public static bool CheckMovable(GameBusinessContext ctx, RoleEntity role) {
            var allow = role.Move_CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos);
            allow &= ctx.wallRepo.Has(role.Pos_GetNextGrid()) == false;
            return allow;
        }

        public static bool CheckPushable(GameBusinessContext ctx, RoleEntity role, out BlockEntity block) {
            var has = ctx.blockRepo.TryGetBlockByPos(role.Pos_GetNextGrid(), out block);
            if (has == false) {
                return false;
            }

            var allow = true;
            var _block = block;
            GridUtils.ForEachGridBySize(block.PosInt, block.sizeInt, (grid) => {
                allow &= ctx.wallRepo.Has(grid) == false;
                allow &= ctx.blockRepo.HasDifferent(grid, _block.entityIndex) == false;
            });

            GLog.Log($"CheckPushable: {allow}");
            return allow;
        }

    }

}