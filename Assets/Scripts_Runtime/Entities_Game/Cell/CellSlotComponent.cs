using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oshi {
    public class CellSlotComponent {

        SortedList<int/*index*/, CellMod> all;
        CellMod[] temp;

        public CellSlotComponent() {
            all = new SortedList<int, CellMod>();
            temp = new CellMod[100];
        }

        public void Add(CellMod mod) {
            all.Add(mod.index, mod);
        }

        public int TakeAll(out CellMod[] result) {
            int count = all.Count;
            if (count > temp.Length) {
                temp = new CellMod[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(temp, 0);
            result = temp;
            return count;
        }

        public CellMod Get(int index) {
            if (all.ContainsKey(index)) {
                return all[index];
            }
            return null;
        }

        public void Clear() {
            all.Clear();
            Array.Clear(temp, 0, temp.Length);
        }

    }

}