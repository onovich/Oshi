using System;
using UnityEngine;
using UnityEngine.UI;
using MortiseFrame.Loom;
using System.Collections.Generic;

namespace Chouten.UI {

    public class Panel_GameInfo : MonoBehaviour, IPanel {

        [SerializeField] Text timeText;
        [SerializeField] Panel_RoleHPElement hpPrefab;
        [SerializeField] RectTransform hpRoot;
        List<Panel_RoleHPElement> hpElements;

        public void Ctor(int hpMax) {
            hpElements = new List<Panel_RoleHPElement>(hpMax);
            for (int i = 0; i < hpMax; i++) {
                var hpElement = Instantiate(hpPrefab, hpRoot);
                hpElements.Add(hpElement);
            }
        }

        public void RefreshHP(int hp) {
            hpRoot.gameObject.SetActive(hp > 0);
            for (int i = 0; i < hpElements.Count; i++) {
                hpElements[i].EnableHP(i < hp);
            }
        }

        public void RefreshTime(float time) {
            timeText.text = time.ToString("F0");
        }

        void OnDestroy() {
            if (hpElements == null || hpElements.Count == 0) {
                return;
            }
            foreach (var hpElement in hpElements) {
                Destroy(hpElement.gameObject);
            }
            hpElements.Clear();
        }

    }

}