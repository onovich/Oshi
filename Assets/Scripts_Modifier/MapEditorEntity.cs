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

        [Header("Role Config")]
        [SerializeField] RoleTM ownerRole;

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
            BakePath();
            BakeSpawnPoint();
            BakeBlock();
            BakeWall();
            BakeGoal();
            BakeSpike();

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
            mapTM.ownerRoleTypeID = ownerRole.typeID;
            mapTM.mapSize = mapSize.GetComponent<SpriteRenderer>().size.RoundToVector2Int();
            mapSize.GetComponent<SpriteRenderer>().size = mapTM.mapSize;
            mapSize.transform.position = -(mapTM.mapSize / 2).ToVector3Int();
        }

        void BakePath() {
            var editors = pathGroup.GetComponentsInChildren<PathEditorEntity>();
            if (editors.Length == 0) return;
            var pathSpawnTMArr = new List<PathSpawnTM>();
            var index = 0;
            foreach (var editor in editors) {
                index++;
                var pathSpawnTM = new PathSpawnTM();
                pathSpawnTM.pathNodeArr = editor.GetPathNodeArr();
                pathSpawnTMArr.Add(pathSpawnTM);
                editor.Rename(index, mapTM.typeName);
            }
        }

        void BakeSpawnPoint() {
            var editor = pointGroup.GetComponentInChildren<SpawnPointEditorEntity>();
            if (editor == null) {
                Debug.Log("SpawnPointEditor Not Found");
            }
            editor.Rename();
            mapTM.spawnPoint = editor.GetPos();
        }

        void BakeBlock() {
            var editors = blockGroup.GetComponentsInChildren<BlockEditorEntity>();
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

        void OnDrawGizmos() {
            Gizmos.color = Color.green;
        }

    }

}
#endif