using System;
using UnityEngine;

namespace Alter {

    public class RoleMod : MonoBehaviour {

        [SerializeField] SpriteRenderer spr;
        [SerializeField] Animator anim;

        public void SetSprite(Sprite sprite) {
            spr.sprite = sprite;
        }

        public void SetColorAlpha(float alpha) {
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, alpha);
        }

        public void PlayIdle() {
            anim.Play("Idle");
        }

        public void PlayAttack() {
            anim.Play("Attack");
        }

        public void PlayAttackFail() {
            anim.Play("Attack_Fail");
        }

        public void PlayHurt() {
            anim.Play("Hurt");
        }

        public void Anim_SetMovement(float speed) {
            anim.SetFloat("HorizontalMovement", speed);
        }

        public void TearDown() {
            Destroy(this.gameObject);
        }

    }

}