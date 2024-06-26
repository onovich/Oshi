using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Map", menuName = "Oshi/MapTM")]
    public class MapTM : ScriptableObject {

        public int typeID;
        public string typeName;

        [Header("Next Map Config")]
        public int nextMapTypeID;
        public bool isLastMap;

        [Header("Weather Config")]
        public WeatherType weatherType;

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
        public int[] blockNumberArr;
        public bool[] blockIsFakeArr;

        [Header("Wall")]
        public int[] wallIndexArr;
        public WallTM[] wallTMArr;
        public Vector2Int[] wallPosArr;

        [Header("Goal")]
        public int[] goalIndexArr;
        public GoalTM[] goalTMArr;
        public Vector2Int[] goalPosArr;
        public int[] goalNumberArr;
        public bool[] goalCanPushArr;

        [Header("Spike")]
        public int[] spikeIndexArr;
        public SpikeTM[] spikeTMArr;
        public Vector2Int[] spikePosArr;

        [Header("Gate")]
        public int[] gateIndexArr;
        public GateTM[] gateTMArr;
        public Vector2Int[] gatePosArr;
        public int[] gateNextGateIndexArr;

        [Header("Path")]
        public int[] pathIndexArr;
        public PathTM[] pathTMArr;
        public PathSpawnTM[] pathSpawnTMArr;
        public EntityType[] pathTravelerTypeArr;
        public int[] pathTravelerIndexArr;
        public bool[] pathIsCircleLoopArr;
        public bool[] pathIsPingPongLoopArr;
        public Vector2[] pathTravelerHalfSizeArr;

        [Header("Terrain")]
        public Vector3Int[] terrainWallSpawnPosArr;
        public TerrainTM[] terrainWallTMArr;
        public Vector3Int[] terrainSpikeSpawnPosArr;
        public TerrainTM[] terrainSpikeTMArr;
        public Vector3Int[] terrainGoalSpawnPosArr;
        public TerrainTM[] terrainGoalTMArr;

        [Header("Camera")]
        public Vector2 cameraConfinerWorldMax;
        public Vector2 cameraConfinerWorldMin;

    }

}