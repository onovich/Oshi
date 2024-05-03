#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Oshi.Modifier {

    public class MapEditorEntity : MonoBehaviour {

        [Header("Map Info")]
        [SerializeField] int typeID;
        [SerializeField] string typeName;
        [SerializeField] GameObject mapSize;
        [SerializeField] MapTM mapTM;

        [Header("Time Config")]
        [SerializeField] bool limitedByTime;
        [SerializeField] float gameTotalTime;
        [SerializeField] bool limitedByStep;
        [SerializeField] int gameTotalStep;

        [Header("Terrain")]
        [SerializeField] Tilemap tilemap_terrain_wall;
        [SerializeField] Tilemap tilemap_terrain_spike;
        [SerializeField] Tilemap tilemap_terrain_goal;
        List<TerrainTM> terrainTMArr;

        [Header("Entity Group")]
        [SerializeField] Transform pointGroup;
        [SerializeField] Transform blockGroup;
        [SerializeField] Transform wallGroup;
        [SerializeField] Transform goalGroup;
        [SerializeField] Transform spikeGroup;
        [SerializeField] Transform pathGroup;

        [Button("Bake")]
        void Bake() {
            BakeMapInfo();
            BakeSpawnPoint();
            GetAllTerrainTM();
            BakeTerrain_Wall();
            BakeTerrain_Spike();
            BakeTerrain_Goal();
            BakeBlock();
            BakeWall();
            BakeGoal();
            BakeSpike();
            BakePath();

            EditorUtility.SetDirty(mapTM);
            AssetDatabase.SaveAssets();
            Debug.Log("Bake Sucess");
        }

        void BakeMapInfo() {
            mapTM.typeID = typeID;
            mapTM.typeName = typeName;
            mapTM.limitedByTime = limitedByTime;
            mapTM.gameTotalTime = gameTotalTime;
            mapTM.limitedByStep = limitedByStep;
            mapTM.gameTotalStep = gameTotalStep;
            mapTM.mapSize = mapSize.GetComponent<SpriteRenderer>().size.RoundToVector2Int();
            mapSize.GetComponent<SpriteRenderer>().size = mapTM.mapSize;
            mapSize.transform.position = -(mapTM.mapSize / 2).ToVector3Int();
        }

        void BakeTerrain_Wall() {
            var terrainSpawnPosList = new List<Vector3Int>();
            var terrainList = new List<TerrainTM>();
            for (int x = tilemap_terrain_wall.cellBounds.x; x < tilemap_terrain_wall.cellBounds.xMax; x++) {
                for (int y = tilemap_terrain_wall.cellBounds.y; y < tilemap_terrain_wall.cellBounds.yMax; y++) {
                    var pos = new Vector3Int(x, y, 0);
                    var tile = tilemap_terrain_wall.GetTile(pos);
                    if (tile == null) continue;
                    terrainSpawnPosList.Add(pos);
                    var terrainTM = GetTerrainTM(tile);
                    if (terrainTM == null) {
                        Debug.Log("TerrainTM Not Found");
                        continue;
                    }
                    terrainList.Add(terrainTM);
                }
            }
            mapTM.terrainWallSpawnPosArr = terrainSpawnPosList.ToArray();
            mapTM.terrainWallTMArr = terrainList.ToArray();
        }

        void BakeTerrain_Spike() {
            var terrainSpawnPosList = new List<Vector3Int>();
            var terrainList = new List<TerrainTM>();
            for (int x = tilemap_terrain_spike.cellBounds.x; x < tilemap_terrain_spike.cellBounds.xMax; x++) {
                for (int y = tilemap_terrain_spike.cellBounds.y; y < tilemap_terrain_spike.cellBounds.yMax; y++) {
                    var pos = new Vector3Int(x, y, 0);
                    var tile = tilemap_terrain_spike.GetTile(pos);
                    if (tile == null) continue;
                    terrainSpawnPosList.Add(pos);
                    var terrainTM = GetTerrainTM(tile);
                    if (terrainTM == null) {
                        Debug.Log("TerrainTM Not Found");
                        continue;
                    }
                    terrainList.Add(terrainTM);
                }
            }
            mapTM.terrainSpikeSpawnPosArr = terrainSpawnPosList.ToArray();
            mapTM.terrainSpikeTMArr = terrainList.ToArray();
        }

        void BakeTerrain_Goal() {
            var terrainSpawnPosList = new List<Vector3Int>();
            var terrainList = new List<TerrainTM>();
            for (int x = tilemap_terrain_goal.cellBounds.x; x < tilemap_terrain_goal.cellBounds.xMax; x++) {
                for (int y = tilemap_terrain_goal.cellBounds.y; y < tilemap_terrain_goal.cellBounds.yMax; y++) {
                    var pos = new Vector3Int(x, y, 0);
                    var tile = tilemap_terrain_goal.GetTile(pos);
                    if (tile == null) continue;
                    terrainSpawnPosList.Add(pos);
                    var terrainTM = GetTerrainTM(tile);
                    if (terrainTM == null) {
                        Debug.Log("TerrainTM Not Found");
                        continue;
                    }
                    terrainList.Add(terrainTM);
                }
            }
            mapTM.terrainGoalSpawnPosArr = terrainSpawnPosList.ToArray();
            mapTM.terrainGoalTMArr = terrainList.ToArray();
        }

        void BakePath() {
            var editors = pathGroup.GetComponentsInChildren<PathEditorEntity>();
            if (editors == null || editors.Length == 0) {
                mapTM.pathSpawnTMArr = null;
                mapTM.pathTravelerTypeArr = null;
                mapTM.pathTravelerIndexArr = null;
                mapTM.pathIndexArr = null;
                mapTM.pathTMArr = null;
                mapTM.pathIsCircleLoopArr = null;
                mapTM.pathIsPingPongLoopArr = null;
                mapTM.pathTravelerHalfSizeArr = null;
                return;
            }
            var pathTMArr = new List<PathTM>();
            var pathSpawnTMArr = new List<PathSpawnTM>();
            var pathTravelerTypeArr = new List<EntityType>();
            var pathTravelerIndexArr = new List<int>();
            var pathIndexArr = new List<int>();
            var pathIsCircleLoopArr = new List<bool>();
            var pathIsPingPongLoopArr = new List<bool>();
            var pathTravelerHalfSizeArr = new List<Vector2>();
            var index = 0;
            foreach (var editor in editors) {
                index++;
                var pathSpawnTM = new PathSpawnTM();
                pathSpawnTM.pathNodeArr = editor.GetPathNodeArr();
                pathSpawnTMArr.Add(pathSpawnTM);
                editor.Rename(index);
                var travelerType = editor.GetTravelerType();
                var travlerIndex = editor.GetTravlerIndex();
                pathTravelerTypeArr.Add(travelerType);
                pathTravelerIndexArr.Add(travlerIndex);
                pathIndexArr.Add(index);
                pathTMArr.Add(editor.pathTM);
                pathIsCircleLoopArr.Add(editor.isCircleLoop);
                pathIsPingPongLoopArr.Add(editor.isPingPongLoop);
                pathTravelerHalfSizeArr.Add(editor.GetTravelerSize(editor.traveler) / 2);
            }
            mapTM.pathSpawnTMArr = pathSpawnTMArr.ToArray();
            mapTM.pathTravelerTypeArr = pathTravelerTypeArr.ToArray();
            mapTM.pathTravelerIndexArr = pathTravelerIndexArr.ToArray();
            mapTM.pathIndexArr = pathIndexArr.ToArray();
            mapTM.pathTMArr = pathTMArr.ToArray();
            mapTM.pathIsCircleLoopArr = pathIsCircleLoopArr.ToArray();
            mapTM.pathIsPingPongLoopArr = pathIsPingPongLoopArr.ToArray();
            mapTM.pathTravelerHalfSizeArr = pathTravelerHalfSizeArr.ToArray();
        }

        void BakeSpawnPoint() {
            var editor = pointGroup.GetComponentInChildren<SpawnPointEditorEntity>();
            if (editor == null) {
                Debug.Log("SpawnPointEditor Not Found");
            }
            editor.Rename();
            mapTM.ownerRoleTypeID = editor.roleTM.typeID;
            mapTM.spawnPoint = editor.GetPosInt();
        }

        void BakeBlock() {
            var editors = blockGroup.GetComponentsInChildren<BlockEditorEntity>();
            if (editors == null || editors.Length == 0) {
                mapTM.blockTMArr = null;
                mapTM.blockPosArr = null;
                mapTM.blockIndexArr = null;
                return;
            }
            var blockTMArr = new List<BlockTM>();
            var blockPosArr = new List<Vector2Int>();
            var blockIndexArr = new List<int>();
            var index = 0;
            foreach (var editor in editors) {
                index += 1;
                blockTMArr.Add(editor.blockTM);
                blockPosArr.Add(editor.GetPosInt());
                blockIndexArr.Add(index);
                editor.Rename(index);
            }
            mapTM.blockTMArr = blockTMArr.ToArray();
            mapTM.blockPosArr = blockPosArr.ToArray();
            mapTM.blockIndexArr = blockIndexArr.ToArray();
        }

        void BakeWall() {
            var editors = wallGroup.GetComponentsInChildren<WallEditorEntity>();
            if (editors == null || editors.Length == 0) {
                mapTM.wallTMArr = null;
                mapTM.wallPosArr = null;
                mapTM.wallIndexArr = null;
                return;
            }
            var wallTMArr = new List<WallTM>();
            var wallPosArr = new List<Vector2Int>();
            var wallIndexArr = new List<int>();
            var index = 0;
            foreach (var editor in editors) {
                index += 1;
                wallTMArr.Add(editor.wallTM);
                wallPosArr.Add(editor.GetPosInt());
                wallIndexArr.Add(index);
                editor.Rename(index);
            }
            mapTM.wallTMArr = wallTMArr.ToArray();
            mapTM.wallPosArr = wallPosArr.ToArray();
            mapTM.wallIndexArr = wallIndexArr.ToArray();
        }

        void BakeGoal() {
            var editors = goalGroup.GetComponentsInChildren<GoalEditorEntity>();
            var goalTMArr = new List<GoalTM>();
            var goalPosArr = new List<Vector2Int>();
            var goalIndexArr = new List<int>();
            var index = 0;
            foreach (var editor in editors) {
                index += 1;
                goalTMArr.Add(editor.goalTM);
                goalPosArr.Add(editor.GetPosInt());
                goalIndexArr.Add(index);
                editor.Rename(index);
            }
            mapTM.goalTMArr = goalTMArr.ToArray();
            mapTM.goalPosArr = goalPosArr.ToArray();
            mapTM.goalIndexArr = goalIndexArr.ToArray();
        }

        void BakeSpike() {
            var editors = spikeGroup.GetComponentsInChildren<SpikeEditorEntity>();
            if (editors == null || editors.Length == 0) {
                mapTM.spikeTMArr = null;
                mapTM.spikePosArr = null;
                mapTM.spikeIndexArr = null;
                return;
            }
            var spikeTMArr = new List<SpikeTM>();
            var spikePosArr = new List<Vector2Int>();
            var spikeIndexArr = new List<int>();
            var index = 0;
            foreach (var editor in editors) {
                index += 1;
                spikeTMArr.Add(editor.spikeTM);
                spikePosArr.Add(editor.GetPosInt());
                spikeIndexArr.Add(index);
                editor.Rename(index);
            }
            mapTM.spikeTMArr = spikeTMArr.ToArray();
            mapTM.spikePosArr = spikePosArr.ToArray();
            mapTM.spikeIndexArr = spikeIndexArr.ToArray();
        }


        TerrainTM GetTerrainTM(TileBase tileBase) {
            foreach (var terrainTM in terrainTMArr) {
                if (terrainTM.tileBase == tileBase) {
                    return terrainTM;
                }
            }
            return null;
        }

        void GetAllTerrainTM() {
            terrainTMArr = FieldHelper.GetAllInstances<TerrainTM>();
        }

        void OnDrawGizmos() {
        }

    }

}
#endif