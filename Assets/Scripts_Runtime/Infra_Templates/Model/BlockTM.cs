using System;
using UnityEngine;

namespace Alter {

    [CreateAssetMenu(fileName = "SO_Block", menuName = "Alter/BlockTM")]
    public class BlockTM : ScriptableObject {

        public int typeID;
        public string typeName;
        public Sprite mesh;
        public Material meshMaterial;
        
    }

}