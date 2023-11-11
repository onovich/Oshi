using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public static class GameBlockDomain {

        public static void RotateLeft(GameBusinessContext ctx, BlockEntity blockEntity) {

            var center = blockEntity.transform.position;

            blockEntity.BlockUnit_ForEach((item) => {
                var pos = item.position;
                var offset = pos - center;
                var newPos = center + new Vector3(offset.y, -offset.x, 0);
                item.localPosition = newPos;
            });

        }

    }

}