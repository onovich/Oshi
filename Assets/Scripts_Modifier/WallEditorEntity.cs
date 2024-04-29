#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class WallEditorEntity : MonoBehaviour {

        [SerializeField] public WallTM wallTM;
        public EntityType type => EntityType.Wall;
        public int index;

        public Vector2Int GetPosInt() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public void Rename(int index) {
            this.index = index;
            gameObject.name = $"Wall - {wallTM.typeName} - {index}";
        }

        void OnDrawGizmos() {
            if (wallTM == null) return;
            if (wallTM.shapeArr == null) return;
            if (wallTM.mesh != null) {
                foreach (var shape in wallTM.shapeArr) {
                    if (shape == null) continue;
                    if (shape.cells == null) continue;
                    var pos = GetPosInt();
                    foreach (var cell in shape.cells) {
                        Gizmos.DrawGUITexture(new Rect(cell.x + pos.x, cell.y + pos.y, 1, 1), wallTM.mesh.texture, 0, 0, 0, 0);
                    }
                }
            }
            for (int i = 0; i < wallTM.shapeNodes.Length; i++) {
                Vector3 node = wallTM.shapeNodes[i] + transform.position;
                var next = wallTM.shapeNodes[(i + 1) % wallTM.shapeNodes.Length] + transform.position;
                Gizmos.color = Color.green;
                Gizmos.DrawCube(node, Vector3.one * 0.1f);
                Gizmos.color = Color.white;
                Gizmos.DrawLine(node, next);
            }
        }

        public void OnDrawGhost(Vector2Int offset) {
            if (wallTM == null) return;
            if (wallTM.shapeArr == null) return;
            foreach (var shape in wallTM.shapeArr) {
                if (shape == null) continue;
                if (shape.cells == null) continue;
                var pos = GetPosInt() + offset + new Vector2(.5f, .5f);
                foreach (var cell in shape.cells) {
                    Gizmos.color = new Color(1, 1, 1, 0.5f);
                    Gizmos.DrawCube(new Vector3(cell.x + pos.x, cell.y + pos.y, 0), Vector3.one);
                }
            }
        }

    }

}
#endif