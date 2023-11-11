using System;
using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public class BlockUnitSlotComponent {

        List<Transform> all;

        public BlockUnitSlotComponent() {
            all = new List<Transform>();
        }

        public void Add(Transform transform) {
            all.Add(transform);
        }

        public void Clear() {
            all.Clear();
        }

        public void ForEach(Action<Transform> action) {
            foreach (var item in all) {
                action(item);
            }
        }

        public List<Transform> GetAll() {
            return all;
        }

    }

}