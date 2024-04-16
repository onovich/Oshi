using System;
using UnityEngine;

namespace Alter {

    [CreateAssetMenu(fileName = "SO_Enemy", menuName = "Alter/EnemyTM")]
    public class EnemyTM : ScriptableObject {

        public int typeID;
        public string typeName;
        
    }

}