using System;
using UnityEngine;

namespace Oshi {

    public class RoleAnimComponent {

        public void PlayIdle(Animator anim) {
            anim.Play("Idle");
        }

        public void PlayAttack(Animator anim) {
            anim.Play("Attack");
        }

        public void PlayAttackFail(Animator anim) {
            anim.Play("Attack_Fail");
        }

        public void PlayHurt(Animator anim) {
            anim.Play("Hurt");
        }

        public void Anim_SetMovement(Animator anim, float speed) {
            anim.SetFloat("HorizontalMovement", speed);
        }

    }

}