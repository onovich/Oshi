#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class SpikeEM : SerializedMonoBehaviour {

        [Header("Bake Target")]
        public SpikeTM spikeTM;

        [Header("Spike Info")]
        public int typeID;
        public string typeName;

        [Header("Spike Mesh")]
        public Sprite mesh;
        public UnityEngine.Color color;
        public Material meshMaterial;

        [Header("Spike Shapes")]
        public ShapeTM[] shapes;

        [Button("Load")]
        void Load() {
            typeID = spikeTM.typeID;
            typeName = spikeTM.typeName;
            mesh = spikeTM.mesh;
            color = spikeTM.meshColor;
            meshMaterial = spikeTM.meshMaterial;
            GetShapes();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        void GetShapes() {
            if (shapes == null) return;
            if (spikeTM == null) return;
            if (spikeTM.shapeArr == null) return;
            shapes = new ShapeTM[spikeTM.shapeArr.Length];
            for (int i = 0; i < spikeTM.shapeArr.Length; i++) {
                var shape = spikeTM.shapeArr[i];
                shapes[i] = shape;
            }
        }

        void BakeShapes() {
            var shapeList = new List<ShapeTM>();
            for (int i = 0; i < shapes.Length; i++) {
                var shape = shapes[i];
                shapeList.Add(shape);
            }
            spikeTM.shapeArr = shapeList.ToArray();
        }

        [Button("Bake")]
        void Bake() {
            spikeTM.typeID = typeID;
            spikeTM.typeName = typeName;
            spikeTM.mesh = mesh;
            spikeTM.meshColor = color;
            spikeTM.meshMaterial = meshMaterial;
            BakeShapes();
            AddressableHelper.SetAddressable(spikeTM, "TM_Spike", "TM_Spike", true);
            EditorUtility.SetDirty(spikeTM);
            AssetDatabase.SaveAssets();
        }

    }

}
#endif