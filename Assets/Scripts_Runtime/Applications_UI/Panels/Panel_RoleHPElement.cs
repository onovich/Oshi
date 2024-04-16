using System;
using UnityEngine;
using UnityEngine.UI;
using MortiseFrame.Loom;

namespace Alter.UI {

    public class Panel_RoleHPElement : MonoBehaviour {

        [SerializeField] Image hp;

        public void EnableHP(bool enable) {
            hp.enabled = enable;
        }

    }

}