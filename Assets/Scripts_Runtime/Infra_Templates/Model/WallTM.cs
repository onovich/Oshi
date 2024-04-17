using System;
using UnityEngine;

namespace Alter {

    [CreateAssetMenu(fileName = "SO_Wall", menuName = "Alter/WallTM")]
    public class WallTM : ScriptableObject {

        public int typeID;
        public string typeName;
        public Sprite mesh;
        public Material meshMaterial;
        
    }

}