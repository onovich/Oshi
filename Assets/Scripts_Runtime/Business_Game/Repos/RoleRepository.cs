using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oshi {

    public class RoleRepository {

        Dictionary<int, RoleEntity> all;
        Dictionary<Vector2Int, RoleEntity> posMap;
        RoleEntity[] temp;

        public RoleRepository() {
            all = new Dictionary<int, RoleEntity>();
            posMap = new Dictionary<Vector2Int, RoleEntity>();
            temp = new RoleEntity[1000];
        }

        public bool Has(Vector2Int pos) {
            return posMap.ContainsKey(pos);
        }

        public bool HasDifferent(Vector2Int pos, int index) {
            var has = posMap.TryGetValue(pos, out var role);
            return has && role.entityID != index;
        }

        public void UpdatePos(Vector2Int oldPos, RoleEntity role) {
            posMap.Remove(oldPos);
            posMap.Add(role.PosInt, role);
        }

        public void Add(RoleEntity role) {
            all.Add(role.entityID, role);
            posMap.Add(role.PosInt, role);
        }

        public int TakeAll(out RoleEntity[] roles) {
            int count = all.Count;
            if (count > temp.Length) {
                temp = new RoleEntity[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(temp, 0);
            roles = temp;
            return count;
        }

        public void Remove(RoleEntity role) {
            all.Remove(role.entityID);
            posMap.Remove(role.PosInt);
        }

        public bool TryGetRole(int entityID, out RoleEntity role) {
            return all.TryGetValue(entityID, out role);
        }

        public bool IsInRange(int entityID, in Vector2 pos, float range) {
            bool has = TryGetRole(entityID, out var role);
            if (!has) {
                return false;
            }
            return Vector2.SqrMagnitude(role.Pos - pos) <= range * range;
        }

        public void ForEach(Action<RoleEntity> action) {
            foreach (var role in all.Values) {
                action(role);
            }
        }

        public RoleEntity GetNeareast(AllyStatus allyStatus, Vector2 pos, float radius) {
            RoleEntity nearestRole = null;
            float nearestDist = float.MaxValue;
            float radiusSqr = radius * radius;
            foreach (var role in all.Values) {
                if (role.allyStatus != allyStatus) {
                    continue;
                }
                float dist = Vector2.SqrMagnitude(role.Pos - pos);
                if (dist <= radiusSqr && dist < nearestDist) {
                    nearestDist = dist;
                    nearestRole = role;
                }
            }
            return nearestRole;
        }

        public void Clear() {
            all.Clear();
            posMap.Clear();
            Array.Clear(temp, 0, temp.Length);
        }

    }

}