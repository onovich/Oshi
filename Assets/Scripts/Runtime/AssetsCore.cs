using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class AssetsCore {

    public static async Task Load(AssetsCoreContext ctx) {

        {
            var list = await Addressables.LoadAssetsAsync<GameObject>("Go_Block", null).Task;
            foreach (var item in list) {
                ctx.Entity_Add("Go_Block", item);
            }
        }

        {
            var list = await Addressables.LoadAssetsAsync<GameObject>("Mod_BlockUnit", null).Task;
            foreach (var item in list) {
                ctx.Entity_Add("Mod_BlockUnit", item);
            }
        }

    }

}