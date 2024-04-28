#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class BlockEditorEntity : MonoBehaviour {

        [SerializeField] public BlockTM blockTM;

        public Vector2Int GetPosInt() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public void Rename(int index) {
            gameObject.name = $"Block - {blockTM.typeName} - {index}";
        }

        void OnDrawGizmos() {
            if (blockTM == null) return;
            if (blockTM.shapeArr == null) return;
            foreach (var shape in blockTM.shapeArr) {
                if (shape == null) continue;
                if (shape.cells == null) continue;
                var pos = GetPosInt();
                foreach (var cell in shape.cells) {
                    Gizmos.color = Color.white;
                    Gizmos.DrawGUITexture(new Rect(cell.x + pos.x, cell.y + pos.y, 1, 1), blockTM.mesh.texture, 0, 0, 1, 1);
                }
            }
        }

    }

}
#endif