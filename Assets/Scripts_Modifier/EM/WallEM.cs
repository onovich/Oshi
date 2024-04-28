#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Oshi {

    public class WallEM : SerializedMonoBehaviour {

        [Header("Bake Target")]
        public WallTM wallTM;

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
            typeID = wallTM.typeID;
            typeName = wallTM.typeName;
            mesh = wallTM.mesh;
            color = wallTM.meshColor;
            meshMaterial = wallTM.meshMaterial;
            GetShapes();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        void GetShapes() {
            if (shapes == null) return;
            if (wallTM == null) return;
            if (wallTM.shapeArr == null) return;
            shapes = new ShapeTM[wallTM.shapeArr.Length];
            for (int i = 0; i < wallTM.shapeArr.Length; i++) {
                var shape = wallTM.shapeArr[i];
                shapes[i] = shape;
            }
        }

        void BakeShapes() {
            var shapeList = new List<ShapeTM>();
            for (int i = 0; i < shapes.Length; i++) {
                var shape = shapes[i];
                shapeList.Add(shape);
            }
            wallTM.shapeArr = shapeList.ToArray();
        }

        [Button("Bake")]
        void Bake() {
            wallTM.typeID = typeID;
            wallTM.typeName = typeName;
            wallTM.mesh = mesh;
            wallTM.meshColor = color;
            wallTM.meshMaterial = meshMaterial;
            BakeShapes();
            EditorUtility.SetDirty(wallTM);
            AssetDatabase.SaveAssets();
        }

    }

}
#endif