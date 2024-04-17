using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public class GoalRepository {

        Dictionary<int, GoalEntity> all;
        Dictionary<Vector2Int, GoalEntity> posMap;
        GoalEntity[] temp;

        public GoalRepository() {
            all = new Dictionary<int, GoalEntity>();
            posMap = new Dictionary<Vector2Int, GoalEntity>();
            temp = new GoalEntity[1000];
        }

        public void Add(GoalEntity goal) {
            all.Add(goal.entityIndex, goal);
            GridUtils.ForEachGridBySize(goal.PosInt, goal.sizeInt, (grid) => {
                posMap.Add(grid, goal);
            });
        }

        public bool Has(Vector2Int pos) {
            return posMap.ContainsKey(pos);
        }

        public bool HasDifferent(Vector2Int pos, int index) {
            var has = posMap.TryGetValue(pos, out var block);
            return has && block.entityIndex != index;
        }

        public int TakeAll(out GoalEntity[] goals) {
            int count = all.Count;
            if (count > temp.Length) {
                temp = new GoalEntity[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(temp, 0);
            goals = temp;
            return count;
        }

        public void UpdatePos(Vector2Int oldPos, GoalEntity goal) {
            GridUtils.ForEachGridBySize(oldPos, goal.sizeInt, (grid) => {
                posMap.Remove(grid);
            });
            GridUtils.ForEachGridBySize(goal.PosInt, goal.sizeInt, (grid) => {
                posMap.Add(grid, goal);
            });
        }

        public void Remove(GoalEntity goal) {
            all.Remove(goal.entityIndex);
            GridUtils.ForEachGridBySize(goal.PosInt, goal.sizeInt, (grid) => {
                posMap.Remove(grid);
            });
        }

        public bool TryGetGoal(int entityID, out GoalEntity goal) {
            return all.TryGetValue(entityID, out goal);
        }

        public bool IsInRange(int entityID, in Vector2 pos, float range) {
            bool has = TryGetGoal(entityID, out var goal);
            if (!has) {
                return false;
            }
            return Vector2.SqrMagnitude(goal.Pos - pos) <= range * range;
        }

        public void ForEach(Action<GoalEntity> action) {
            foreach (var goal in all.Values) {
                action(goal);
            }
        }

        public GoalEntity GetNeareast(Vector2 pos, float radius) {
            GoalEntity nearestGoal = null;
            float nearestDist = float.MaxValue;
            float radiusSqr = radius * radius;
            foreach (var goal in all.Values) {
                float dist = Vector2.SqrMagnitude(goal.Pos - pos);
                if (dist <= radiusSqr && dist < nearestDist) {
                    nearestDist = dist;
                    nearestGoal = goal;
                }
            }
            return nearestGoal;
        }

        public void Clear() {
            all.Clear();
            posMap.Clear();
            Array.Clear(temp, 0, temp.Length);
        }

    }

}