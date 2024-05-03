using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Terrain", menuName = "Oshi/TerrainTM")]
    public class TerrainTM : ScriptableObject {

        public int typeID;
        public string typeName;
        public TileBase tileBase;
        
    }

}