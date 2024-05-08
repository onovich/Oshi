using System;
using UnityEngine;

namespace Oshi {

    public static class GridUtils_Constraint {

        public static bool CheckConstraint(Vector2 constraintSize, Vector2 constraintCenter, Vector2 pos, Vector2 axis) {
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

    }

}