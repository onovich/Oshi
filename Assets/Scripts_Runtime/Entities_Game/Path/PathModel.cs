using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public class PathModel : MonoBehaviour {

        // Base Info
        public int index;
        public int typeID;

        // Easing
        public EasingType easingType;
        public EasingMode easingMode;

        // Traveler
        public EntityType travelerType;
        public int travelerIndex;

        // Node
        Vector3[] pathNodeArr;
        public int currentPathNodeIndex;
        public int nodeIndexDir;

        // Car
        public Vector3 pathCarPos;

        // Loop Type
        public bool isCircleLoop;
        public bool isPingPongLoop;

        // Timer
        public float movingDuration;
        public float movingCurrentTime;

        // State
        public bool isMoving;

        // Render
        [SerializeField] LineRenderer lineRenderer;

        public void Ctor() {
            nodeIndexDir = 1;
        }

        // Line Render
        public void Line_SetMaterial(Material material) {
            lineRenderer.material = material;
        }

        public void Line_SetColor(Color color) {
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }

        public void Line_SetWidth(float width) {
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
        }

        public void Line_SetPositions(Vector3[] positions, Vector2 offset) {
            var lrPosArr = new Vector3[positions.Length];
            Array.Copy(positions, lrPosArr, positions.Length);
            lineRenderer.positionCount = isCircleLoop ? lrPosArr.Length + 1 : lrPosArr.Length;
            for (int i = 0; i < positions.Length; i++) {
                lrPosArr[i] += new Vector3(offset.x, offset.y, 0);
            }
            lineRenderer.SetPositions(lrPosArr);
            if (isCircleLoop) {
                lineRenderer.SetPosition(lrPosArr.Length, lrPosArr[0]);
            }
        }

        // Node
        public void Path_SetNodeArr(Vector3[] pathNodeArr) {
            this.pathNodeArr = new Vector3[pathNodeArr.Length];
            Array.Copy(pathNodeArr, this.pathNodeArr, pathNodeArr.Length);
            pathCarPos = pathNodeArr[0];
        }

        public Vector2Int GetCurrentNode() {
            return pathNodeArr[currentPathNodeIndex].RoundToVector2Int();
        }

        public Vector2Int GetNextNode() {
            var nextIndex = GetNextIndex();
            return pathNodeArr[nextIndex].RoundToVector2Int();
        }

        // Car
        public void Tick_MoveCarToNext(float dt, out bool isEnd) {
            isEnd = false;
            var current = pathNodeArr[currentPathNodeIndex];
            var nextIndex = currentPathNodeIndex + nodeIndexDir;
            nextIndex = ClampIndex(nextIndex);
            var next = pathNodeArr[nextIndex];
            var pos = EasingHelper.Easing2D(current, next, movingCurrentTime, movingDuration, easingType, easingMode);
            movingCurrentTime += dt;
            pathCarPos = pos;
            isMoving = true;
            if (movingCurrentTime >= movingDuration) {
                isEnd = true;
                pathCarPos = next;
                ResetMoveState();
                isMoving = false;
            }
        }

        public void ResetMoveState() {
            movingCurrentTime = 0;
        }

        // Index
        public int GetNextIndex() {
            var nextIndex = currentPathNodeIndex + nodeIndexDir;
            nextIndex = ClampIndex(nextIndex);
            return nextIndex;
        }

        public void Undo() {
            currentPathNodeIndex -= nodeIndexDir;
            currentPathNodeIndex = ClampIndex(currentPathNodeIndex);
            movingCurrentTime = 0;
        }

        public void ResetTimer() {
            movingCurrentTime = 0;
        }

        public void PushIndexToNext() {
            currentPathNodeIndex += nodeIndexDir;
            currentPathNodeIndex = ClampIndex(currentPathNodeIndex);
        }

        int ClampIndex(int index) {
            if (isCircleLoop) {
                if (index >= pathNodeArr.Length) index = 0;
                if (index < 0) index = pathNodeArr.Length - 1;
            } else if (isPingPongLoop) {
                if (index >= pathNodeArr.Length) {
                    index = pathNodeArr.Length - 2;
                    nodeIndexDir = -1;
                }
                if (index < 0) {
                    index = 1;
                    nodeIndexDir = 1;
                }
            } else {
                if (index >= pathNodeArr.Length) index = pathNodeArr.Length - 1;
                if (index < 0) index = 0;
            }
            return index;
        }

        public void TearDown() {
            Destroy(gameObject);
        }

    }

}