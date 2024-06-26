using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using VTD;

namespace Oshi {

    public static class TemplateInfra {

        public static async Task LoadAssets(TemplateInfraContext ctx) {

            {
                var handle = Addressables.LoadAssetAsync<GameConfig>("TM_Config");
                var config = await handle.Task;
                ctx.Config_Set(config);
                ctx.configHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<MapTM>("TM_Map", null);
                var mapList = await handle.Task;
                foreach (var tm in mapList) {
                    ctx.Map_Add(tm);
                }
                ctx.mapHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<RoleTM>("TM_Role", null);
                var roleList = await handle.Task;
                foreach (var tm in roleList) {
                    ctx.Role_Add(tm);
                }
                ctx.roleHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<BlockTM>("TM_Block", null);
                var blockList = await handle.Task;
                foreach (var tm in blockList) {
                    ctx.Block_Add(tm);
                }
                ctx.blockHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<WallTM>("TM_Wall", null);
                var wallList = await handle.Task;
                foreach (var tm in wallList) {
                    ctx.Wall_Add(tm);
                }
                ctx.wallHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<GoalTM>("TM_Goal", null);
                var goalList = await handle.Task;
                foreach (var tm in goalList) {
                    ctx.Goal_Add(tm);
                }
                ctx.goalHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<SpikeTM>("TM_Spike", null);
                var spikeList = await handle.Task;
                foreach (var tm in spikeList) {
                    ctx.Spike_Add(tm);
                }
                ctx.spikeHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<PathTM>("TM_Path", null);
                var pathList = await handle.Task;
                foreach (var tm in pathList) {
                    ctx.Path_Add(tm);
                }
                ctx.pathHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<TerrainTM>("TM_Terrain", null);
                var terrainList = await handle.Task;
                foreach (var tm in terrainList) {
                    ctx.Terrain_Add(tm);
                }
                ctx.terrainHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetsAsync<GateTM>("TM_Gate", null);
                var gateList = await handle.Task;
                foreach (var tm in gateList) {
                    ctx.Gate_Add(tm);
                }
                ctx.gateHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetAsync<SoundTable>("Table_Sound");
                var soundTable = await handle.Task;
                ctx.SoundTable_Set(soundTable);
                ctx.soundTableHandle = handle;
            }

            {
                var handle = Addressables.LoadAssetAsync<VFXTable>("Table_VFX");
                var vfxTable = await handle.Task;
                ctx.VFXTable_Set(vfxTable);
                ctx.vfxTableHandle = handle;
            }

        }

        public static void Release(TemplateInfraContext ctx) {
            if (ctx.configHandle.IsValid()) {
                Addressables.Release(ctx.configHandle);
            }
            if (ctx.mapHandle.IsValid()) {
                Addressables.Release(ctx.mapHandle);
            }
            if (ctx.roleHandle.IsValid()) {
                Addressables.Release(ctx.roleHandle);
            }
            if (ctx.blockHandle.IsValid()) {
                Addressables.Release(ctx.blockHandle);
            }
            if (ctx.wallHandle.IsValid()) {
                Addressables.Release(ctx.wallHandle);
            }
            if (ctx.goalHandle.IsValid()) {
                Addressables.Release(ctx.goalHandle);
            }
            if (ctx.spikeHandle.IsValid()) {
                Addressables.Release(ctx.spikeHandle);
            }
            if (ctx.pathHandle.IsValid()) {
                Addressables.Release(ctx.pathHandle);
            }
            if (ctx.terrainHandle.IsValid()) {
                Addressables.Release(ctx.terrainHandle);
            }
            if (ctx.gateHandle.IsValid()) {
                Addressables.Release(ctx.gateHandle);
            }
            if (ctx.soundTableHandle.IsValid()) {
                Addressables.Release(ctx.soundTableHandle);
            }
            if (ctx.vfxTableHandle.IsValid()) {
                Addressables.Release(ctx.vfxTableHandle);
            }
        }

    }

}