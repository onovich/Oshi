using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Oshi {

    public class MapEntity : MonoBehaviour {

        public int typeID;
        public string typeName;
        public Vector2Int mapSize;
        [SerializeField] SpriteRenderer spr;

        public Vector2 spawnPoint;
        public Vector2 Pos => transform.position;

        public bool limitedByTime;
        public float gameTotalTime;

        public bool limitedByStep;
        public int gameTotalStep;

        public void Ctor() {
        }

        public void Mesh_SetSize(Vector2Int size) {
            spr.size = size;
            spr.transform.position -= new Vector3(size.x / 2, size.y / 2, 0);
        }

        public void TearDown() {
            Destroy(gameObject);
        }

        public void DecTimer(float dt) {
            gameTotalTime -= dt;
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