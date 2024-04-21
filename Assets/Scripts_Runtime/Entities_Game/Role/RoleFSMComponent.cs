using UnityEngine;

namespace Oshi {

    public class RoleFSMComponent {

        public RoleFSMStatus status;

        public bool idle_isEntering;
        public bool moving_isEntering;
        public bool dead_isEntering;

        public float moving_durationSec;
        public float moving_currentSec;
        public Vector2 moving_start;
        public Vector2 moving_end;
        public bool moving_pushBlock;
        public int moving_pushBlockIndex;
        public Vector2Int moving_pushBlockOldPos;

        public RoleFSMComponent() { }

        public void Idle_Enter() {
            status = RoleFSMStatus.Idle;
            idle_isEntering = true;
        }

        public void Moving_Enter(float duration, Vector2 start, Vector2 end, bool push = false, int blockIndex = 0, Vector2Int blockOldPos = default) {
            Reset();
            status = RoleFSMStatus.Moving;
            moving_isEntering = true;
            moving_durationSec = duration;
            moving_currentSec = 0;
            moving_start = start;
            moving_end = end;
            moving_pushBlock = push;
            moving_pushBlockIndex = blockIndex;
            moving_pushBlockOldPos = blockOldPos;
        }

        public void Moving_IncTimer(float dt) {
            moving_currentSec += dt;
        }

        public void Dead_Enter() {
            Reset();
            status = RoleFSMStatus.Dead;
            dead_isEntering = true;
        }

        public void Reset() {
            status = RoleFSMStatus.Idle;
            idle_isEntering = false;
            moving_isEntering = false;
            dead_isEntering = false;
            moving_durationSec = 0;
            moving_currentSec = 0;
            moving_start = Vector2.zero;
            moving_end = Vector2.zero;
            moving_pushBlock = false;
            moving_pushBlockIndex = 0;
        }

    }

}