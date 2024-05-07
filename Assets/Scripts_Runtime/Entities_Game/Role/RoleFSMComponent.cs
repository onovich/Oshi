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
        public bool moving_pushTarget;
        public EntityType moving_pushTargetType;
        public int moving_pushTargetIndex;
        public Vector2Int moving_pushTargetStartPos;

        public RoleFSMComponent() { }

        public void Idle_Enter() {
            status = RoleFSMStatus.Idle;
            idle_isEntering = true;
        }

        public void Moving_EnterWithOutPush(float duration,
                                 Vector2 start,
                                 Vector2 end) {
            Reset();
            status = RoleFSMStatus.Moving;
            moving_isEntering = true;
            moving_durationSec = duration;
            moving_currentSec = 0;
            moving_start = start;
            moving_end = end;
            moving_pushTarget = false;
        }

        public void Moving_EnterWithPush(float duration,
                                 Vector2 start,
                                 Vector2 end,
                                 EntityType pushTargetType,
                                 int pushTargetIndex,
                                 Vector2Int pushTargetStartPos) {
            Reset();
            status = RoleFSMStatus.Moving;
            moving_isEntering = true;
            moving_durationSec = duration;
            moving_currentSec = 0;
            moving_start = start;
            moving_end = end;
            moving_pushTarget = true;
            moving_pushTargetType = pushTargetType;
            moving_pushTargetIndex = pushTargetIndex;
            moving_pushTargetStartPos = pushTargetStartPos;
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
            moving_pushTarget = false;
            moving_pushTargetIndex = 0;
            moving_pushTargetType = EntityType.None;
        }

    }

}