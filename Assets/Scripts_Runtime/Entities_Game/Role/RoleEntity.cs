using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Alter {

    public class RoleEntity : MonoBehaviour {

        // Base Info
        public int entityID;
        public int typeID;
        public string typeName;
        public AllyStatus allyStatus;

        // Attr
        public float moveSpeed;
        public Vector2 Velocity => rb.velocity;
        public Vector2 faceDir;
        public int hpMax;
        public int hp;
        public Vector2 halfSize;

        // Move
        public float moveDurationSec;
        public EasingType moveEasingType;
        public EasingMode moveEasingMode;

        // State
        public bool needTearDown;
        public int stageCounter;

        // FSM
        public RoleFSMComponent fsmCom;

        // Input
        public RoleInputComponent inputCom;

        // Render
        [SerializeField] public Transform body;
        RoleMod roleMod;

        // VFX
        public string deadVFXName;
        public float deadVFXDuration;

        // Physics
        [SerializeField] Rigidbody2D rb;

        // Pos
        public Vector2 Pos => Pos_GetPos();

        public void Ctor() {
            fsmCom = new RoleFSMComponent();
            inputCom = new RoleInputComponent();
            stageCounter = 0;
        }

        // Pos
        public void Pos_SetPos(Vector2 pos) {
            transform.position = pos;
        }

        Vector2 Pos_GetPos() {
            return transform.position;
        }

        // Attr
        public float Attr_GetMoveSpeed() {
            return moveSpeed;
        }

        // State
        public void State_IncStageCounter() {
            stageCounter++;
        }

        // Move
        public bool Move_CheckMovable(Vector2 constarintSize, Vector2 contraintCenter) {
            var moveAxisX = inputCom.moveAxis.x;
            var moveAxisY = inputCom.moveAxis.y;
            if (moveAxisX == 0 && moveAxisY == 0) {
                return false;
            }

            var pos = transform.position;
            var min = contraintCenter - constarintSize / 2 + contraintCenter + halfSize;
            var max = contraintCenter + constarintSize / 2 + contraintCenter - halfSize;
            if (pos.x + moveAxisX >= max.x || pos.x + moveAxisX <= min.x) {
                return false;
            }
            if (pos.y + moveAxisY >= max.y || pos.y + moveAxisY <= min.y) {
                return false;
            }
            return true;
        }

        // Color
        public void Color_SetAlpha(float alpha) {
            roleMod.SetColorAlpha(alpha);
        }

        // FSM
        public RoleFSMStatus FSM_GetStatus() {
            return fsmCom.status;
        }

        public RoleFSMComponent FSM_GetComponent() {
            return fsmCom;
        }

        public void FSM_EnterIdle() {
            fsmCom.Idle_Enter();
        }

        public void FSM_EnterMoving(float duration) {
            var start = transform.position;
            if (inputCom.moveAxis.x != 0 && inputCom.moveAxis.y != 0) {
                return;
            }
            var end = new Vector2(start.x + inputCom.moveAxis.x, start.y + inputCom.moveAxis.y);
            fsmCom.Moving_Enter(duration, start, end);
        }

        public void FSM_EnterDead() {
            fsmCom.Dead_Enter();
        }

        // Mod
        public void Mod_Set(RoleMod mod) {
            roleMod = mod;
        }

        // VFX
        public void TearDown() {
            roleMod.TearDown();
            Destroy(this.gameObject);
        }

    }

}