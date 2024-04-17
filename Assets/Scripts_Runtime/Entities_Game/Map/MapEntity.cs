using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Alter {

    public class MapEntity : MonoBehaviour {

        public int typeID;
        public Vector2Int mapSize;
        [SerializeField] SpriteRenderer spr;

        public Vector2 spawnPoint;

        public float timer;

        public void Ctor() {
            timer = 0;
        }

        public void SetSprSize(Vector2Int size) {
            spr.size = size;
        }

        public void IncTimer(float dt) {
            timer += dt;
        }

        public void TearDown() {
            Destroy(gameObject);
        }

    }

}