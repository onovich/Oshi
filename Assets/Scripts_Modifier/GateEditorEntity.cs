#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class GateEditorEntity : MonoBehaviour {

        [SerializeField] public GateTM gateTM;
        [SerializeField] public GateEditorEntity nextGate;
        public EntityType type => EntityType.Gate;
        public int index;

        public Vector2Int GetPosInt() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public void Rename(int index) {
            this.index = index;
            gameObject.name = $"Gate - {gateTM.typeName} - {index}";
        }

        public int GetNextGateIndex() {
            if (nextGate == null) return -1;
            return nextGate.index;
        }

        void OnDrawGizmos() {
            if (gateTM == null) return;
            if (gateTM.shapeArr == null) return;
            foreach (var shape in gateTM.shapeArr) {
                if (shape == null) continue;
                if (shape.cells == null) continue;
                var pos = GetPosInt();
                foreach (var cell in shape.cells) {
                    Gizmos.DrawGUITexture(new Rect(cell.x + pos.x, cell.y + pos.y, 1, 1), gateTM.mesh.texture, 0, 0, 0, 0);
                }
            }
        }

        public void OnDrawGhost(Vector3 offset) {
            if (gateTM == null) return;
            if (gateTM.shapeArr == null) return;
            foreach (var shape in gateTM.shapeArr) {
                if (shape == null) continue;
                if (shape.cells == null) continue;
                var pos = GetPosInt().ToVector3Int() + offset + new Vector3(.5f, .5f, 0f);
                foreach (var cell in shape.cells) {
                    Gizmos.color = new Color(1, 1, 1, 0.5f);
                    Gizmos.DrawCube(new Vector3(cell.x + pos.x, cell.y + pos.y, 0), Vector3.one);
                }
            }
        }

    }

}
#endif