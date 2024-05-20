using System;
using UnityEngine;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Block", menuName = "Oshi/BlockTM")]
    public class BlockTM : ScriptableObject {

        public int typeID;
        public string typeName;
        public Material fakeMaterial;
        public Color fakeColor;
        public Material numberMaterial;
        public Color numberColor;
        public Sprite mesh;
        public Color meshColor;
        public Material meshMaterial_default;
        public Material meshMaterial_bloom;
        public ShapeTM[] shapeArr;

    }

}