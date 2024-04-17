using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alter {

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
            GridUtils.ForEachGridBySize(wall.PosInt, wall.sizeInt, (grid) => {
                posMap.Add(grid, wall);
            });
        }

        public bool Has(Vector2Int pos) {
            return posMap.ContainsKey(pos);
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

        public void ChangePos(Vector2Int oldPos, WallEntity wall) {
            posMap.Remove(oldPos);
            posMap.Add(wall.PosInt, wall);
        }

        public void Remove(WallEntity wall) {
            all.Remove(wall.entityIndex);
            GridUtils.ForEachGridBySize(wall.PosInt, wall.sizeInt, (grid) => {
                posMap.Remove(grid);
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