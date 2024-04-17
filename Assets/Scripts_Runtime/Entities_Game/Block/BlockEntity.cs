using System;
using UnityEngine;

namespace Alter {

    public class BlockEntity : MonoBehaviour {

        // Base Info
        public int entityIndex;
        public int typeID;
        public string typeName;

        // Render
        [SerializeField] public Transform body;
        [SerializeField] SpriteRenderer spr;

        // Pos
        public Vector2 Pos => transform.position;
        public Vector2Int PosInt => Pos_GetPosInt();

        // Size
        public Vector2 halfSize;
        public Vector2Int sizeInt;

        public void Ctor() {
        }

        // Pos
        public void Pos_SetPos(Vector2 pos) {
            transform.position = pos;
        }

        Vector2Int Pos_GetPosInt() {
            return transform.position.RoundToVector3Int().ToVector2Int();
        }

        // Push
        public bool Move_CheckMovable(Vector2 constarintSize, Vector2 axis, Vector2 contraintCenter) {
            var moveAxisX = axis.x;
            var moveAxisY = axis.y;
            if (moveAxisX == 0 && moveAxisY == 0) {
                return false;
            }

            var pos = transform.position;
            var min = contraintCenter - constarintSize / 2 + contraintCenter + halfSize;
            var max = contraintCenter + constarintSize / 2 + contraintCenter - halfSize;
            if (pos.x + moveAxisX >= max.x || pos.x + moveAxisX <= min.x) {
                return false;
            }
            if (pos.y + moveAxisY >= max.y || pos.y + moveAxisY <= min.y) {
                return false;
            }
            return true;
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
            this.name = $"Block - {typeID} - {entityIndex}";
        }

        public void TearDown() {
            Destroy(gameObject);
        }

    }

}