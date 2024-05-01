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
                GizmosHelper.DrawCubeLine(node, next, lineWidth, lineColor);
                Gizmos.color = UnityEngine.Color.green;
                Gizmos.DrawCube(node, Vector3.one * 0.1f);
            }
        }

    }

}
#endif