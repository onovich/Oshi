using System;
using UnityEngine;

namespace Oshi {

    public class WallEntity : MonoBehaviour {

        // Base Info
        public int entityIndex;
        public int typeID;
        public string typeName;

        // Cell
        [SerializeField] Transform cellRoot;
        public CellSlotComponent cellSlotComponent;

        // Line
        [SerializeField] LineRenderer lineRenderer;

        // Shape
        public ShapeComponent shapeComponent;
        public int shapeIndex;

        // Pos
        public Vector2 Pos => transform.position;
        public Vector2Int PosInt => Pos_GetPosInt();

        public void Ctor() {
            cellSlotComponent = new CellSlotComponent();
            shapeComponent = new ShapeComponent();
            shapeIndex = 0;
        }

        // Pos
        public void Pos_SetPos(Vector2 pos) {
            transform.position = pos;
        }

        Vector2Int Pos_GetPosInt() {
            return transform.position.RoundToVector3Int().ToVector2Int();
        }

        // Line
        public void Line_SetPoints(Vector3[] points) {
            lineRenderer.useWorldSpace = false;
            lineRenderer.startWidth = 4f / 32;
            lineRenderer.endWidth = 4f / 32;
            lineRenderer.positionCount = points.Length;
            for (int i = 0; i < points.Length; i++) {
                lineRenderer.SetPosition(i, points[i]);
            }
            lineRenderer.loop = true;
        }

        public void TearDown() {
            Destroy(gameObject);
        }

    }

}