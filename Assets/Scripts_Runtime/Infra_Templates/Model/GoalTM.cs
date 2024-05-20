using System;
using UnityEngine;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Goal", menuName = "Oshi/GoalTM")]
    public class GoalTM : ScriptableObject {

        public int typeID;
        public Material canPushMaterial;
        public Sprite canPushMesh;
        public Color canPushColor;
        public Material numberMaterial;
        public Color numberColor;
        public string typeName;
        public Sprite mesh;
        public Color meshColor;
        public Material meshMaterial;
        public ShapeTM[] shapeArr;

    }

}