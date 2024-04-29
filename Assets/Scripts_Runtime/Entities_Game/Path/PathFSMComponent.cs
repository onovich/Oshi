using System.Diagnostics;
using UnityEngine;

namespace Oshi {

    public class PathFSMComponent {

        public PathFSMStatus status;

        public bool idle_isEntering;

        public bool moving_isEntering;
        public float moving_duration;
        public float moving_currentTime;

        public void Idle_Enter() {
            Reset();
            status = PathFSMStatus.Idle;
            idle_isEntering = true;
        }

        public void Moving_Enter(float duration) {
            Reset();
            status = PathFSMStatus.Moving;
            moving_isEntering = true;
            moving_duration = duration;
            moving_currentTime = 0;
        }

        public void Moving_IncTimer(float dt) {
            moving_currentTime += dt;
        }

        public void Reset() {
            idle_isEntering = false;
            moving_isEntering = false;
            moving_duration = 0;
            moving_currentTime = 0;
        }

    }

}