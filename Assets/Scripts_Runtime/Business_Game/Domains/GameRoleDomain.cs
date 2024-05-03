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
                onEnd.Invoke();
            }
        }

        static void ApplyPush(GameBusinessContext ctx, RoleEntity role, BlockEntity blockEntity) {
            var lastPos = role.lastFramePos;
            var offset = role.Pos - lastPos;
            var pos = blockEntity.Pos;
            pos += offset;
            blockEntity.Pos_SetPos(pos);
        }

        public static bool CheckMovable(GameBusinessContext ctx, RoleEntity role) {
            var allow = role.Move_CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos);
            // Wall
            allow &= ctx.wallRepo.Has(role.Pos_GetNextGrid()) == false;
            // Terrain Wall
            allow &= ctx.currentMapEntity.Terrain_HasWall(role.Pos_GetNextGrid()) == false;
            // Constraint
            allow &= role.Move_CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos);
            return allow;
        }

        public static bool CheckPushNeed(GameBusinessContext ctx, RoleEntity role) {
            var has = ctx.blockRepo.TryGetBlockByPos(role.Pos_GetNextGrid(), out var _);
            if (has == false) {
                return false;
            }

            return has;
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

        public static bool CheckPushable(GameBusinessContext ctx, RoleEntity role, out BlockEntity block) {
            var has = ctx.blockRepo.TryGetBlockByPos(role.Pos_GetNextGrid(), out block);
            if (has == false) {
                return false;
            }

            var allow = true;
            var _block = block;
            block.cellSlotComponent.ForEach((index, mod) => {
                // Wall
                allow &= ctx.wallRepo.Has(_block.PosInt + mod.LocalPosInt + role.Pos_GetNextDir()) == false;
                // Block
                allow &= ctx.blockRepo.HasDifferent(_block.PosInt + mod.LocalPosInt + role.Pos_GetNextDir(), _block.entityIndex) == false;
                // Terrain Wall
                allow &= ctx.currentMapEntity.Terrain_HasWall(_block.PosInt + mod.LocalPosInt + role.Pos_GetNextDir()) == false;
                // Constraint
                allow &= _block.Move_CheckConstraint(ctx.currentMapEntity.mapSize, ctx.currentMapEntity.Pos, _block.PosInt + mod.LocalPosInt, role.Pos_GetNextDir());
            });
            return allow;
        }

    }

}