using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public static class GameRecordDomain {

        public static void RecordCurrentState(GameBusinessContext ctx) {

            var model = new RecordModel();

            // Record Owner
            var owner = ctx.Role_GetOwner();
            if (owner.isMovingByGate) {
                return;
            }
            model.ownerPos = owner.PosInt;

            // Record Blocks
            var len = ctx.blockRepo.TakeAll(out var blocks);
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

            // Undo Owner
            var owner = ctx.Role_GetOwner();
            var oldPos = owner.PosInt;
            owner.Pos_SetPos(record.ownerPos);
            if (oldPos != record.ownerPos &&
            !(owner.fsmCom.status == RoleFSMStatus.Moving && !owner.isMovingByGate)) {
                ctx.roleRepo.UpdatePos(oldPos, owner);
            }

            // Undo Blocks
            var len = ctx.blockRepo.TakeAll(out var blocks);
            for (int i = 0; i < len; i++) {
                oldPos = blocks[i].PosInt;
                blocks[i].Pos_SetPos(record.blockPosArr[i]);
                if (oldPos == record.blockPosArr[i]) {
                    continue;
                }
                ctx.blockRepo.UpdatePos(oldPos, blocks[i]);
            }

            // Undo Gates
            len = ctx.gateRepo.TakeAll(out var gates);
            for (int i = 0; i < len; i++) {
                oldPos = gates[i].PosInt;
                gates[i].Pos_SetPos(record.gatePosArr[i]);
                if (oldPos == record.gatePosArr[i]) {
                    continue;
                }
                ctx.gateRepo.UpdatePos(oldPos, gates[i]);
            }

            // Undo Goals
            len = ctx.goalRepo.TakeAll(out var goals);
            for (int i = 0; i < len; i++) {
                oldPos = goals[i].PosInt;
                goals[i].Pos_SetPos(record.goalPosArr[i]);
                if (oldPos == record.goalPosArr[i]) {
                    continue;
                }
                ctx.goalRepo.UpdatePos(oldPos, goals[i]);
            }

            // Undo Spikes
            len = ctx.spikeRepo.TakeAll(out var spikes);
            for (int i = 0; i < len; i++) {
                oldPos = spikes[i].PosInt;
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
                if (path.isMoving || owner.fsmCom.status == RoleFSMStatus.Moving) {
                    path.ResetTimer();
                    continue;
                }
                path.Undo();
            }

            // Undo Role FSM
            owner.FSM_EnterIdle();

            // Undo Game State
            var game = ctx.gameEntity;
            game.fsmComponent.PlayerTurn_Enter();

            // Block Bloom
            len = ctx.blockRepo.TakeAll(out var blockArr);
            for (int i = 0; i < len; i++) {
                var block = blockArr[i];
                GameBlockDomain.ApplyBloom(ctx, block);
            }

        }

    }

}