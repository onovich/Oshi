using System;
using UnityEngine;
using UnityEngine.UI;
using TenonKit.Loom;

namespace Oshi.UI {

    public class Panel_Login : MonoBehaviour, IPanel {

        [SerializeField] Button startGameBtn;
        [SerializeField] Button loadGameBtn;
        [SerializeField] Button exitGameBtn;

        public Action OnClickStartGameHandle;
        public Action OnClickLoadGameHandle;
        public Action OnClickExitGameHandle;

        public void Ctor() {
            startGameBtn.onClick.AddListener(() => {
                OnClickStartGameHandle?.Invoke();
            });

            loadGameBtn.onClick.AddListener(() => {
                OnClickLoadGameHandle?.Invoke();
            });

            exitGameBtn.onClick.AddListener(() => {
                OnClickExitGameHandle?.Invoke();
            });
        }

        public void SetLoadInteractable(bool interactable) {
            loadGameBtn.interactable = interactable;
        }

        void OnDestroy() {
            startGameBtn.onClick.RemoveAllListeners();
            exitGameBtn.onClick.RemoveAllListeners();
            loadGameBtn.onClick.RemoveAllListeners();
            OnClickStartGameHandle = null;
            OnClickExitGameHandle = null;
            OnClickLoadGameHandle = null;
        }

    }

}