using System;
using System.Collections.Generic;

namespace Alter {

    public class BlockRepository {

        Dictionary<int, BlockEntity> dict;
        List<BlockEntity> removeList;

        public BlockRepository() {
            dict = new Dictionary<int, BlockEntity>(100);
            removeList = new List<BlockEntity>(100);
        }

        public void Add(BlockEntity entity) {
            dict.Add(entity.entityID, entity);
        }

        public void Remove(BlockEntity entity) {
            dict.Remove(entity.entityID);
        }

        public void RemoveAll(Func<BlockEntity, bool> predicate) {
            removeList.Clear();
            foreach (var item in dict) {
                if (predicate(item.Value)) {
                    removeList.Add(item.Value);
                }
            }
            foreach (var item in removeList) {
                dict.Remove(item.entityID);
            }
        }

        public bool TryGet(int id, out BlockEntity entity) {
            return dict.TryGetValue(id, out entity);
        }

        public void ForEach(Action<BlockEntity> action) {
            foreach (var item in dict) {
                action(item.Value);
            }
        }

        public void Clear() {
            dict.Clear();
        }

    }

}