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

        [Header("Goal Info")]
        public int typeID;
        public string typeName;

        [Header("Pushable Goal")]
        public Sprite canPushMesh;
        public Material canPushMaterial;
        public UnityEngine.Color canPushColor;

        [Header("Goal Number")]
        public Material numberMaterial;
        public UnityEngine.Color numberColor;

        [Header("Goal Mesh")]
        public Sprite mesh;
        public UnityEngine.Color color;
        public Material meshMaterial;

        [Header("Goal Shapes")]
        public ShapeTM[] shapes;

        [Button("Load")]
        void Load() {
            typeID = goalTM.typeID;
            typeName = goalTM.typeName;
            canPushMesh = goalTM.canPushMesh;
            canPushMaterial = goalTM.canPushMaterial;
            canPushColor = goalTM.canPushColor;
            numberMaterial = goalTM.numberMaterial;
            numberColor = goalTM.numberColor;
            mesh = goalTM.mesh;
            color = goalTM.meshColor;
            meshMaterial = goalTM.meshMaterial;
            GetShapes();
            AddressableHelper.SetAddressable(goalTM, "TM_Goal", "TM_Goal", true);
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
            goalTM.canPushMesh = canPushMesh;
            goalTM.canPushMaterial = canPushMaterial;
            goalTM.canPushColor = canPushColor;
            goalTM.numberMaterial = numberMaterial;
            goalTM.numberColor = numberColor;
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