using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Map", menuName = "Oshi/MapTM")]
    public class MapTM : ScriptableObject {

        public int typeID;

        public Vector2Int mapSize;
        public Vector2Int spawnPoint;

        // Block
        public int[] blockIndexArr;
        public BlockTM[] blockTMArr;
        public Vector2Int[] blockPosArr;

        // Wall
        public int[] wallIndexArr;
        public WallTM[] wallTMArr;
        public Vector2Int[] wallPosArr;

        // Goal
        public int[] goalIndexArr;
        public GoalTM[] goalTMArr;
        public Vector2Int[] goalPosArr;

        // Spike
        public int[] spikeIndexArr;
        public SpikeTM[] spikeTMArr;
        public Vector2Int[] spikePosArr;

        // Camera
        public Vector2 cameraConfinerWorldMax;
        public Vector2 cameraConfinerWorldMin;

    }

}