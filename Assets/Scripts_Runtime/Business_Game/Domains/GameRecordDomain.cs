using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public static class GameRecordDomain {

        public static void RecordCurrentState(GameBusinessContext ctx) {

            // Record Roles
            var len = ctx.roleRepo.TakeAll(out var roles);
            var model = new RecordModel();
            model.rolePosArr = new Vector2Int[len];
            for (int i = 0; i < len; i++) {
                model.rolePosArr[i] = roles[i].PosInt;
            }

            // Record Blocks
            len = ctx.blockRepo.TakeAll(out var blocks);
            model.blockPosArr = new Vector2Int[len];
            for (int i = 0; i < len; i++) {
                model.blockPosArr[i] = blocks[i].PosInt;
            }

            // Record Gates
            len = ctx.gateRepo.TakeAll(out var gates);
            model.gatePosArr = new Vector2Int[len];
            for (int i = 0; i < len; i++) {
                model.gatePosArr[i] = gates[i].PosInt;
            }

            // Record Goals
            len = ctx.goalRepo.TakeAll(out var goals);
            model.goalPosArr = new Vector2Int[len];
            for (int i = 0; i < len; i++) {
                model.goalPosArr[i] = goals[i].PosInt;
            }

            // Record Spikes
            len = ctx.spikeRepo.TakeAll(out var spikes);
            model.spikePosArr = new Vector2Int[len];
            for (int i = 0; i < len; i++) {
                model.spikePosArr[i] = spikes[i].PosInt;
            }

            ctx.recordRepo.Push(model);

        }

        public static void UndoRecord(GameBusinessContext ctx) {

            var input = ctx.inputEntity;
            if (!input.isPressUndo) {
                return;
            }

            var succ = ctx.recordRepo.TryPop(out var record);
            if (succ == false) {
                return;
            }

            // Undo Roles
            var len = ctx.roleRepo.TakeAll(out var roles);
            for (int i = 0; i < len; i++) {
                var oldPos = roles[i].PosInt;
                roles[i].Pos_SetPos(record.rolePosArr[i]);
                if (oldPos == record.rolePosArr[i]) {
                    continue;
                }
                if (roles[i].fsmCom.status == RoleFSMStatus.Moving) {
                    continue;
                }
                ctx.roleRepo.UpdatePos(oldPos, roles[i]);
            }

            // Undo Blocks
            len = ctx.blockRepo.TakeAll(out var blocks);
            for (int i = 0; i < len; i++) {
                var oldPos = blocks[i].PosInt;
                blocks[i].Pos_SetPos(record.blockPosArr[i]);
                if (oldPos == record.blockPosArr[i]) {
                    continue;
                }
                ctx.blockRepo.UpdatePos(oldPos, blocks[i]);
            }

            // Undo Gates
            len = ctx.gateRepo.TakeAll(out var gates);
            for (int i = 0; i < len; i++) {
                var oldPos = gates[i].PosInt;
                gates[i].Pos_SetPos(record.gatePosArr[i]);
                if (oldPos == record.gatePosArr[i]) {
                    continue;
                }
                ctx.gateRepo.UpdatePos(oldPos, gates[i]);
            }

            // Undo Goals
            len = ctx.goalRepo.TakeAll(out var goals);
            for (int i = 0; i < len; i++) {
                var oldPos = goals[i].PosInt;
                goals[i].Pos_SetPos(record.goalPosArr[i]);
                if (oldPos == record.goalPosArr[i]) {
                    continue;
                }
                ctx.goalRepo.UpdatePos(oldPos, goals[i]);
            }

            // Undo Spikes
            len = ctx.spikeRepo.TakeAll(out var spikes);
            for (int i = 0; i < len; i++) {
                var oldPos = spikes[i].PosInt;
                spikes[i].Pos_SetPos(record.spikePosArr[i]);
                if (oldPos == record.spikePosArr[i]) {
                    continue;
                }
                ctx.spikeRepo.UpdatePos(oldPos, spikes[i]);
            }

            // Undo Path State
            len = ctx.pathRepo.TakeAll(out var paths);
            for (int i = 0; i < len; i++) {
                var path = paths[i];
                path.Undo();
            }

            // Undo Role FSM
            var owner = ctx.Role_GetOwner();
            owner.FSM_EnterIdle();

            // Undo Game State
            var game = ctx.gameEntity;
            game.fsmComponent.PlayerTurn_Enter();

        }

    }

}