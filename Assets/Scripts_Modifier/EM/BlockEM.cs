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
        public Sprite fakeMesh;
        public Material fakeMaterial;
        public UnityEngine.Color fakeColor;

        [Header("Block Number")]
        public Material numberMaterial;
        public UnityEngine.Color numberColor;

        [Header("Block Mesh")]
        public Sprite mesh;
        public UnityEngine.Color color;
        public Material meshMaterial_default;
        public Material meshMaterial_bloom;

        [Header("Block Shapes")]
        public ShapeTM[] shapes;

        [Button("Load")]
        void Load() {
            typeID = blockTM.typeID;
            typeName = blockTM.typeName;
            fakeMesh = blockTM.fakeMesh;
            fakeMaterial = blockTM.fakeMaterial;
            fakeColor = blockTM.fakeColor;
            numberMaterial = blockTM.numberMaterial;
            numberColor = blockTM.numberColor;
            mesh = blockTM.mesh;
            color = blockTM.meshColor;
            meshMaterial_default = blockTM.meshMaterial_default;
            meshMaterial_bloom = blockTM.meshMaterial_bloom;
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
            blockTM.fakeMesh = fakeMesh;
            blockTM.fakeMaterial = fakeMaterial;
            blockTM.fakeColor = fakeColor;
            blockTM.numberMaterial = numberMaterial;
            blockTM.numberColor = numberColor;
            blockTM.mesh = mesh;
            blockTM.meshColor = color;
            blockTM.meshMaterial_default = meshMaterial_default;
            blockTM.meshMaterial_bloom = meshMaterial_bloom;
            BakeShapes();
            AddressableHelper.SetAddressable(blockTM, "TM_Block", "TM_Block", true);
            EditorUtility.SetDirty(blockTM);
            AssetDatabase.SaveAssets();
        }

    }

}
#endif