using System;
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

        // State
        public bool needTearDown;

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

        // Move
        public void Move_ApplyMove(float fixdt) {
            Move_Apply(inputCom.moveAxis);
        }

        public void Move_Stop() {
            Move_Apply(Vector2.zero);
        }

        void Move_Apply(Vector2 axis) {
            var velo = rb.velocity;
            axis.Normalize();
            velo.x = axis.x * moveSpeed;
            velo.y = axis.y * moveSpeed;
            rb.velocity = velo;
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
            fsmCom.EnterIdle();
        }

        public void FSM_EnterDead() {
            fsmCom.EnterDead();
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