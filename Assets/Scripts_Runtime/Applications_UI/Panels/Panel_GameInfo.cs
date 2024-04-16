using System;
using UnityEngine;
using UnityEngine.UI;
using MortiseFrame.Loom;
using System.Collections.Generic;

namespace Alter.UI {

    public class Panel_GameInfo : MonoBehaviour, IPanel {

        [SerializeField] Text timeText;
        [SerializeField] RectTransform hpRoot;

        public void Ctor() {
        }

        public void RefreshTime(float time) {
            timeText.text = time.ToString("F0");
        }

    }

}