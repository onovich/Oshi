using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Examples;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockSO", menuName = "Alter/BlockSO", order = 0)]
public class BlockSO : SerializedScriptableObject {

    [TableMatrix(DrawElementMethod = "DrawBlock", HorizontalTitle = "Block Shape", SquareCells = true)]
    public bool[,] shape = new bool[5, 5]; // 直接初始化为5x5数组

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

    [Button("Print")]
    public void Print() {
        for (int i = 0; i < shape.GetLength(0); i++) {
            for (int j = 0; j < shape.GetLength(1); j++) {
                Debug.Log(shape[i, j]);
            }
        }
    }

}