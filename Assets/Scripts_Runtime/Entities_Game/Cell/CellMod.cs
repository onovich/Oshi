using System;
using UnityEngine;
using TMPro;

namespace Oshi {

    public class CellMod : MonoBehaviour {

        public int index;
        [SerializeField] SpriteRenderer spr;
        [SerializeField] TextMeshProUGUI textMesh;
        [SerializeField] Canvas canvas;

        public Vector2Int LocalPosInt => Pos_GetLocalPosInt();

        public void Ctor() {

        }

        public void SetNumberMaterial(Material material) {
            textMesh.material = material;
        }

        public void SetNumberColor(Color color) {
            textMesh.color = color;
        }

        public void SetNumber(int number) {
            textMesh.text = number.ToString();
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

        public void SetSortingLayer(string sortingLayerName) {
            spr.sortingLayerName = sortingLayerName;
            if (canvas != null) {
                canvas.sortingLayerName = sortingLayerName;
            }
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