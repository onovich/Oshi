using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oshi {

    public class WallRepository {

        Dictionary<int, WallEntity> all;
        Dictionary<Vector2Int, WallEntity> posMap;
        WallEntity[] temp;

        public WallRepository() {
            all = new Dictionary<int, WallEntity>();
            posMap = new Dictionary<Vector2Int, WallEntity>();
            temp = new WallEntity[1000];
        }

        public void Add(WallEntity wall) {
            all.Add(wall.entityIndex, wall);
            wall.cellSlotComponent.ForEach((index, mod) => {
                posMap.Add(mod.LocalPosInt + wall.PosInt, wall);
            });
        }

        public bool Has(Vector2Int pos) {
            return posMap.ContainsKey(pos);
        }

        public bool HasDifferent(Vector2Int pos, int index) {
            var has = posMap.TryGetValue(pos, out var block);
            return has && block.entityIndex != index;
        }

        public int TakeAll(out WallEntity[] walls) {
            int count = all.Count;
            if (count > temp.Length) {
                temp = new WallEntity[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(temp, 0);
            walls = temp;
            return count;
        }

        public void UpdatePos(Vector2Int oldPos, WallEntity wall) {
            wall.cellSlotComponent.ForEach((index, mod) => {
                posMap.Remove(mod.LocalPosInt + oldPos);
            });
            wall.cellSlotComponent.ForEach((index, mod) => {
                posMap.Add(mod.LocalPosInt + wall.PosInt, wall);
            });
        }

        public void Remove(WallEntity wall) {
            all.Remove(wall.entityIndex);
            wall.cellSlotComponent.ForEach((index, mod) => {
                posMap.Remove(mod.LocalPosInt + wall.PosInt);
            });
        }

        public bool TryGetWall(int entityID, out WallEntity wall) {
            return all.TryGetValue(entityID, out wall);
        }

        public bool IsInRange(int entityID, in Vector2 pos, float range) {
            bool has = TryGetWall(entityID, out var wall);
            if (!has) {
                return false;
            }
            return Vector2.SqrMagnitude(wall.Pos - pos) <= range * range;
        }

        public void ForEach(Action<WallEntity> action) {
            foreach (var wall in all.Values) {
                action(wall);
            }
        }

        public WallEntity GetNeareast(Vector2 pos, float radius) {
            WallEntity nearestWall = null;
            float nearestDist = float.MaxValue;
            float radiusSqr = radius * radius;
            foreach (var wall in all.Values) {
                float dist = Vector2.SqrMagnitude(wall.Pos - pos);
                if (dist <= radiusSqr && dist < nearestDist) {
                    nearestDist = dist;
                    nearestWall = wall;
                }
            }
            return nearestWall;
        }

        public void Clear() {
            all.Clear();
            posMap.Clear();
            Array.Clear(temp, 0, temp.Length);
        }

    }

}