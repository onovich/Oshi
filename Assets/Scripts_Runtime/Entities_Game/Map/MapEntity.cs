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
        [SerializeField] Tilemap tilemap_terrain_wall;
        [SerializeField] Tilemap tilemap_terrain_spike;
        [SerializeField] Tilemap tilemap_terrain_goal;

        // Terrain
        public Dictionary<Vector2Int, int> terrainWallDict;
        public Dictionary<Vector2Int, int> terrainSpikeDict;
        public Dictionary<Vector2Int, int> terrainGoalDict;

        public void Ctor() {
            terrainWallDict = new Dictionary<Vector2Int, int>();
            terrainSpikeDict = new Dictionary<Vector2Int, int>();
            terrainGoalDict = new Dictionary<Vector2Int, int>();
        }

        public void Mesh_SetSize(Vector2Int size) {
            spr.size = size;
            spr.transform.position -= new Vector3(size.x / 2, size.y / 2, 0);
        }

        public void TearDown() {
            tilemap_terrain_wall.ClearAllTiles();
            tilemap_terrain_spike.ClearAllTiles();
            tilemap_terrain_goal.ClearAllTiles();
            terrainWallDict.Clear();
            Destroy(gameObject);
        }

        public void DecTimer(float dt) {
            gameTotalTime -= dt;
        }

        // Terrain
        public void Terrain_SetWall(Vector3Int pos, TileBase tile, int typeID) {
            tilemap_terrain_wall.SetTile(pos, tile);
            terrainWallDict.Add(new Vector2Int(pos.x, pos.y), typeID);
        }

        public bool Terrain_HasWall(Vector2Int pos) {
            return terrainWallDict.ContainsKey(pos);
        }

        public void Terrain_RemoveWall(Vector2Int pos) {
            tilemap_terrain_wall.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
            terrainWallDict.Remove(pos);
        }

        public void Terrain_SetSpike(Vector3Int pos, TileBase tile, int typeID) {
            tilemap_terrain_spike.SetTile(pos, tile);
            terrainSpikeDict.Add(new Vector2Int(pos.x, pos.y), typeID);
        }

        public bool Terrain_HasSpike(Vector2Int pos) {
            return terrainSpikeDict.ContainsKey(pos);
        }

        public void Terrain_RemoveSpike(Vector2Int pos) {
            tilemap_terrain_spike.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
            terrainSpikeDict.Remove(pos);
        }

        public void Terrain_SetGoal(Vector3Int pos, TileBase tile, int typeID) {
            tilemap_terrain_goal.SetTile(pos, tile);
            terrainGoalDict.Add(new Vector2Int(pos.x, pos.y), typeID);
        }

        public bool Terrain_HasGoal(Vector2Int pos) {
            return terrainGoalDict.ContainsKey(pos);
        }

        public void Terrain_RemoveGoal(Vector2Int pos) {
            tilemap_terrain_goal.SetTile(new Vector3Int(pos.x, pos.y, 0), null);
            terrainGoalDict.Remove(pos);
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