using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oshi {

    public class GateRepository {

        Dictionary<int, GateEntity> all;
        Dictionary<Vector2Int, GateEntity> posMap;
        GateEntity[] temp;

        public GateRepository() {
            all = new Dictionary<int, GateEntity>();
            posMap = new Dictionary<Vector2Int, GateEntity>();
            temp = new GateEntity[1000];
        }

        public void Add(GateEntity gate) {
            all.Add(gate.entityIndex, gate);
            var len = gate.cellSlotComponent.TakeAll(out var mods);
            for (int i = 0; i < len; i++) {
                var mod = mods[i];
                posMap.Add(mod.LocalPosInt + gate.PosInt, gate);
            }
        }

        public bool Has(Vector2Int pos) {
            return posMap.ContainsKey(pos);
        }

        public bool HasDifferent(Vector2Int pos, int index) {
            var has = posMap.TryGetValue(pos, out var block);
            return has && block.entityIndex != index;
        }

        public int TakeAll(out GateEntity[] gates) {
            int count = all.Count;
            if (count > temp.Length) {
                temp = new GateEntity[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(temp, 0);
            gates = temp;
            return count;
        }

        public void UpdatePos(Vector2Int oldPos, GateEntity gate) {
            var len = gate.cellSlotComponent.TakeAll(out var mods);
            for(int i = 0; i < len; i++) {
                var mod = mods[i];
                posMap.Remove(mod.LocalPosInt + oldPos);
            }
            for(int i = 0; i < len; i++) {
                var mod = mods[i];
                posMap.Add(mod.LocalPosInt + gate.PosInt, gate);
            }
        }

        public void Remove(GateEntity gate) {
            all.Remove(gate.entityIndex);
            var len = gate.cellSlotComponent.TakeAll(out var mods);
            for(int i = 0; i < len; i++) {
                var mod = mods[i];
                posMap.Remove(mod.LocalPosInt + gate.PosInt);
            }
        }

        public bool TryGetGate(int index, out GateEntity gate) {
            return all.TryGetValue(index, out gate);
        }

        public bool TryGetGateByPos(Vector2Int pos, out GateEntity gate){
            return posMap.TryGetValue(pos, out gate);
        }

        public bool IsInRange(int entityID, in Vector2 pos, float range) {
            bool has = TryGetGate(entityID, out var gate);
            if (!has) {
                return false;
            }
            return Vector2.SqrMagnitude(gate.Pos - pos) <= range * range;
        }

        public void ForEach(Action<GateEntity> action) {
            foreach (var gate in all.Values) {
                action(gate);
            }
        }

        public GateEntity GetNeareast(Vector2 pos, float radius) {
            GateEntity nearestGate = null;
            float nearestDist = float.MaxValue;
            float radiusSqr = radius * radius;
            foreach (var gate in all.Values) {
                float dist = Vector2.SqrMagnitude(gate.Pos - pos);
                if (dist <= radiusSqr && dist < nearestDist) {
                    nearestDist = dist;
                    nearestGate = gate;
                }
            }
            return nearestGate;
        }

        public void Clear() {
            all.Clear();
            posMap.Clear();
            Array.Clear(temp, 0, temp.Length);
        }

    }

}