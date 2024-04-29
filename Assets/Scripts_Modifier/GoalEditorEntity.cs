#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class GoalEditorEntity : MonoBehaviour {

        [SerializeField] public GoalTM goalTM;
        public EntityType type => EntityType.Goal;
        public int index;

        public Vector2Int GetPosInt() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public void Rename(int index) {
            this.index = index;
            gameObject.name = $"Goal - {goalTM.typeName} - {index}";
        }

        void OnDrawGizmos() {
            if (goalTM == null) return;
            if (goalTM.shapeArr == null) return;
            foreach (var shape in goalTM.shapeArr) {
                if (shape == null) continue;
                if (shape.cells == null) continue;
                var pos = GetPosInt();
                foreach (var cell in shape.cells) {
                    Gizmos.DrawGUITexture(new Rect(cell.x + pos.x, cell.y + pos.y, 1, 1), goalTM.mesh.texture, 0, 0, 0, 0);
                }
            }
        }

        public void OnDrawGhost(Vector2Int offset) {
            if (goalTM == null) return;
            if (goalTM.shapeArr == null) return;
            foreach (var shape in goalTM.shapeArr) {
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