using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public class PathModel {

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
        Vector2Int[] pathNodeArr;
        public int currentPathNodeIndex;
        public int nodeIndexDir;

        // Car
        public Vector2 pathCarPos;

        // Loop Type
        public bool isCircleLoop;
        public bool isPingPongLoop;

        // Timer
        public float movingDuration;
        public float movingCurrentTime;

        public PathModel() {
            nodeIndexDir = 1;
        }

        // Node
        public void Path_SetNodeArr(Vector2Int[] pathNodeArr) {
            this.pathNodeArr = new Vector2Int[pathNodeArr.Length];
            Array.Copy(pathNodeArr, this.pathNodeArr, pathNodeArr.Length);
            pathCarPos = pathNodeArr[0];
        }

        public Vector2Int GetCurrentNode() {
            return pathNodeArr[currentPathNodeIndex];
        }

        public Vector2Int GetNextNode() {
            var nextIndex = GetNextIndex();
            return pathNodeArr[nextIndex];
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
            if (movingCurrentTime >= movingDuration) {
                isEnd = true;
                ResetMoveState();
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

    }

}