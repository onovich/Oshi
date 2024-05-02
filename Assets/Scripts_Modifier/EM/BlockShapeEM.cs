#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class BlockShapeEM : SerializedMonoBehaviour {

        [Header("Bake Target")]
        public ShapeTM shapeTM;

        [Header("Block Cells")]
        public Vector2Int sizeInt;

        [Button("Load")]
        void Load() {
            sizeInt = shapeTM.sizeInt;
            GetCells();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        void BakeCells() {
            List<Vector2Int> cellList = new List<Vector2Int>();
            for (int x = 0; x < sizeInt.x; x++) {
                for (int y = 0; y < sizeInt.y; y++) {
                    if (cells[x, y]) {
                        cellList.Add(new Vector2Int(x, sizeInt.y - 1 - y));
                    }
                }
            }
            shapeTM.cells = cellList.ToArray();
        }

        [Button("Bake")]
        void Bake() {
            shapeTM.sizeInt = sizeInt;
            BakeCells();
            EditorUtility.SetDirty(shapeTM);
            AssetDatabase.SaveAssets();
        }

        void GetCells() {
            cells = new bool[sizeInt.x, sizeInt.y];
            for (int i = 0; i < shapeTM.cells.Length; i++) {
                var x = shapeTM.cells[i].x;
                var y = sizeInt.y - 1 - shapeTM.cells[i].y;
                cells[x, y] = true;
            }
        }

        [Button("Clear")]
        void Clear() {
            cells = new bool[sizeInt.x, sizeInt.y];
        }

        [Button("Resize")]
        void Resize() {
            var newCells = new bool[sizeInt.x, sizeInt.y];
            for (int x = 0; x < Mathf.Min(sizeInt.x, cells.GetLength(0)); x++) {
                for (int y = 0; y < Mathf.Min(sizeInt.y, cells.GetLength(1)); y++) {
                    newCells[x, y] = cells[x, y];
                }
            }
            cells = newCells;
        }

        [TableMatrix(DrawElementMethod = "DrawCell", SquareCells = true)]
        public bool[,] cells;

        bool DrawCell(Rect rect, bool value) {
            if (Event.current.type == EventType.MouseDown &&
            rect.Contains(Event.current.mousePosition)) {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }
            EditorGUI.DrawRect(
                rect.Padding(1),
                value ? new UnityEngine.Color(0.1f, 0.8f, 0.2f) :
                new UnityEngine.Color(0, 0, 0, 0.5f));
            return value;
        }
    }

}
#endif