using System;
using UnityEngine;

namespace Oshi {

    public class BlockEntity : MonoBehaviour {

        // Base Info
        public int entityIndex;
        public int typeID;
        public string typeName;

        // Is Fake
        public bool isFake;

        // Block Number
        public int number;
        public bool showNumber;
        public Material numberMaterial;
        public Color numberColor;

        // Cell
        [SerializeField] public Transform cellRoot;
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

        public void TearDown() {
            cellSlotComponent.ForEach((index, mod) => {
                Destroy(mod.gameObject);
            });
            cellSlotComponent.Clear();
            Destroy(gameObject);
        }

    }

}