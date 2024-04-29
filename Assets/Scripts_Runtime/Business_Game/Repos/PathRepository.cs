using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oshi {

    public class PathRepository {

        Dictionary<int, PathModel> all;
        PathModel[] temp;

        public PathRepository() {
            all = new Dictionary<int, PathModel>();
            temp = new PathModel[1000];
        }

        public void Add(PathModel path) {
            all.Add(path.index, path);
        }

        public int TakeAll(out PathModel[] paths) {
            int count = all.Count;
            if (count > temp.Length) {
                temp = new PathModel[(int)(count * 1.5f)];
            }
            all.Values.CopyTo(temp, 0);
            paths = temp;
            return count;
        }

        public void Remove(PathModel path) {
            all.Remove(path.index);
        }

        public bool TryGetPath(int index, out PathModel path) {
            return all.TryGetValue(index, out path);
        }

        public void ForEach(Action<PathModel> action) {
            foreach (var path in all.Values) {
                action(path);
            }
        }

        public void Clear() {
            all.Clear();
            Array.Clear(temp, 0, temp.Length);
        }

    }

}