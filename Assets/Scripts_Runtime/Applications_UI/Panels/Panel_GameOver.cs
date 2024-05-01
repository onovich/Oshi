using System;
using UnityEngine;
using UnityEngine.UI;
using MortiseFrame.Loom;

namespace Oshi.UI {

    public class Panel_GameOver : MonoBehaviour, IPanel {

        [SerializeField] Button restartGameBtn;
        [SerializeField] Button exitGameBtn;
        [SerializeField] Button nextLevelBtn;

        [SerializeField] Text resultTxt;

        public Action OnClickRestartGameHandle;
        public Action OnClickExitGameHandle;
        public Action OnClickNextLevelHandle;

        public void Ctor() {
            restartGameBtn.onClick.AddListener(() => {
                OnClickRestartGameHandle?.Invoke();
            });

            exitGameBtn.onClick.AddListener(() => {
                OnClickExitGameHandle?.Invoke();
            });

            nextLevelBtn.onClick.AddListener(() => {
                OnClickNextLevelHandle?.Invoke();
            });
        }

        public void SetResult(string result) {
            resultTxt.text = result;
        }

        public void ShowExitBtn(bool show) {
            exitGameBtn.gameObject.SetActive(show);
        }

        public void ShowNextLevelBtn(bool show) {
            nextLevelBtn.gameObject.SetActive(show);
        }

        void OnDestroy() {
            restartGameBtn.onClick.RemoveAllListeners();
            exitGameBtn.onClick.RemoveAllListeners();
            nextLevelBtn.onClick.RemoveAllListeners();
            OnClickRestartGameHandle = null;
            OnClickExitGameHandle = null;
            OnClickNextLevelHandle = null;
        }

    }

}