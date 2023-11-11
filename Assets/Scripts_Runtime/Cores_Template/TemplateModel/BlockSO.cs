using System.Text;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Alter {

    [CreateAssetMenu(fileName = "BlockSO", menuName = "Alter/BlockSO", order = 0)]
    public class BlockSO : SerializedScriptableObject {

        [TableMatrix(DrawElementMethod = "DrawBlock", HorizontalTitle = "Block Shape", SquareCells = true)]
        public bool[,] shape;
        public BlockTM tm;
        public int typeID;

        static bool DrawBlock(Rect rect, bool value) {
            if (Event.current.type == EventType.MouseDown &&
            rect.Contains(Event.current.mousePosition)) {
                value = !value;
                GUI.changed = true;
                Event.current.Use();
            }

            EditorGUI.DrawRect(
                rect.Padding(1),
                value ? new Color(0.1f, 0.8f, 0.2f)
                      : new Color(0, 0, 0, 0.5f));

            return value;

        }

        [Button("Init")]
        public void Clear() {
            shape = new bool[4, 4];
        }

        [Button("TestData")]
        public void Print() {
            StringBuilder sb = new StringBuilder();
            if (tm == null || tm.shape == null || tm.shape.GetLength(0) != 5 || tm.shape.GetLength(1) != 5) {
                Debug.LogError("tm is null");
                return;
            }
            for (int i = 0; i < tm.shape.GetLength(1); i++) {
                for (int j = 0; j < tm.shape.GetLength(0); j++) {
                    sb.Append(shape[j, i]).Append(" ");
                }
                if (i < shape.GetLength(1) - 1) {
                    sb.AppendLine();
                }
            }
            Debug.Log(sb.ToString());
        }

        [Button("Bake")]
        public void Bake() {

            tm = new BlockTM();
            tm.typeID = typeID;
            tm.shape = new bool[5, 5];
            for (int i = 0; i < shape.GetLength(1); i++) {
                for (int j = 0; j < shape.GetLength(0); j++) {
                    tm.shape[j, i] = shape[j, i];
                }
            }

            UnityEditor.EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

        }

    }

}