using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public class PathModel {

        public int index;
        public int typeID;
        public EasingType easingType;
        public EasingMode easingMode;

        public EntityType travelerType;
        public int travelerIndex;

        Vector2Int[] pathNodeArr;
        public int currentPathNodeIndex;
        public Vector2 pathCar;
        public bool isCircleLoop;
        public bool isPingPongLoop;
        public int dir;
        public float durationTime;
        public float currentTime;

        // FSM
        PathFSMComponent pathFSMCom;

        public PathModel() {
            dir = 1;
            pathFSMCom = new PathFSMComponent();
        }

        // FSM
        public PathFSMStatus FSM_GetStatus() {
            return pathFSMCom.status;
        }

        public PathFSMComponent FSM_GetComponent() {
            return pathFSMCom;
        }

        public void Path_SetNodeArr(Vector2Int[] pathNodeArr) {
            this.pathNodeArr = new Vector2Int[pathNodeArr.Length];
            Array.Copy(pathNodeArr, this.pathNodeArr, pathNodeArr.Length);
            pathCar = pathNodeArr[0];
            durationTime = .5f;
        }

        public void PushIndexToNext() {
            currentPathNodeIndex += dir;
            currentPathNodeIndex = ClampIndex(currentPathNodeIndex);
        }

        public void Tick_MoveCarToNext(float dt, Action onEnd) {
            var current = pathNodeArr[currentPathNodeIndex];
            var nextIndex = currentPathNodeIndex + dir;
            nextIndex = ClampIndex(nextIndex);
            var next = pathNodeArr[nextIndex];
            var pos = EasingHelper.Easing2D(current, next, currentTime, durationTime, easingType, easingMode);
            currentTime += dt;

            pathCar = pos;
            if (currentTime >= durationTime) {
                onEnd?.Invoke();
            }
        }

        int ClampIndex(int index) {
            if (isCircleLoop) {
                if (index >= pathNodeArr.Length) index = 0;
                if (index < 0) index = pathNodeArr.Length - 1;
            } else if (isPingPongLoop) {
                if (index >= pathNodeArr.Length) {
                    index = pathNodeArr.Length - 2;
                    dir = -1;
                }
                if (index < 0) {
                    index = 1;
                    dir = 1;
                }
            } else {
                if (index >= pathNodeArr.Length) index = pathNodeArr.Length - 1;
                if (index < 0) index = 0;
            }
            return index;
        }

    }

}