using System;
using UnityEngine;
using UnityEngine.UI;
using MortiseFrame.Loom;
using System.Collections.Generic;

namespace Oshi.UI {

    public class Panel_GameInfo : MonoBehaviour, IPanel {

        [SerializeField] Text timeText;
        [SerializeField] Text gameStageCounterText;
        [SerializeField] Button restartBtn;

        public Action OnRestartBtnClickHandle;

        public void Ctor() {
            restartBtn.onClick.AddListener(() => {
                OnRestartBtnClickHandle?.Invoke();
            });
        }

        public void RefreshTime(float time) {
            timeText.text = time.ToString("F0");
        }

        public void RefreshGameStageCounter(int counter) {
            gameStageCounterText.text = counter.ToString();
        }

        public void OnDestroy() {
            restartBtn.onClick.RemoveAllListeners();
            OnRestartBtnClickHandle = null;
        }

    }

}