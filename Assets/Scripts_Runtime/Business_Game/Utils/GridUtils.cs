using System;
using UnityEngine;

namespace Oshi {

    public static class GridUtils {

        public static Vector2Int[] temp = new Vector2Int[100];

        public static void ForEachGridBySize(Vector2Int pos, Vector2Int size, Action<Vector2Int> action) {
            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    var grid = new Vector2Int(pos.x + x, pos.y + y);
                    action(grid);
                }
            }
        }

        public static int GetCoveredStraightGrid(Vector2Int start, Vector2Int end, Vector2Int[] result) {
            var dir = end - start;
            for (int i = 0; i < dir.x; i++) {
                result[i] = new Vector2Int(start.x + i, start.y);
            }
            for (int i = 0; i < dir.y; i++) {
                result[i] = new Vector2Int(start.x, start.y + i);
            }
            return dir.x + dir.y;
        }

    }

}