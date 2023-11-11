using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TemplateCore {

    Dictionary<int, BlockTM> blockTMs;

    public async Task Load() {

        var blocks = await Addressables.LoadAssetsAsync<BlockSO>("TM_Block", null).Task;
        foreach (var item in blocks) {
            blockTMs.Add(item.typeID, item.tm);
        }

    }

}