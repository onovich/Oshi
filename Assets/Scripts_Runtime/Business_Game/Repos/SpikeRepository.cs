using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oshi {

    public class SpikeRepository {

        Dictionary<int, SpikeEntity> all;
        Dictionary<Vector2Int, SpikeEntity> posMap;
        SpikeEntity[] temp;

        public SpikeRepository() {
            all = new Dictionary<int, SpikeEntity>();
            posMap = new Dictionary<Vector2Int, SpikeEntity>();
            temp = new SpikeEntity[1000];
        }

        public void Add(SpikeEntity spike) {
            all.Add(spike.entityIndex, spike);
            spike.cellSlotComponent.ForEach((index, mod) => {
                posMap.Add(mod.LocalPosInt + spike.PosInt, spike);
            });
        }

        public bool Has(Vector2Int pos) {
            return posMap.ContainsKey(pos);
        }

        public bool HasDifferent(Vector2Int pos, int index) {
            var has = posMap.TryGetValue(pos, out var block);
            return has && block.entityIndex != index;
        }

        public int TakeAll(out SpikeEntity[] spikes) {
            int count = all.Count;
            if (count > temp.Length) {
                temp = new SpikeEntity[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(temp, 0);
            spikes = temp;
            return count;
        }

        public void UpdatePos(Vector2Int oldPos, SpikeEntity spike) {
            spike.cellSlotComponent.ForEach((index, mod) => {
                posMap.Remove(mod.LocalPosInt + oldPos);
            });
            spike.cellSlotComponent.ForEach((index, mod) => {
                posMap.Add(mod.LocalPosInt + spike.PosInt, spike);
            });
        }

        public void Remove(SpikeEntity spike) {
            all.Remove(spike.entityIndex);
            spike.cellSlotComponent.ForEach((index, mod) => {
                posMap.Remove(mod.LocalPosInt + spike.PosInt);
            });
        }

        public bool TryGetSpike(int entityID, out SpikeEntity spike) {
            return all.TryGetValue(entityID, out spike);
        }

        public bool IsInRange(int entityID, in Vector2 pos, float range) {
            bool has = TryGetSpike(entityID, out var spike);
            if (!has) {
                return false;
            }
            return Vector2.SqrMagnitude(spike.Pos - pos) <= range * range;
        }

        public void ForEach(Action<SpikeEntity> action) {
            foreach (var spike in all.Values) {
                action(spike);
            }
        }

        public SpikeEntity GetNeareast(Vector2 pos, float radius) {
            SpikeEntity nearestSpike = null;
            float nearestDist = float.MaxValue;
            float radiusSqr = radius * radius;
            foreach (var spike in all.Values) {
                float dist = Vector2.SqrMagnitude(spike.Pos - pos);
                if (dist <= radiusSqr && dist < nearestDist) {
                    nearestDist = dist;
                    nearestSpike = spike;
                }
            }
            return nearestSpike;
        }

        public void Clear() {
            all.Clear();
            posMap.Clear();
            Array.Clear(temp, 0, temp.Length);
        }

    }

}