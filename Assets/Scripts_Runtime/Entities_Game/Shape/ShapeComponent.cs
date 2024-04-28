using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oshi {
    public class ShapeComponent {

        SortedList<int/*index*/, ShapeModel> all;
        public int Count => all.Count;

        public ShapeComponent() {
            all = new SortedList<int, ShapeModel>();
        }

        public void Add(ShapeModel model) {
            all.Add(model.index, model);
        }

        public ShapeModel GetNext(int index) {
            var nextIndex = index + 1;
            if (nextIndex >= all.Count) {
                nextIndex = 0;
            }
            if (all.ContainsKey(nextIndex)) {
                return all[nextIndex];
            }
            Debug.LogError($"ShapeComponent.GetNext: {nextIndex} not found");
            return default;
        }

        public ShapeModel Get(int index) {
            if (all.ContainsKey(index)) {
                return all[index];
            }
            return default;
        }

        public void Clear() {
            all.Clear();
        }

    }

}