using System;
using UnityEngine;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Gate", menuName = "Oshi/GateTM")]
    public class GateTM : ScriptableObject {

        public int typeID;
        public string typeName;
        public Sprite mesh;
        public Color meshColor;
        public Material meshMaterial;
        public ShapeTM[] shapeArr;

    }

}