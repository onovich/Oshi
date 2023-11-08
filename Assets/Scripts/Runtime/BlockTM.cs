using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class BlockTM {

    [TableMatrix(HorizontalTitle = "X Axis", VerticalTitle = "Y Axis", SquareCells = true), SerializeField]
    public int[,] shape = new int[5, 5]; // 直接初始化为5x5数组

    [ContextMenu("Print")]
    public void Print() {
        for (int i = 0; i < shape.GetLength(0); i++) {
            for (int j = 0; j < shape.GetLength(1); j++) {
                Debug.Log(shape[i, j]);
            }
        }
    }

}