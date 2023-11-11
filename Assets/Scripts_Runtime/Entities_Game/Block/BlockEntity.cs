using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public class BlockEntity : MonoBehaviour {

        public int typeID;
        public int entityID;
        BlockUnitSlotComponent blockUnitSlotComponent;
        public void BlockUnit_Add(Transform transform) {
            blockUnitSlotComponent.Add(transform);
        }

        public List<Transform> BlockUnit_GetAll() {
            return blockUnitSlotComponent.GetAll();
        }

        public void BlockUnit_Clear() {
            blockUnitSlotComponent.Clear();
        }

        public void BlockUnit_ForEach(System.Action<Transform> action) {
            blockUnitSlotComponent.ForEach(action);
        }

        public void Pos_SetPos(Vector2Int pos) {
            transform.position = pos.ToVector3();
        }

        public void Ctor() {

        }

    }

}