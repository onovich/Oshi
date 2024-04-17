using System;
using UnityEngine;

namespace Alter {

    [CreateAssetMenu(fileName = "SO_Spike", menuName = "Alter/SpikeTM")]
    public class SpikeTM : ScriptableObject {

        public int typeID;
        public string typeName;
        public Sprite mesh;
        public Material meshMaterial;
        
    }

}