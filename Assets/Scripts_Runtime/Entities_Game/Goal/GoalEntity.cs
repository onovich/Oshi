using System;
using UnityEngine;

namespace Oshi {

    public class GoalEntity : MonoBehaviour {

        // Base Info
        public int entityIndex;
        public int typeID;
        public string typeName;

        // Render
        [SerializeField] SpriteRenderer spr;

        // Size
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
        }

        // Mesh
        public void Mesh_Set(Sprite sp) {
            this.spr.sprite = sp;
        }

        public void Mesh_SetMaterial(Material mat) {
            this.spr.material = mat;
        }

        public void TearDown() {
            Destroy(gameObject);
        }

    }

}