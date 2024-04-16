using System;
using UnityEngine;

namespace Alter {

    [CreateAssetMenu(fileName = "SO_Role", menuName = "Alter/RoleTM")]
    public class RoleTM : ScriptableObject {

        public int typeID;
        public string typeName;
        public AllyStatus allyStatus;
        public float moveSpeed;
        public int hpMax;
        public RoleMod mod;
        public GameObject deadVFX;
        public float deadVFXDuration;
        
    }

}