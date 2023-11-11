using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public class UIAppContext {

        // Internal
        public Canvas mainCanvas;
        public UIEventCenter evt;

        public Transform mainCanvasRoot => mainCanvas.transform;
        public Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
        public Dictionary<string, MonoBehaviour> uniquePanelDic = new Dictionary<string, MonoBehaviour>();
        public Dictionary<string, MonoBehaviour> openedUniqueDic;


        // External
        public TemplateCoreContext templateCoreContext;

        public UIAppContext() {
            evt = new UIEventCenter();
            prefabDic = new Dictionary<string, GameObject>();
            openedUniqueDic = new Dictionary<string, MonoBehaviour>();
        }

        public void UniquePanel_Add(string name, MonoBehaviour panel) {
            uniquePanelDic.Add(name, panel);
        }

        public void UniquePanel_Remove(string name) {
            uniquePanelDic.Remove(name);
        }

        public bool UniquePanel_TryGet(string name, out MonoBehaviour panel) {
            return uniquePanelDic.TryGetValue(name, out panel);
        }

        public T UniquePanel_Get<T>() where T : MonoBehaviour {
            string name = typeof(T).Name;
            bool has = openedUniqueDic.TryGetValue(name, out var comp);
            if (!has) {
                return null;
            }
            var panel = comp as T;
            return panel;
        }

    }

}