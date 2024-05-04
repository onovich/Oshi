using System;
using UnityEngine;

namespace Oshi {

    public class BlockEntity : MonoBehaviour {

        // Base Info
        public int entityIndex;
        public int typeID;
        public string typeName;

        // Cell
        [SerializeField] Transform cellRoot;
        public CellSlotComponent cellSlotComponent;

        // Shape
        public ShapeComponent shapeComponent;
        public int shapeIndex;

        // Pos
        public Vector2 Pos => transform.position;
        public Vector2Int PosInt => Pos_GetPosInt();
        public Vector2 originalPos;

        // Render
        public Material meshMaterial_default;
        public Material meshMaterial_bloom;

        // VFX
        public string deadVFXName;
        public float deadVFXDuration;

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

        // Render
        public void Render_Bloom(Func<Vector2Int, bool> isGoal) {
            cellSlotComponent.ForEach((index, mod) => {
                if (isGoal(mod.LocalPosInt + PosInt)) {
                    mod.SetSprMaterial(meshMaterial_bloom);
                    return;
                }
                mod.SetSprMaterial(meshMaterial_default);
            });
        }

        // Push
        public bool Move_CheckConstraint(Vector2 constraintSize, Vector2 constraintCenter, Vector2 pos, Vector2 axis) {
            var offset = Vector2.zero;
            offset.x = 1 - constraintSize.x % 2;
            offset.y = 1 - constraintSize.y % 2;
            var min = constraintCenter - constraintSize / 2 + constraintCenter - offset;
            var max = constraintCenter + constraintSize / 2 + constraintCenter;
            if (pos.x + axis.x >= max.x || pos.x + axis.x <= min.x) {
                return false;
            }
            if (pos.y + axis.y >= max.y || pos.y + axis.y <= min.y) {
                return false;
            }
            return true;
        }

        public void TearDown() {
            cellSlotComponent.ForEach((index, mod) => {
                Destroy(mod.gameObject);
            });
            cellSlotComponent.Clear();
            Destroy(gameObject);
        }

    }

}