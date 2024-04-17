using UnityEngine;

namespace Alter {

    public static class GameFactory {

        public static MapEntity Map_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 int typeID) {

            var has = templateInfraContext.Map_TryGet(typeID, out var mapTM);
            if (!has) {
                GLog.LogError($"Map {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetMap();
            var map = GameObject.Instantiate(prefab).GetComponent<MapEntity>();
            map.Ctor();
            map.typeID = typeID;
            map.mapSize = mapTM.mapSize;

            // Set Spr
            map.SetSprSize(map.mapSize);

            // Set Point
            map.spawnPoint = mapTM.spawnPoint;

            return map;
        }

        public static WallEntity Wall_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 int typeID,
                                 int index,
                                 Vector2Int pos,
                                 Vector2Int size) {

            var has = templateInfraContext.Wall_TryGet(typeID, out var wallTM);
            if (!has) {
                GLog.LogError($"Wall {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetWall();
            var wall = GameObject.Instantiate(prefab).GetComponent<WallEntity>();
            wall.Ctor();

            // Base Info
            wall.typeID = typeID;
            wall.entityIndex = index;
            wall.typeName = wallTM.typeName;

            // Rename
            wall.gameObject.name = $"Wall - {wall.typeName} - {wall.entityIndex}";

            // Set Pos
            wall.Pos_SetPos(pos);

            // Set Size
            wall.Size_SetSize(size);

            return wall;
        }

        public static BlockEntity Block_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 IDRecordService idRecordService,
                                 int typeID,
                                 int index,
                                 Vector2Int pos,
                                 Vector2Int size) {

            var has = templateInfraContext.Block_TryGet(typeID, out var blockTM);
            if (!has) {
                GLog.LogError($"Block {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetBlock();
            var block = GameObject.Instantiate(prefab).GetComponent<BlockEntity>();
            block.Ctor();

            // Base Info
            block.typeID = typeID;
            block.entityIndex = index;
            block.typeName = blockTM.typeName;

            // Rename
            block.gameObject.name = $"Block - {block.typeName} - {block.entityIndex}";

            // Set Pos
            block.Pos_SetPos(pos);

            // Set Size
            block.Size_SetSize(size);

            return block;
        }

        public static RoleEntity Role_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 IDRecordService idRecordService,
                                 int typeID,
                                 Vector2 pos,
                                 Vector2 direction) {

            var has = templateInfraContext.Role_TryGet(typeID, out var roleTM);
            if (!has) {
                GLog.LogError($"Role {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetRole();
            var role = GameObject.Instantiate(prefab).GetComponent<RoleEntity>();
            role.Ctor();

            // Base Info
            role.entityID = idRecordService.PickRoleEntityID();
            role.typeID = typeID;
            role.allyStatus = roleTM.allyStatus;

            // Set Attr
            role.moveSpeed = roleTM.moveSpeed;
            role.hpMax = roleTM.hpMax;
            role.hp = role.hpMax;
            role.typeName = roleTM.typeName;
            role.halfSize = (Vector2)roleTM.size / 2;

            // Set Move
            role.moveDurationSec = roleTM.moveDurationSec;
            role.moveEasingType = roleTM.moveEasingType;
            role.moveEasingMode = roleTM.moveEasingMode;

            // Rename
            role.gameObject.name = $"{role.typeName} - {role.entityID}";

            // Set Pos
            role.Pos_SetPos(pos);

            // Set Mod
            var modPrefab = roleTM.mod;
            var mod = GameObject.Instantiate(modPrefab, role.body).GetComponent<RoleMod>();
            role.Mod_Set(mod);

            // Set FSM
            role.FSM_EnterIdle();

            // Set VFX
            role.deadVFXName = roleTM.deadVFX.name;
            role.deadVFXDuration = roleTM.deadVFXDuration;

            return role;
        }

    }

}