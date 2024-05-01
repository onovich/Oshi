#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Oshi {

    public static class GizmosHelper {

      public static  void DrawCubeLine(Vector3 start, Vector3 end, float width, Color color) {
            Vector3 midPoint = (start + end) / 2;
            float lineLength = Vector3.Distance(start, end);
            Gizmos.color = color;
            Vector3 direction = (end - start).normalized;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, direction);
            Gizmos.matrix = Matrix4x4.TRS(midPoint, rotation, new Vector3(lineLength, width, width));
            Gizmos.DrawCube(Vector3.zero, Vector3.one); 
            Gizmos.matrix = Matrix4x4.identity;
        }

    }

}
#endif