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

        [Button("Bake")]
        void Bake() {
            BakeMapInfo();
            BakeSpawnPoint();

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

        void OnDrawGizmos() {
            Gizmos.color = Color.green;
        }

    }

}
#endif