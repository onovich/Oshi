using System;
using UnityEngine;
using UnityEngine.UI;
using MortiseFrame.Loom;

namespace Alter.UI {

    public class Panel_GameOver : MonoBehaviour, IPanel {

        [SerializeField] Button restartGameBtn;
        [SerializeField] Button exitGameBtn;
        [SerializeField] Text resultTxt;

        public Action OnClickRestartGameHandle;
        public Action OnClickExitGameHandle;

        public void Ctor() {
            restartGameBtn.onClick.AddListener(() => {
                OnClickRestartGameHandle?.Invoke();
            });

            exitGameBtn.onClick.AddListener(() => {
                OnClickExitGameHandle?.Invoke();
            });
        }

        public void SetResult(string result) {
            resultTxt.text = result;
        }

        void OnDestroy() {
            restartGameBtn.onClick.RemoveAllListeners();
            exitGameBtn.onClick.RemoveAllListeners();
            OnClickRestartGameHandle = null;
            OnClickExitGameHandle = null;
        }

    }

}