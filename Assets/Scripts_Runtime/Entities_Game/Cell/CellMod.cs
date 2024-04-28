using System;
using UnityEngine;

namespace Oshi {

    public class CellMod : MonoBehaviour {

        public int index;
        [SerializeField] public SpriteRenderer spr;
        public Vector2Int LocalPosInt => Pos_GetLocalPosInt();

        Vector2Int Pos_GetLocalPosInt() {
            return transform.localPosition.RoundToVector3Int().ToVector2Int();
        }

    }

}