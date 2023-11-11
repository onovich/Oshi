using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public class AssetsCoreContext {

        Dictionary<string, GameObject> entities;

        public AssetsCoreContext() {
            entities = new Dictionary<string, GameObject>();
        }

        public void Entity_Add(string key, GameObject Entity) {
            entities.Add(key, Entity);
        }

        bool Entity_TryGet(string key, out GameObject Entity) {
            return entities.TryGetValue(key, out Entity);
        }

        public void Entity_RemoveBlock(string key) {
            entities.Remove(key);
        }

        public GameObject Entity_GetBlock() {
            if (entities.TryGetValue("Go_Block", out GameObject Entity)) {
                return Entity;
            }
            Debug.LogError("Entity not found: Go_Block");
            return null;
        }

        public GameObject Entity_GetBlockUnitMod() {
            if (entities.TryGetValue("Mod_BlockUnit", out GameObject Entity)) {
                return Entity;
            }
            Debug.LogError("Entity not found: Mod_BlockUnit");
            return null;
        }

    }

}