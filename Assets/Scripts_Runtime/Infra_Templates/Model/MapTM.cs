using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Map", menuName = "Oshi/MapTM")]
    public class MapTM : ScriptableObject {

        public int typeID;
        public string typeName;

        [Header("Time Config")]
        public bool limitedByTime;
        public float gameTotalTime;
        public bool limitedByStep;
        public int gameTotalStep;

        [Header("Role Config")]
        public int ownerRoleTypeID;

        [Header("Map Config")]
        public Vector2Int mapSize;
        public Vector2Int spawnPoint;

        [Header("Block")]
        public int[] blockIndexArr;
        public BlockTM[] blockTMArr;
        public Vector2Int[] blockPosArr;

        [Header("Wall")]
        public int[] wallIndexArr;
        public WallTM[] wallTMArr;
        public Vector2Int[] wallPosArr;

        [Header("Goal")]
        public int[] goalIndexArr;
        public GoalTM[] goalTMArr;
        public Vector2Int[] goalPosArr;

        [Header("Spike")]
        public int[] spikeIndexArr;
        public SpikeTM[] spikeTMArr;
        public Vector2Int[] spikePosArr;

        [Header("Camera")]
        public Vector2 cameraConfinerWorldMax;
        public Vector2 cameraConfinerWorldMin;

    }

}