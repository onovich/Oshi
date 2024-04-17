using System;
using UnityEngine;

namespace Alter {

    public class WallEntity : MonoBehaviour {

        // Base Info
        public int entityIndex;
        public int typeID;
        public string typeName;

        // Render
        [SerializeField] public Transform body;
        [SerializeField] SpriteRenderer spr;

        // Size
        public Vector2 halfSize;
        public Vector2Int sizeInt;

        // Pos
        public Vector2 Pos => transform.position;
        public Vector2Int PosInt => Pos_GetPosInt();

        public void Ctor() {
        }

        // Pos
        public void Pos_SetPos(Vector2 pos) {
            transform.position = pos;
        }

        Vector2Int Pos_GetPosInt() {
            return transform.position.RoundToVector3Int().ToVector2Int();
        }

        // Size
        public void Size_SetSize(Vector2Int size) {
            spr.size = size;
            this.sizeInt = size;
            halfSize = size / 2;
        }

        // Mesh
        public void Mesh_Set(Sprite sp) {
            this.spr.sprite = sp;
        }

        // Rename
        public void Rename() {
            this.name = $"Wall - {typeID} - {entityIndex}";
        }

        public void TearDown() {
            Destroy(gameObject);
        }

    }

}