using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Oshi {

    public class RecordModel {

        public int stepIndex;
        public Vector2Int ownerPos;
        public Vector2Int[] blockPosArr;
        public Vector2Int[] gatePosArr;
        public Vector2Int[] goalPosArr;
        public Vector2Int[] spikePosArr;

    }

}