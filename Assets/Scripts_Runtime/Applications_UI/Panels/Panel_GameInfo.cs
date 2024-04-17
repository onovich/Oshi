using System;
using UnityEngine;
using UnityEngine.UI;
using MortiseFrame.Loom;
using System.Collections.Generic;

namespace Alter.UI {

    public class Panel_GameInfo : MonoBehaviour, IPanel {

        [SerializeField] Text timeText;
        [SerializeField] Text gameStageCounterText;

        public void Ctor() {
        }

        public void RefreshTime(float time) {
            timeText.text = time.ToString("F0");
        }

        public void RefreshGameStageCounter(int counter) {
            gameStageCounterText.text = counter.ToString();
        }

    }

}