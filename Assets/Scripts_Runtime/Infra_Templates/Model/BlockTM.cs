using System;
using UnityEngine;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Block", menuName = "Oshi/BlockTM")]
    public class BlockTM : ScriptableObject {

        public int typeID;
        public bool isFake;
        public int number;
        public bool showNumber;
        public Material numberMaterial;
        public Color numberColor;
        public string typeName;
        public Sprite mesh;
        public Color meshColor;
        public Material meshMaterial_default;
        public Material meshMaterial_bloom;
        public GameObject deadVFX;
        public float deadVFXDuration;
        public ShapeTM[] shapeArr;

    }

}