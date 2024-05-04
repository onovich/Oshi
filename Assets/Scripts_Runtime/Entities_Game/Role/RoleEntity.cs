using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public class RoleEntity : MonoBehaviour {

        // Base Info
        public int entityID;
        public int typeID;
        public string typeName;
        public AllyStatus allyStatus;

        // Attr
        public Vector2 faceDir;
        public int hpMax;
        public int hp;

        // Size
        public Vector2Int size;

        // Move
        public float moveDurationSec;
        public EasingType moveEasingType;
        public EasingMode moveEasingMode;

        // State
        public bool needTearDown;
        public int step;

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
            step = 0;
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

        public Vector2Int Pos_GetDir() {
            return inputCom.moveAxis;
        }

        // Size
        public void Size_SetSize(Vector2Int size) {
            spr.size = size;
            this.size = size;
        }

        // State
        public void State_IncStageCounter() {
            step++;
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

        public void FSM_EnterMoving(Vector2 end,
                                    float duration,
                                    bool push = false,
                                    int blockIndex = 0,
                                    Vector2Int blockOldPos = default,
                                    Vector2Int pushTarget = default) {
            var start = transform.position;
            if (inputCom.moveAxis.x != 0 && inputCom.moveAxis.y != 0) {
                return;
            }
            fsmCom.Moving_Enter(duration, start, end, push, blockIndex, blockOldPos, pushTarget);
        }

        public void FSM_EnterDead() {
            if (fsmCom.status == RoleFSMStatus.Dead) {
                return;
            }
            fsmCom.Dead_Enter();
        }

        // VFX
        public void TearDown() {
            Destroy(this.gameObject);
        }

    }

}