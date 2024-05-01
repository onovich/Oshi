#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Oshi {

    public class WallEM : SerializedMonoBehaviour {

        [Header("Bake Target")]
        public WallTM wallTM;

        [Header("Block Info")]
        public int typeID;
        public string typeName;

        [Header("Block Mesh")]
        public Sprite mesh;
        public UnityEngine.Color meshColor;
        public Material meshMaterial;

        [Header("Block Line")]
        public UnityEngine.Color lineColor;
        public Material lineMaterial;
        public float lineWidth;

        [Header("Block Shapes")]
        public ShapeTM[] shapes;

        [Button("Load")]
        void Load() {
            typeID = wallTM.typeID;
            typeName = wallTM.typeName;
            mesh = wallTM.mesh;
            meshColor = wallTM.meshColor;
            meshMaterial = wallTM.meshMaterial;
            lineColor = wallTM.lineColor;
            lineMaterial = wallTM.lineMaterial;
            lineWidth = wallTM.lineWidth;
            GetShapes();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        void GetShapes() {
            if (shapes == null) return;
            if (wallTM == null) return;
            if (wallTM.shapeArr == null) return;
            shapes = new ShapeTM[wallTM.shapeArr.Length];
            for (int i = 0; i < wallTM.shapeArr.Length; i++) {
                var shape = wallTM.shapeArr[i];
                shapes[i] = shape;
            }
        }

        void BakeShapes() {
            var shapeList = new List<ShapeTM>();
            for (int i = 0; i < shapes.Length; i++) {
                var shape = shapes[i];
                shapeList.Add(shape);
            }
            wallTM.shapeArr = shapeList.ToArray();
        }

        void BakeShapeNodes() {
            var nodeList = new List<Vector2>();
            for (int i = 0; i < shapes.Length; i++) {
                var shape = shapes[i];
                for (int j = 0; j < shape.cells.Length; j++) {
                    var lb = new Vector2(shape.cells[j].x, shape.cells[j].y);
                    var lt = new Vector2(lb.x, lb.y + 1);
                    var rt = new Vector2(lb.x + 1, lb.y + 1);
                    var rb = new Vector2(lb.x + 1, lb.y);
                    nodeList.Add(lb);
                    nodeList.Add(lt);
                    nodeList.Add(rt);
                    nodeList.Add(rb);
                }
            }
            var node3List = Vector2SortingHelper.Sort(nodeList);
            wallTM.shapeNodes = node3List.ToArray();
        }

        [Button("Bake")]
        void Bake() {
            wallTM.typeID = typeID;
            wallTM.typeName = typeName;
            wallTM.mesh = mesh;
            wallTM.meshColor = meshColor;
            wallTM.meshMaterial = meshMaterial;
            wallTM.lineColor = lineColor;
            wallTM.lineMaterial = lineMaterial;
            wallTM.lineWidth = lineWidth;
            BakeShapes();
            BakeShapeNodes();
            EditorUtility.SetDirty(wallTM);
            AssetDatabase.SaveAssets();
        }

        void OnDrawGizmos() {
            if (wallTM.shapeNodes == null) return;
            if (wallTM.shapeNodes.Length == 0) return;
            for (int i = 0; i < wallTM.shapeNodes.Length; i++) {
                var node = wallTM.shapeNodes[i];
                var next = wallTM.shapeNodes[(i + 1) % wallTM.shapeNodes.Length];
                Gizmos.color = lineColor;
                // Gizmos.DrawLine(node, next);
                DrawCubeLine(node, next, lineWidth);
                Gizmos.color = UnityEngine.Color.green;
                Gizmos.DrawCube(node, Vector3.one * 0.1f);
            }
        }

        void DrawCubeLine(Vector3 start, Vector3 end, float width) {
            // 计算线段的中点
            Vector3 midPoint = (start + end) / 2;

            // 计算线段的长度
            float lineLength = Vector3.Distance(start, end);

            // 设置Gizmos颜色
            Gizmos.color = lineColor;

            // 计算线段的方向（从起点指向终点）
            Vector3 direction = (end - start).normalized;

            // 计算旋转角度，需要将线段的方向从默认的(1,0,0)或Z轴旋转到正确的方向
            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, direction);

            // 设置Gizmos的变换矩阵以便正确放置和旋转立方体
            Gizmos.matrix = Matrix4x4.TRS(midPoint, rotation, new Vector3(lineLength, width, width));

            // 绘制立方体，使其长度等于点之间的距离，宽度和高度由传入的 width 决定
            Gizmos.DrawCube(Vector3.zero, Vector3.one);  // 在局部坐标中绘制立方体

            // 重置Gizmos矩阵
            Gizmos.matrix = Matrix4x4.identity;
        }

    }

}
#endif