using UnityEngine;

namespace Oshi {

    public static class GameGateDomain {

        public static GateEntity Spawn(GameBusinessContext ctx, int typeID, int index, Vector2Int pos, int nextGateIndex) {
            var gate = GameFactory.Gate_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              typeID,
                                              index,
                                              pos,
                                              nextGateIndex);

            var has = ctx.templateInfraContext.Gate_TryGet(typeID, out var gateTM);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }

            var shape = gate.shapeComponent.Get(gate.shapeIndex);
            int cellIndex = 0;
            shape.ForEachCell((localPos) => {
                cellIndex++;
                var cellPos = pos + localPos;
                var cell = GameCellDomain.Spawn(ctx, false, cellPos, gate.cellRoot);
                cell.SetSpr(gateTM.mesh);
                cell.SetSortingLayer(SortingLayerConst.Gate);
                cell.SetSprColor(gateTM.meshColor);
                cell.SetSprMaterial(gateTM.meshMaterial);
                cell.index = cellIndex;
                gate.cellSlotComponent.Add(cell);
                cell.SetParent(gate.transform);
            });

            ctx.gateRepo.Add(gate);
            return gate;
        }

        public static bool TryGetNextGate(GameBusinessContext ctx, GateEntity gate, out GateEntity next) {
            var nextGateIndex = gate.nextGateIndex;
            return ctx.gateRepo.TryGetGate(nextGateIndex, out next);
        }

        public static void UnSpawn(GameBusinessContext ctx, GateEntity gate) {
            ctx.gateRepo.Remove(gate);
            gate.TearDown();
        }

        static bool CheckInSpike(GameBusinessContext ctx, GateEntity gate) {
            var pos = gate.PosInt;
            var inSpike = false;
            gate.cellSlotComponent.ForEach((index, mod) => {
                // Spike
                inSpike |= (ctx.spikeRepo.Has(mod.LocalPosInt + pos));
                // Terrain Spike
                inSpike |= (ctx.currentMapEntity.Terrain_HasSpike(mod.LocalPosInt + pos));
            });
            return inSpike;
        }

        static void ResetGate(GameBusinessContext ctx, GateEntity gate) {
            var originalPos = gate.originalPos;
            var oldPos = gate.PosInt;
            gate.Pos_SetPos(originalPos);
            ctx.gateRepo.UpdatePos(oldPos, gate);

            // VFX
            var vfxTable = ctx.templateInfraContext.VFXTable_Get();
            VFXApp.AddVFXToWorld(ctx.vfxContext, vfxTable.gateDeadVFX.name, vfxTable.gateDeadVFXDuration, gate.Pos);

            // Camera
            GameCameraDomain.ShakeOnce(ctx);
        }

        public static void CheckAndResetGate(GameBusinessContext ctx, GateEntity gate) {
            var owner = ctx.Role_GetOwner();
            if (owner == null || owner.fsmCom.status != RoleFSMStatus.Idle) {
                return;
            }

            var inSpike = CheckInSpike(ctx, gate);
            if (inSpike) {
                ResetGate(ctx, gate);
            }
        }

    }

}