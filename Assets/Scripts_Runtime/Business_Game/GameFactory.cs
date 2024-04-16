using UnityEngine;

namespace Chouten {

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
            map.SetGroundPos(mapTM.middlePoint);

            // Set Bound
            map.middlePos = mapTM.middlePoint;
            map.leftBound = mapTM.leftBound;
            map.rightBound = mapTM.rightBound;

            // SetWave
            map.leftWaveModel = new WaveModel {
                typeID = mapTM.leftWaveTM.typeID,
                spawnTimeArr = mapTM.leftWaveTM.spawnTimeArr,
                roleTypeIDArr = mapTM.leftWaveTM.roleTypeIDArr
            };
            map.rightWaveModel = new WaveModel {
                typeID = mapTM.rightWaveTM.typeID,
                spawnTimeArr = mapTM.rightWaveTM.spawnTimeArr,
                roleTypeIDArr = mapTM.rightWaveTM.roleTypeIDArr
            };

            return map;
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
            role.attackDistance = roleTM.attackDistance;
            role.hpMax = roleTM.hpMax;
            role.hp = role.hpMax;
            role.typeName = roleTM.typeName;
            role.damageFrame = roleTM.damageFrame;
            role.skillTotalFrame = roleTM.skillTotalFrame;

            // Rename
            role.gameObject.name = $"{role.typeName} - {role.entityID}";

            // Set Pos
            role.Pos_SetPos(pos);

            // Set Dir
            role.Move_SetFace(direction);

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