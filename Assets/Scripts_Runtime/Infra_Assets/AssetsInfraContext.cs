using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;

namespace Oshi {

    public class AssetsInfraContext {

        Dictionary<string, GameObject> entityDict;
        public AsyncOperationHandle entityHandle;

        public AssetsInfraContext() {
            entityDict = new Dictionary<string, GameObject>();
        }

        // Entity
        public void Entity_Add(string name, GameObject prefab) {
            entityDict.Add(name, prefab);
        }

        bool Entity_TryGet(string name, out GameObject asset) {
            var has = entityDict.TryGetValue(name, out asset);
            return has;
        }

        public GameObject Entity_GetMap() {
            var has = Entity_TryGet("Entity_Map", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Map not found");
            }
            return prefab;
        }

        public GameObject Entity_GetRole() {
            var has = Entity_TryGet("Entity_Role", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Role not found");
            }
            return prefab;
        }

        public GameObject Entity_GetBlock() {
            var has = Entity_TryGet("Entity_Block", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Block not found");
            }
            return prefab;
        }

        public GameObject Entity_GetWall() {
            var has = Entity_TryGet("Entity_Wall", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Wall not found");
            }
            return prefab;
        }

        public GameObject Entity_GetGate() {
            var has = Entity_TryGet("Entity_Gate", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Gate not found");
            }
            return prefab;
        }

        public GameObject Entity_GetSpike() {
            var has = Entity_TryGet("Entity_Spike", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Spike not found");
            }
            return prefab;
        }

        public GameObject Entity_GetGoal() {
            var has = Entity_TryGet("Entity_Goal", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Goal not found");
            }
            return prefab;
        }

        public GameObject Mod_GetCell() {
            var has = Entity_TryGet("Mod_Cell", out var prefab);
            if (!has) {
                GLog.LogError($"Mod Cell not found");
            }
            return prefab;
        }

        public GameObject Mod_GetNumberCell() {
            var has = Entity_TryGet("Mod_NumberCell", out var prefab);
            if (!has) {
                GLog.LogError($"Mod NumberCell not found");
            }
            return prefab;
        }

        public GameObject Entity_GetPath() {
            var has = Entity_TryGet("Entity_Path", out var prefab);
            if (!has) {
                GLog.LogError($"Entity Path not found");
            }
            return prefab;
        }

    }

}