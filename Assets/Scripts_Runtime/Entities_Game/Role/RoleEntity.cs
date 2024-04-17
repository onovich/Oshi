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
        public Vector2 faceDir;
        public int hpMax;
        public int hp;

        // Size
        public Vector2 size;

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

        // Anim
        public RoleAnimComponent animCom;

        // Render
        [SerializeField] SpriteRenderer spr;
        [SerializeField] Animator anim;

        // VFX
        public string deadVFXName;
        public float deadVFXDuration;

        // Pos
        public Vector2 Pos => Pos_GetPos();
        public Vector2Int PosInt => Pos_GetPos().RoundToVector2Int();
        public Vector2 lastFramePos;

        public void Ctor() {
            fsmCom = new RoleFSMComponent();
            animCom = new RoleAnimComponent();
            inputCom = new RoleInputComponent();
            stageCounter = 0;
        }

        // Pos
        public void Pos_SetPos(Vector2 pos) {
            transform.position = pos;
        }

        public void Pos_RecordLastFramePos() {
            lastFramePos = transform.position;
        }

        Vector2 Pos_GetPos() {
            return transform.position;
        }

        public Vector2Int Pos_GetNextGrid() {
            var axis = inputCom.moveAxis;
            if (axis.x != 0 && axis.y != 0) {
                axis = Vector2Int.zero;
            }
            return PosInt + inputCom.moveAxis;
        }

        public Vector2Int Pos_GetNextDir() {
            return inputCom.moveAxis;
        }

        // Size
        public void Size_SetSize(Vector2 size) {
            spr.size = size;
            this.size = size;
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
        public bool Move_CheckConstraint(Vector2 constarintSize, Vector2 constraintCenter) {
            var moveAxisX = inputCom.moveAxis.x;
            var moveAxisY = inputCom.moveAxis.y;
            var pos = transform.position;
            var min = constraintCenter - constarintSize / 2 + constraintCenter - Vector2.one;
            var max = constraintCenter + constarintSize / 2 + constraintCenter;
            if (pos.x + moveAxisX >= max.x || pos.x + moveAxisX <= min.x) {
                return false;
            }
            if (pos.y + moveAxisY >= max.y || pos.y + moveAxisY <= min.y) {
                return false;
            }
            return true;
        }

        // Mesh
        public void Mesh_Set(Sprite sp) {
            spr.sprite = sp;
        }

        public void Mesh_SetAlpha(float alpha) {
            var color = spr.color;
            color.a = alpha;
            spr.color = color;
        }

        public void Mesh_SetMaterial(Material mat) {
            this.spr.material = mat;
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

        public void FSM_EnterMoving(float duration, bool push = false, int blockIndex = 0, Vector2Int blockOldPos = default) {
            var start = transform.position;
            if (inputCom.moveAxis.x != 0 && inputCom.moveAxis.y != 0) {
                return;
            }
            var end = new Vector2(start.x + inputCom.moveAxis.x, start.y + inputCom.moveAxis.y);
            fsmCom.Moving_Enter(duration, start, end, push, blockIndex, blockOldPos);
        }

        public void FSM_EnterDead() {
            fsmCom.Dead_Enter();
        }

        // VFX
        public void TearDown() {
            Destroy(this.gameObject);
        }

    }

}