#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class BlockEM : SerializedMonoBehaviour {

        [Header("Bake Target")]
        public BlockTM blockTM;

        [Header("Block Info")]
        public int typeID;
        public string typeName;

        [Header("Fake Block")]
        public bool isFake;

        [Header("Block Number")]
        public int number;
        public bool showNumber;

        [Header("Block Mesh")]
        public Sprite mesh;
        public UnityEngine.Color color;
        public Material meshMaterial_default;
        public Material meshMaterial_bloom;

        [Header("Block Shapes")]
        public ShapeTM[] shapes;

        [Header("Block VFX")]
        public GameObject deadVFX;
        public float deadVFXDuration;

        [Button("Load")]
        void Load() {
            typeID = blockTM.typeID;
            typeName = blockTM.typeName;
            isFake = blockTM.isFake;
            number = blockTM.number;
            showNumber = blockTM.showNumber;
            mesh = blockTM.mesh;
            color = blockTM.meshColor;
            meshMaterial_default = blockTM.meshMaterial_default;
            meshMaterial_bloom = blockTM.meshMaterial_bloom;
            deadVFX = blockTM.deadVFX;
            deadVFXDuration = blockTM.deadVFXDuration;
            GetShapes();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        void GetShapes() {
            if (shapes == null) return;
            if (blockTM == null) return;
            if (blockTM.shapeArr == null) return;
            shapes = new ShapeTM[blockTM.shapeArr.Length];
            for (int i = 0; i < blockTM.shapeArr.Length; i++) {
                var shape = blockTM.shapeArr[i];
                shapes[i] = shape;
            }
        }

        void BakeShapes() {
            var shapeList = new List<ShapeTM>();
            for (int i = 0; i < shapes.Length; i++) {
                var shape = shapes[i];
                shapeList.Add(shape);
            }
            blockTM.shapeArr = shapeList.ToArray();
        }

        [Button("Bake")]
        void Bake() {
            blockTM.typeID = typeID;
            blockTM.typeName = typeName;
            blockTM.isFake = isFake;
            blockTM.number = number;
            blockTM.showNumber = showNumber;
            blockTM.mesh = mesh;
            blockTM.meshColor = color;
            blockTM.meshMaterial_default = meshMaterial_default;
            blockTM.meshMaterial_bloom = meshMaterial_bloom;
            blockTM.deadVFX = deadVFX;
            blockTM.deadVFXDuration = deadVFXDuration;
            BakeShapes();
            EditorUtility.SetDirty(blockTM);
            AssetDatabase.SaveAssets();
        }

    }

}
#endif