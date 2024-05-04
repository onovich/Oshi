using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using VTD;

namespace Oshi {

    public class TemplateInfraContext {

        GameConfig config;
        public AsyncOperationHandle configHandle;

        Dictionary<int, MapTM> mapDict;
        MapTM lastMap;
        public AsyncOperationHandle mapHandle;

        Dictionary<int, RoleTM> roleDict;
        public AsyncOperationHandle roleHandle;

        Dictionary<int, BlockTM> blockDict;
        public AsyncOperationHandle blockHandle;

        Dictionary<int, WallTM> wallDict;
        public AsyncOperationHandle wallHandle;

        Dictionary<int, GoalTM> goalDict;
        public AsyncOperationHandle goalHandle;

        Dictionary<int, SpikeTM> spikeDict;
        public AsyncOperationHandle spikeHandle;

        Dictionary<int, PathTM> pathDict;
        public AsyncOperationHandle pathHandle;

        Dictionary<int, TerrainTM> terrainDict;
        public AsyncOperationHandle terrainHandle;

        SoundTable soundTable;
        public AsyncOperationHandle soundTableHandle;

        public TemplateInfraContext() {
            mapDict = new Dictionary<int, MapTM>();
            roleDict = new Dictionary<int, RoleTM>();
            blockDict = new Dictionary<int, BlockTM>();
            wallDict = new Dictionary<int, WallTM>();
            goalDict = new Dictionary<int, GoalTM>();
            spikeDict = new Dictionary<int, SpikeTM>();
            pathDict = new Dictionary<int, PathTM>();
            terrainDict = new Dictionary<int, TerrainTM>();
        }

        // Game
        public void Config_Set(GameConfig config) {
            this.config = config;
        }

        public GameConfig Config_Get() {
            return config;
        }

        // Map
        public void Map_Add(MapTM map) {
            mapDict.Add(map.typeID, map);
            if (map.isLastMap) {
                lastMap = map;
            }
        }

        public bool Map_TryGet(int typeID, out MapTM map) {
            var has = mapDict.TryGetValue(typeID, out map);
            if (!has) {
                GLog.LogError($"Map {typeID} not found");
            }
            return has;
        }

        public MapTM Map_GetLast() {
            return lastMap;
        }

        // Role
        public void Role_Add(RoleTM role) {
            roleDict.Add(role.typeID, role);
        }

        public bool Role_TryGet(int typeID, out RoleTM role) {
            var has = roleDict.TryGetValue(typeID, out role);
            if (!has) {
                GLog.LogError($"Role {typeID} not found");
            }
            return has;
        }

        // Block
        public void Block_Add(BlockTM block) {
            blockDict.Add(block.typeID, block);
        }

        public bool Block_TryGet(int typeID, out BlockTM block) {
            var has = blockDict.TryGetValue(typeID, out block);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }
            return has;
        }

        // Wall
        public void Wall_Add(WallTM wall) {
            wallDict.Add(wall.typeID, wall);
        }

        public bool Wall_TryGet(int typeID, out WallTM wall) {
            var has = wallDict.TryGetValue(typeID, out wall);
            if (!has) {
                GLog.LogError($"Wall {typeID} not found");
            }
            return has;
        }

        // Goal
        public void Goal_Add(GoalTM goal) {
            goalDict.Add(goal.typeID, goal);
        }

        public bool Goal_TryGet(int typeID, out GoalTM goal) {
            var has = goalDict.TryGetValue(typeID, out goal);
            if (!has) {
                GLog.LogError($"Goal {typeID} not found");
            }
            return has;
        }

        // Spike
        public void Spike_Add(SpikeTM spike) {
            spikeDict.Add(spike.typeID, spike);
        }

        public bool Spike_TryGet(int typeID, out SpikeTM spike) {
            var has = spikeDict.TryGetValue(typeID, out spike);
            if (!has) {
                GLog.LogError($"Spike {typeID} not found");
            }
            return has;
        }

        // Path
        public void Path_Add(PathTM path) {
            pathDict.Add(path.typeID, path);
        }

        public bool Path_TryGet(int typeID, out PathTM path) {
            var has = pathDict.TryGetValue(typeID, out path);
            if (!has) {
                GLog.LogError($"Path {typeID} not found");
            }
            return has;
        }

        // Terrain
        public void Terrain_Add(TerrainTM terrain) {
            terrainDict.Add(terrain.typeID, terrain);
        }

        public bool Terrain_TryGet(int typeID, out TerrainTM terrain) {
            var has = terrainDict.TryGetValue(typeID, out terrain);
            if (!has) {
                GLog.LogError($"Terrain {typeID} not found");
            }
            return has;
        }

        // Sound
        public void SoundTable_Set(SoundTable soundTable) {
            this.soundTable = soundTable;
        }

        public SoundTable SoundTable_Get() {
            return soundTable;
        }

        // Clear
        public void Clear() {
            lastMap = null;
            mapDict.Clear();
            roleDict.Clear();
            blockDict.Clear();
            wallDict.Clear();
            goalDict.Clear();
            spikeDict.Clear();
            pathDict.Clear();
            terrainDict.Clear();
        }

    }

}