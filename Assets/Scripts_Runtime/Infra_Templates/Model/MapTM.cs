using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Alter {

    [CreateAssetMenu(fileName = "SO_Map", menuName = "Alter/MapTM")]
    public class MapTM : ScriptableObject {

        public int typeID;

        public Vector2Int mapSize;
        public Vector2Int spawnPoint;

        // Block
        public int[] blockIndexArr;
        public BlockTM[] blockTMArr;
        public Vector2Int[] blockPosArr;
        public Vector2Int[] blockSizeArr;

        // Wall
        public int[] wallIndexArr;
        public WallTM[] wallTMArr;
        public Vector2Int[] wallPosArr;
        public Vector2Int[] wallSizeArr;

        // Camera
        public Vector2 cameraConfinerWorldMax;
        public Vector2 cameraConfinerWorldMin;

    }

}