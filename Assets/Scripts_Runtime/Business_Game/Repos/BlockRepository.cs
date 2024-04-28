using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oshi {

    public class BlockRepository {

        Dictionary<int, BlockEntity> all;
        Dictionary<Vector2Int, BlockEntity> posMap;
        BlockEntity[] temp;

        public BlockRepository() {
            all = new Dictionary<int, BlockEntity>();
            posMap = new Dictionary<Vector2Int, BlockEntity>();
            temp = new BlockEntity[1000];
        }

        public void Add(BlockEntity block) {
            all.Add(block.entityIndex, block);
            block.cellSlotComponent.ForEach((index, mod) => {
                posMap.Add(mod.LocalPosInt + block.PosInt, block);
            });
        }

        public bool Has(Vector2Int pos) {
            return posMap.ContainsKey(pos);
        }

        public bool HasDifferent(Vector2Int pos, int index) {
            var has = posMap.TryGetValue(pos, out var block);
            return has && block.entityIndex != index;
        }

        public int TakeAll(out BlockEntity[] blocks) {
            int count = all.Count;
            if (count > temp.Length) {
                temp = new BlockEntity[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(temp, 0);
            blocks = temp;
            return count;
        }

        public void UpdatePos(Vector2Int oldPos, BlockEntity block) {
            block.cellSlotComponent.ForEach((index, mod) => {
                posMap.Remove(mod.LocalPosInt + oldPos);
            });
            block.cellSlotComponent.ForEach((index, mod) => {
                posMap.Add(mod.LocalPosInt + block.PosInt, block);
            });
        }

        public void Remove(BlockEntity block) {
            all.Remove(block.entityIndex);
            block.cellSlotComponent.ForEach((index, mod) => {
                posMap.Remove(mod.LocalPosInt + block.PosInt);
            });
        }

        public bool TryGetBlock(int index, out BlockEntity block) {
            return all.TryGetValue(index, out block);
        }

        public bool TryGetBlockByPos(Vector2Int pos, out BlockEntity block) {
            return posMap.TryGetValue(pos, out block);
        }

        public bool IsInRange(int entityID, in Vector2 pos, float range) {
            bool has = TryGetBlock(entityID, out var block);
            if (!has) {
                return false;
            }
            return Vector2.SqrMagnitude(block.Pos - pos) <= range * range;
        }

        public void ForEach(Action<BlockEntity> action) {
            foreach (var block in all.Values) {
                action(block);
            }
        }

        public BlockEntity GetNeareast(Vector2 pos, float radius) {
            BlockEntity nearestBlock = null;
            float nearestDist = float.MaxValue;
            float radiusSqr = radius * radius;
            foreach (var block in all.Values) {
                float dist = Vector2.SqrMagnitude(block.Pos - pos);
                if (dist <= radiusSqr && dist < nearestDist) {
                    nearestDist = dist;
                    nearestBlock = block;
                }
            }
            return nearestBlock;
        }

        public void Clear() {
            all.Clear();
            posMap.Clear();
            Array.Clear(temp, 0, temp.Length);
        }

    }

}