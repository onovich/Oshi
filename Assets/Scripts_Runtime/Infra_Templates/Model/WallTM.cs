using System;
using UnityEngine;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Wall", menuName = "Oshi/WallTM")]
    public class WallTM : ScriptableObject {

        public int typeID;
        public string typeName;
        public Sprite mesh;
        public Color meshColor;
        public Material meshMaterial;
        public Color lineColor;
        public Material lineMaterial;
        public float lineWidth;
        public ShapeTM[] shapeArr;
        public Vector3[] shapeNodes;
        
    }

}