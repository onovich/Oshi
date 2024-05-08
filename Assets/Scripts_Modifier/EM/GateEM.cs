#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class GateEM : SerializedMonoBehaviour {

        [Header("Bake Target")]
        public GateTM gateTM;

        [Header("Gate Info")]
        public int typeID;
        public string typeName;

        [Header("Can Push")]
        public bool canPush;

        [Header("Gate Number")]
        public int number;
        public bool showNumber;
        public Material numberMaterial;
        public UnityEngine.Color numberColor;

        [Header("Gate Mesh")]
        public Sprite mesh;
        public UnityEngine.Color color;
        public Material meshMaterial;

        [Header("Gate Shapes")]
        public ShapeTM[] shapes;

        [Button("Load")]
        void Load() {
            typeID = gateTM.typeID;
            typeName = gateTM.typeName;
            canPush = gateTM.canPush;
            number = gateTM.number;
            showNumber = gateTM.showNumber;
            numberMaterial = gateTM.numberMaterial;
            numberColor = gateTM.numberColor;
            mesh = gateTM.mesh;
            color = gateTM.meshColor;
            meshMaterial = gateTM.meshMaterial;
            GetShapes();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        void GetShapes() {
            if (shapes == null) return;
            if (gateTM == null) return;
            if (gateTM.shapeArr == null) return;
            shapes = new ShapeTM[gateTM.shapeArr.Length];
            for (int i = 0; i < gateTM.shapeArr.Length; i++) {
                var shape = gateTM.shapeArr[i];
                shapes[i] = shape;
            }
        }

        void BakeShapes() {
            var shapeList = new List<ShapeTM>();
            for (int i = 0; i < shapes.Length; i++) {
                var shape = shapes[i];
                shapeList.Add(shape);
            }
            gateTM.shapeArr = shapeList.ToArray();
        }

        [Button("Bake")]
        void Bake() {
            gateTM.typeID = typeID;
            gateTM.typeName = typeName;
            gateTM.canPush = canPush;
            gateTM.number = number;
            gateTM.showNumber = showNumber;
            gateTM.numberMaterial = numberMaterial;
            gateTM.numberColor = numberColor;
            gateTM.mesh = mesh;
            gateTM.meshColor = color;
            gateTM.meshMaterial = meshMaterial;
            BakeShapes();
            EditorUtility.SetDirty(gateTM);
            AssetDatabase.SaveAssets();
        }

    }

}
#endif