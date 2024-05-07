using System;
using UnityEngine;

namespace Oshi {

    public class GoalEntity : MonoBehaviour {

        // Base Info
        public int entityIndex;
        public int typeID;
        public string typeName;

        // Can Push
        public bool canPush;

        // Cell
        [SerializeField] Transform cellRoot;
        public CellSlotComponent cellSlotComponent;

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

        public void TearDown() {
            Destroy(gameObject);
        }

    }

}