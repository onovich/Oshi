using System;
using UnityEngine;

namespace Oshi {

    public class CellMod : MonoBehaviour {

        public int index;
        [SerializeField] SpriteRenderer spr;
        public Vector2Int LocalPosInt => Pos_GetLocalPosInt();

        public void Ctor() {

        }

        public void Pos_SetPosInt(Vector2Int pos) {
            transform.position = pos.ToVector3Int();
        }

        public void Pos_SetLocalPosInt(Vector2Int localPos) {
            transform.localPosition = localPos.ToVector3Int();
        }

        Vector2Int Pos_GetLocalPosInt() {
            return transform.localPosition.RoundToVector3Int().ToVector2Int();
        }

        public void SetParent(Transform parent) {
            transform.SetParent(parent, true);
        }

        public void SetSpr(Sprite spr) {
            this.spr.sprite = spr;
        }

        public void SetSprColor(Color color) {
            this.spr.color = color;
        }

        public void SetSprMaterial(Material material) {
            this.spr.material = material;
        }

    }

}