#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Oshi {

    public static class Vector2SortingHelper {

        public static List<Vector3> Sort(List<Vector2> vectors) {
            vectors = RemoveDuplicates(vectors);
            vectors = SortVectorsByNearistAxis(vectors);
            return Vector2ToVector3(vectors);
        }

        static List<Vector3> Vector2ToVector3(List<Vector2> vectors) {
            return vectors.Select(v => (Vector3)v).ToList();
        }

        static List<Vector2> RemoveDuplicates(List<Vector2> vectors) {
            return vectors
                .GroupBy(v => v)
                .Where(g => g.Count() % 2 == 1)
                .Select(g => g.Key)
                .ToList();
        }

        // 旅行商问题(TSP)的变种: 从一个点出发，经过所有点，最后回到出发点，使得总路径最短(约束: 只走直线)
        static List<Vector2> SortVectorsByNearistAxis(List<Vector2> vectors) {
            if (vectors.Count == 0)
                return new List<Vector2>();

            List<Vector2> sorted = new List<Vector2>();
            Vector2 current = vectors[0];
            sorted.Add(current);
            vectors.RemoveAt(0);

            while (vectors.Count > 0) {
                Vector2 next = FindNextPointInNearistAxis(current, vectors);
                sorted.Add(next);
                vectors.Remove(next);
                current = next;
            }

            return sorted;
        }

        static Vector2 FindNextPointInNearistAxis(Vector2 current, List<Vector2> vectors) {
            Vector2? closestMatch = null;
            float minDistance = float.MaxValue;

            foreach (var point in vectors) {
                float distance = Vector2.Distance(current, point);
                if ((current.x == point.x || current.y == point.y) && distance < minDistance) {
                    closestMatch = point;
                    minDistance = distance;
                }
            }

            return closestMatch ?? vectors[0];
        }
    }

}
#endif