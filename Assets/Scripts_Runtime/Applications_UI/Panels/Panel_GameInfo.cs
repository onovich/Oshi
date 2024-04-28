using System;
using UnityEngine;
using UnityEngine.UI;
using MortiseFrame.Loom;
using System.Collections.Generic;

namespace Oshi.UI {

    public class Panel_GameInfo : MonoBehaviour, IPanel {

        [SerializeField] Text timeText;
        [SerializeField] Transform timeGroup;
        [SerializeField] Text stepText;
        [SerializeField] Transform stepGroup;
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

        public void ShowTime(bool show) {
            if (show) {
                timeGroup.gameObject.SetActive(true);
            } else {
                timeGroup.gameObject.SetActive(false);
            }
        }

        public void ShowStep(bool show) {
            if (show) {
                stepGroup.gameObject.SetActive(true);
            } else {
                stepGroup.gameObject.SetActive(false);
            }
        }

        public void RefreshGameStageCounter(int counter) {
            stepText.text = counter.ToString();
        }

        public void OnDestroy() {
            restartBtn.onClick.RemoveAllListeners();
            OnRestartBtnClickHandle = null;
        }

    }

}