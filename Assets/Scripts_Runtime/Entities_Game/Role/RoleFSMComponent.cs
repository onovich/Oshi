using System.Numerics;

namespace Chouten {

    public class RoleFSMComponent {

        public RoleFSMStatus status;

        public bool idle_isEntering;
        public bool dead_isEntering;
        public bool leaving_isEntering;
        public bool casting_isEntering;

        public float leaving_totalFrame;
        public float leaving_currentFrame;

        public float casting_totalFrame;
        public float casting_damageFrame;
        public float casting_currentFrame;

        public RoleFSMComponent() { }

        public void EnterIdle() {
            status = RoleFSMStatus.Idle;
            idle_isEntering = true;
        }

        public void EnterDead() {
            status = RoleFSMStatus.Dead;
            dead_isEntering = true;
        }

        public void EnterLeaving(float totalFrame) {
            status = RoleFSMStatus.Leaving;
            leaving_isEntering = true;
            leaving_totalFrame = totalFrame;
            leaving_currentFrame = 0;
        }

        public void Leaving_IncFrame() {
            leaving_currentFrame += 1;
        }

        public void EnterCasting(float totalFrame, float damageFrame) {
            status = RoleFSMStatus.Casting;
            casting_isEntering = true;
            casting_totalFrame = totalFrame;
            casting_damageFrame = damageFrame;
            casting_currentFrame = 0;
        }

        public void Casting_IncFrame() {
            casting_currentFrame += 1;
        }

    }

}