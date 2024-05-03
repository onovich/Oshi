using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Oshi {

    public class MapEntity : MonoBehaviour {

        // Base Info
        public int typeID;
        public string typeName;
        public Vector2Int mapSize;

        // Next Map
        public int nextMapTypeID;
        public bool isLastMap;

        // Role
        public Vector2 spawnPoint;

        // Limit
        public bool limitedByTime;
        public float gameTotalTime;

        public bool limitedByStep;
        public int gameTotalStep;

        // Pos
        public Vector2 Pos => transform.position;

        // Render
        [SerializeField] SpriteRenderer spr;
        [SerializeField] Tilemap tilemap;

        public void Ctor() {
        }

        public void Mesh_SetSize(Vector2Int size) {
            spr.size = size;
            spr.transform.position -= new Vector3(size.x / 2, size.y / 2, 0);
        }

        public void TearDown() {
            tilemap.ClearAllTiles();
            Destroy(gameObject);
        }

        public void DecTimer(float dt) {
            gameTotalTime -= dt;
        }

        public void Tilemap_SetTile(Vector3Int pos, TileBase tile) {
            tilemap.SetTile(pos, tile);
        }

#if UNITY_EDITOR
        public void DrawGizmos() {
            GUIStyle style = new GUIStyle();
            var color = Color.white;
            color.a = 0.5f;
            style.normal.textColor = color;
            style.fontSize = 4;
            Gizmos.color = color;

            for (int i = -mapSize.x / 2; i < mapSize.x / 2; i++) {
                for (int j = -mapSize.y / 2; j < mapSize.y / 2; j++) {
                    Vector3 _pos = new Vector3(i + 1f / 2, j + 1f / 2, 0);
                    Vector3 pos = new Vector3(i, j + 2, 0);
                    Gizmos.DrawWireCube(_pos, Vector3.one);
                    Handles.Label(pos, $"{i},{j}", style);
                }
            }
        }
#endif

    }

}