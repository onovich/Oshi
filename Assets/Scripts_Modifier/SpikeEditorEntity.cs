#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class SpikeEditorEntity : MonoBehaviour {

        [SerializeField] public SpikeTM spikeTM;
        public EntityType type => EntityType.Spike;
        public int index;

        public Vector2Int GetPosInt() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public void Rename(int index) {
            this.index = index;
            gameObject.name = $"Spike - {spikeTM.typeName} - {index}";
        }

        void OnDrawGizmos() {
            if (spikeTM == null) return;
            if (spikeTM.shapeArr == null) return;
            foreach (var shape in spikeTM.shapeArr) {
                if (shape == null) continue;
                if (shape.cells == null) continue;
                var pos = GetPosInt();
                foreach (var cell in shape.cells) {
                    Gizmos.DrawGUITexture(new Rect(cell.x + pos.x, cell.y + pos.y, 1, 1), spikeTM.mesh.texture, 0, 0, 0, 0);
                }
            }
        }

    }

}
#endif