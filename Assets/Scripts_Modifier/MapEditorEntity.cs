#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Alter.Modifier {

    public class MapEditorEntity : MonoBehaviour {

        [SerializeField] int typeID;
        [SerializeField] GameObject mapSize;
        [SerializeField] MapTM mapTM;
        [SerializeField] Transform pointGroup;
        [SerializeField] Transform blockGroup;
        [SerializeField] Transform wallGroup;

        [Button("Bake")]
        void Bake() {
            BakeMapInfo();
            BakeSpawnPoint();
            BakeBlock();
            BakeWall();

            EditorUtility.SetDirty(mapTM);
            AssetDatabase.SaveAssets();
            Debug.Log("Bake Sucess");
        }

        void BakeMapInfo() {
            mapTM.typeID = typeID;
            mapTM.mapSize = mapSize.GetComponent<SpriteRenderer>().size.RoundToVector2Int();
            mapSize.GetComponent<SpriteRenderer>().size = mapTM.mapSize;
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
            var blockSizeArr = new List<Vector2Int>();
            var index = 0;
            foreach (var editor in editors) {
                blockTMArr.Add(editor.blockTM);
                blockPosArr.Add(editor.GetPosInt());
                blockIndexArr.Add(index++);
                blockSizeArr.Add(editor.GetSizeInt());
            }
            mapTM.blockTMArr = blockTMArr.ToArray();
            mapTM.blockPosArr = blockPosArr.ToArray();
            mapTM.blockIndexArr = blockIndexArr.ToArray();
            mapTM.blockSizeArr = blockSizeArr.ToArray();
        }

        void BakeWall() {
            var editors = wallGroup.GetComponentsInChildren<WallEditorEntity>();
            var wallTMArr = new List<WallTM>();
            var wallPosArr = new List<Vector2Int>();
            var wallIndexArr = new List<int>();
            var wallSizeArr = new List<Vector2Int>();
            var index = 0;
            foreach (var editor in editors) {
                wallTMArr.Add(editor.wallTM);
                wallPosArr.Add(editor.GetPosInt());
                wallIndexArr.Add(index++);
                wallSizeArr.Add(editor.GetSizeInt());
            }
            mapTM.wallTMArr = wallTMArr.ToArray();
            mapTM.wallPosArr = wallPosArr.ToArray();
            mapTM.wallIndexArr = wallIndexArr.ToArray();
            mapTM.wallSizeArr = wallSizeArr.ToArray();
        }

        void OnDrawGizmos() {
            Gizmos.color = Color.green;
        }

    }

}
#endif