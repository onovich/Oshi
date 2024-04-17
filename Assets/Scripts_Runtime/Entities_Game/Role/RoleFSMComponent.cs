using UnityEngine;

namespace Alter {

    public class RoleFSMComponent {

        public RoleFSMStatus status;

        public bool idle_isEntering;
        public bool moving_isEntering;
        public bool dead_isEntering;

        public float moving_durationSec;
        public float moving_currentSec;
        public Vector2 moving_start;
        public Vector2 moving_end;

        public RoleFSMComponent() { }

        public void Idle_Enter() {
            status = RoleFSMStatus.Idle;
            idle_isEntering = true;
        }

        public void Moving_Enter(float duration, Vector2 start, Vector2 end) {
            status = RoleFSMStatus.Moving;
            moving_isEntering = true;
            moving_durationSec = duration;
            moving_currentSec = 0;
            moving_start = start;
            moving_end = end;
        }

        public void Moving_IncTimer(float dt) {
            moving_currentSec += dt;
        }

        public void Dead_Enter() {
            status = RoleFSMStatus.Dead;
            dead_isEntering = true;
        }

    }

}