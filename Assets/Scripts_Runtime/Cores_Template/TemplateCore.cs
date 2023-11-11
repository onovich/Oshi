using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Alter {

    public static class TemplateCore {

        public static async Task Load(TemplateCoreContext ctx) {

            var blocks = await Addressables.LoadAssetsAsync<BlockSO>("TM_Block", null).Task;
            foreach (var item in blocks) {
                ctx.BlockTM_Add(item.typeID, item.tm);
            }

        }

    }

}