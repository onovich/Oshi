using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Alter {

    public class UIApp {

        public static async Task LoadAssets(UIAppContext ctx) {
            var list = await Addressables.LoadAssetsAsync<GameObject>("UI", null).Task;
            foreach (var prefab in list) {
                ctx.prefabDic.Add(prefab.name, prefab);
            }
        }

        public static void GameInfo_Open(UIAppContext ctx) {
            PanelGameInfoDomain.Open(ctx);
        }

        public static void GameInfo_SetLevel(UIAppContext ctx, int level) {
            PanelGameInfoDomain.SetLevel(ctx, level);
        }

        public static void Tick(UIAppContext ctx, float dt) {

        }

    }

}