#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class GoalEM : SerializedMonoBehaviour {

        [Header("Bake Target")]
        public GoalTM goalTM;

        [Header("Block Info")]
        public int typeID;
        public string typeName;

        [Header("Block Mesh")]
        public Sprite mesh;
        public UnityEngine.Color color;
        public Material meshMaterial;

        [Header("Block Shapes")]
        public ShapeTM[] shapes;

        [Button("Load")]
        void Load() {
            typeID = goalTM.typeID;
            typeName = goalTM.typeName;
            mesh = goalTM.mesh;
            color = goalTM.meshColor;
            meshMaterial = goalTM.meshMaterial;
            GetShapes();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        void GetShapes() {
            if (shapes == null) return;
            if (goalTM == null) return;
            if (goalTM.shapeArr == null) return;
            shapes = new ShapeTM[goalTM.shapeArr.Length];
            for (int i = 0; i < goalTM.shapeArr.Length; i++) {
                var shape = goalTM.shapeArr[i];
                shapes[i] = shape;
            }
        }

        void BakeShapes() {
            var shapeList = new List<ShapeTM>();
            for (int i = 0; i < shapes.Length; i++) {
                var shape = shapes[i];
                shapeList.Add(shape);
            }
            goalTM.shapeArr = shapeList.ToArray();
        }

        [Button("Bake")]
        void Bake() {
            goalTM.typeID = typeID;
            goalTM.typeName = typeName;
            goalTM.mesh = mesh;
            goalTM.meshColor = color;
            goalTM.meshMaterial = meshMaterial;
            BakeShapes();
            EditorUtility.SetDirty(goalTM);
            AssetDatabase.SaveAssets();
        }

    }

}
#endif