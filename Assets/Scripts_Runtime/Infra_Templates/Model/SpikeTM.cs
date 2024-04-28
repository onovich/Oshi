using System;
using UnityEngine;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Spike", menuName = "Oshi/SpikeTM")]
    public class SpikeTM : ScriptableObject {

        public int typeID;
        public string typeName;
        public Sprite mesh;
        public Color meshColor;
        public Material meshMaterial;
        public ShapeTM[] shapeArr;
        
    }

}