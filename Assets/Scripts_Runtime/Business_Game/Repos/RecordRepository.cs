using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oshi {

    public class RecordRepository {

        Stack<RecordModel> all;

        public RecordRepository() {
            all = new Stack<RecordModel>();
        }

        public void Push(RecordModel record) {
            all.Push(record);
        }

        public bool TryPop(out RecordModel record) {
            return all.TryPop(out record);
        }

        public void Clear() {
            all.Clear();
        }

    }

}