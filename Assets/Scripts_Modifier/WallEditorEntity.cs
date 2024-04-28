#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class WallEditorEntity : MonoBehaviour {

        [SerializeField] public WallTM wallTM;

        public Vector2Int GetPosInt() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public void Rename(int index) {
            gameObject.name = $"Wall - {wallTM.typeName} - {index}";
        }

        void OnDrawGizmos() {
            if (wallTM == null) return;
            if (wallTM.shapeArr == null) return;
            foreach (var shape in wallTM.shapeArr) {
                if (shape == null) continue;
                if (shape.cells == null) continue;
                var pos = GetPosInt();
                foreach (var cell in shape.cells) {
                    Gizmos.DrawGUITexture(new Rect(cell.x + pos.x, cell.y + pos.y, 1, 1), wallTM.mesh.texture, 0, 0, 0, 0);
                }
            }
        }

    }

}
#endif