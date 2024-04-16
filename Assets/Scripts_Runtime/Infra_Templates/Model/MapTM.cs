using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Alter {

    [CreateAssetMenu(fileName = "SO_Map", menuName = "Alter/MapTM")]
    public class MapTM : ScriptableObject {

        public int typeID;

        public Vector2Int mapSize;
        public Vector2Int spawnPoint;

        // Camera
        public Vector2 cameraConfinerWorldMax;
        public Vector2 cameraConfinerWorldMin;

    }

}