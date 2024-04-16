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
        public void Move_ApplyMove(float dt) {
            float dir = 0f;
            if (allyStatus == AllyStatus.Player) {
                dir = inputCom.skillAxis.x;
            } else if (allyStatus == AllyStatus.Enemy) {
                dir = faceDir.x;
                Move_Apply(dir, Attr_GetMoveSpeed(), dt);
            } else {
                GLog.LogError($"Move_ApplyMove: unknown allyStatus: {allyStatus}");
            }
            Move_SetFace(dir * Vector2.right);
        }

        public void Move_Stop() {
            Move_Apply(0, 0, 0);
        }

        void Move_Apply(float xAxis, float moveSpeed, float fixdt) {
            var velo = rb.velocity;
            velo.x = xAxis * moveSpeed;
            rb.velocity = velo;
        }

        public void Move_SetFace(Vector2 moveDir) {
            if (moveDir != Vector2.zero) {
                faceDir = moveDir;
            }

            if (moveDir.x != 0) {
                body.localScale = new Vector3(Mathf.Abs(body.localScale.x) * -Mathf.Sign(moveDir.x),
                                              body.localScale.y,
                                              body.localScale.z);
            }
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