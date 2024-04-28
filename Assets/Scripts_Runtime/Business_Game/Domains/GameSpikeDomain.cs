using UnityEngine;

namespace Oshi {

    public static class GameSpikeDomain {

        public static SpikeEntity Spawn(GameBusinessContext ctx, int typeID, int index, Vector2Int pos) {
            var spike = GameFactory.Spike_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              typeID,
                                              index,
                                              pos);

            ctx.spikeRepo.Add(spike);

            var has = ctx.templateInfraContext.Spike_TryGet(typeID, out var spikeTM);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }

            var shape = spike.shapeComponent.Get(spike.shapeIndex);
            int cellIndex = 0;
            shape.ForEachCell((localPos) => {
                cellIndex++;
                var cellPos = pos + localPos;
                var cell = GameCellDomain.Spawn(ctx, cellPos);
                cell.SetSpr(spikeTM.mesh);
                cell.SetSprColor(spikeTM.meshColor);
                cell.SetSprMaterial(spikeTM.meshMaterial);
                cell.index = cellIndex;
                spike.cellSlotComponent.Add(cell);
                cell.SetParent(spike.transform);
            });

            return spike;
        }

        public static void UnSpawn(GameBusinessContext ctx, SpikeEntity spike) {
            ctx.spikeRepo.Remove(spike);
            spike.TearDown();
        }

    }

}