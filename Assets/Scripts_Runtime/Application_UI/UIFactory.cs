using UnityEngine;

namespace Alter {

    public static class UIFactory {

        // UniquePanel
        public static T UniquePanel_Open<T>(UIAppContext ctx) where T : MonoBehaviour {
            var dic = ctx.prefabDic;
            string name = typeof(T).Name;
            bool has = dic.TryGetValue(name, out var prefab);
            if (!has) {
                AlterLog.LogError($"UIFactory.Open<{name}>: prefab not found");
                return null;
            }
            var panel = GameObject.Instantiate(prefab, ctx.mainCanvasRoot).GetComponent<T>();
            ctx.UniquePanel_Add(name, panel);
            return panel;
        }

        public static void UniquePanel_Close<T>(UIAppContext ctx) where T : MonoBehaviour {
            string name = typeof(T).Name;
            bool has = ctx.UniquePanel_TryGet(name, out var panel);
            if (!has) {
                AlterLog.LogError($"UIFactory.Close<{name}>: component not found");
                return;
            }
            ctx.UniquePanel_Remove(name);
            GameObject.Destroy(panel.gameObject);
        }

    }

}