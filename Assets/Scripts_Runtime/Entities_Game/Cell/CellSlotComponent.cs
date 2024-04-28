using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oshi {
    public class CellSlotComponent {

        SortedList<int/*index*/, CellMod> all;

        public CellSlotComponent() {
            all = new SortedList<int, CellMod>();
        }

        public void Add(CellMod mod) {
            all.Add(mod.index, mod);
        }

        public void ForEach(Action<int, CellMod> action) {
            for (int i = 0; i < all.Count; i++) {
                var index = all.Keys[i];
                var mod = all.Values[i];
                action(index, mod);
            }
        }

        public CellMod Get(int index) {
            if (all.ContainsKey(index)) {
                return all[index];
            }
            return null;
        }

        public void Clear() {
            all.Clear();
        }

    }

}