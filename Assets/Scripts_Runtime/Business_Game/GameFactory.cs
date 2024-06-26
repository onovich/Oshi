using UnityEngine;

namespace Oshi {

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
            map.typeName = mapTM.typeName;

            // Set Weather
            map.weatherType = mapTM.weatherType;

            // Set Limit
            map.limitedByTime = mapTM.limitedByTime;
            map.gameTotalTime = mapTM.gameTotalTime;
            map.limitedByStep = mapTM.limitedByStep;
            map.gameTotalStep = mapTM.gameTotalStep;

            // Set Mesh
            map.Mesh_SetSize(map.mapSize);

            // Set Next Map
            map.nextMapTypeID = mapTM.nextMapTypeID;
            map.isLastMap = mapTM.isLastMap;

            // Set Point
            map.spawnPoint = mapTM.spawnPoint;

            return map;
        }

        public static PathModel Path_Spawn(TemplateInfraContext templateInfraContext,
                                           AssetsInfraContext assetsInfraContext,
                                           int typeID,
                                           int index,
                                           bool isCircleLoop,
                                           bool isPingPongLoop,
                                           Vector3[] nodes,
                                           EntityType travelerType,
                                           int travelerIndex,
                                           Vector2 travelerHalfSize) {

            var has = templateInfraContext.Path_TryGet(typeID, out var pathTM);
            if (!has) {
                GLog.LogError($"Path {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetPath();
            var path = GameObject.Instantiate(prefab).GetComponent<PathModel>();
            path.Ctor();

            // Base Info
            path.typeID = typeID;
            path.index = index;

            // Attr
            path.easingMode = pathTM.easingMode;
            path.easingType = pathTM.easingType;
            path.isCircleLoop = isCircleLoop;
            path.isPingPongLoop = isPingPongLoop;
            path.movingDuration = pathTM.movingDuration;

            // Set Traveler
            path.travelerType = travelerType;
            path.travelerIndex = travelerIndex;

            // Set Nodes
            path.Path_SetNodeArr(nodes);

            // Set Line
            path.Line_SetMaterial(pathTM.lineMaterial);
            path.Line_SetColor(pathTM.lineColor);
            path.Line_SetWidth(pathTM.lineWidth);
            path.Line_SetPositions(nodes, travelerHalfSize);

            return path;
        }

        public static SpikeEntity Spike_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 int typeID,
                                 int index,
                                 Vector2Int pos) {

            var has = templateInfraContext.Spike_TryGet(typeID, out var spikeTM);
            if (!has) {
                GLog.LogError($"Spike {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetSpike();
            var spike = GameObject.Instantiate(prefab).GetComponent<SpikeEntity>();
            spike.Ctor();

            // Base Info
            spike.typeID = typeID;
            spike.entityIndex = index;
            spike.typeName = spikeTM.typeName;

            // Rename
            spike.gameObject.name = $"Spike - {spike.typeName} - {spike.entityIndex}";

            // Set Pos
            spike.Pos_SetPos(pos);

            // Set Models
            for (int i = 0; i < spikeTM.shapeArr.Length; i++) {
                var shapeTM = spikeTM.shapeArr[i];
                var shape = new Vector2Int[shapeTM.cells.Length];
                shapeTM.ForEachCells((index, localPos) => {
                    shape[index] = localPos;
                });
                var shapeModel = new ShapeModel {
                    index = i,
                    shape = shape,
                    sizeInt = shapeTM.sizeInt,
                    centerFloat = shapeTM.GetCenterFloat()
                };
                spike.shapeComponent.Add(shapeModel);
            }

            return spike;
        }

        public static GoalEntity Goal_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 int typeID,
                                 int index,
                                 Vector2Int pos,
                                 bool canPush,
                                 int number) {

            var has = templateInfraContext.Goal_TryGet(typeID, out var goalTM);
            if (!has) {
                GLog.LogError($"Goal {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetGoal();
            var goal = GameObject.Instantiate(prefab).GetComponent<GoalEntity>();
            goal.Ctor();

            // Base Info
            goal.typeID = typeID;
            goal.entityIndex = index;
            goal.typeName = goalTM.typeName;

            // Can Push
            goal.canPush = canPush;

            // Goal Number
            goal.number = number;
            goal.showNumber = number != 0;
            goal.numberMaterial = goalTM.numberMaterial;
            goal.numberColor = goalTM.numberColor;

            // Rename
            goal.gameObject.name = $"Goal - {goal.typeName} - {goal.entityIndex}";

            // Set Pos
            goal.Pos_SetPos(pos);
            goal.originalPos = pos;

            // Set Models
            for (int i = 0; i < goalTM.shapeArr.Length; i++) {
                var shapeTM = goalTM.shapeArr[i];
                var shape = new Vector2Int[shapeTM.cells.Length];
                shapeTM.ForEachCells((index, localPos) => {
                    shape[index] = localPos;
                });
                var shapeModel = new ShapeModel {
                    index = i,
                    shape = shape,
                    sizeInt = shapeTM.sizeInt,
                    centerFloat = shapeTM.GetCenterFloat()
                };
                goal.shapeComponent.Add(shapeModel);
            }

            return goal;
        }

        public static WallEntity Wall_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 int typeID,
                                 int index,
                                 Vector2Int pos) {

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

            // Set Models
            for (int i = 0; i < wallTM.shapeArr.Length; i++) {
                var shapeTM = wallTM.shapeArr[i];
                var shape = new Vector2Int[shapeTM.cells.Length];
                shapeTM.ForEachCells((index, localPos) => {
                    shape[index] = localPos;
                });
                var shapeModel = new ShapeModel {
                    index = i,
                    shape = shape,
                    sizeInt = shapeTM.sizeInt,
                    centerFloat = shapeTM.GetCenterFloat()
                };
                wall.shapeComponent.Add(shapeModel);
            }

            // Set Line
            wall.Line_SetPoints(wallTM.shapeNodes, wallTM.lineMaterial, wallTM.lineColor, wallTM.lineWidth);

            return wall;
        }

        public static GateEntity Gate_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 int typeID,
                                 int index,
                                 Vector2Int pos,
                                 int nextGateIndex) {

            var has = templateInfraContext.Gate_TryGet(typeID, out var gateTM);
            if (!has) {
                GLog.LogError($"Gate {typeID} not found");
            }

            var prefab = assetsInfraContext.Entity_GetGate();
            var gate = GameObject.Instantiate(prefab).GetComponent<GateEntity>();
            gate.Ctor();

            // Base Info
            gate.typeID = typeID;
            gate.entityIndex = index;
            gate.typeName = gateTM.typeName;

            // Rename
            gate.gameObject.name = $"Gate - {gate.typeName} - {gate.entityIndex}";

            // Set Pos
            gate.Pos_SetPos(pos);
            gate.originalPos = pos;

            // Set Models
            for (int i = 0; i < gateTM.shapeArr.Length; i++) {
                var shapeTM = gateTM.shapeArr[i];
                var shape = new Vector2Int[shapeTM.cells.Length];
                shapeTM.ForEachCells((index, localPos) => {
                    shape[index] = localPos;
                });
                var shapeModel = new ShapeModel {
                    index = i,
                    shape = shape,
                    sizeInt = shapeTM.sizeInt,
                    centerFloat = shapeTM.GetCenterFloat()
                };
                gate.shapeComponent.Add(shapeModel);
            }

            // Set Next Gate
            gate.nextGateIndex = nextGateIndex;

            return gate;
        }

        public static BlockEntity Block_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 int typeID,
                                 int index,
                                 Vector2Int pos,
                                 bool isFake,
                                 int number) {

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

            // Is Fake
            block.isFake = isFake;

            // Block Number
            block.number = number;
            block.showNumber = number != 0;
            block.numberMaterial = blockTM.numberMaterial;
            block.numberColor = blockTM.numberColor;

            // Rename
            block.gameObject.name = $"Block - {block.typeName} - {block.entityIndex}";

            // Set Pos
            block.Pos_SetPos(pos);
            block.originalPos = pos;

            // Set Material
            block.meshMaterial_default = isFake ? blockTM.fakeMaterial : blockTM.meshMaterial_default;
            block.meshMaterial_bloom = isFake ? blockTM.fakeMaterial : blockTM.meshMaterial_bloom;

            // Set Models
            for (int i = 0; i < blockTM.shapeArr.Length; i++) {
                var shapeTM = blockTM.shapeArr[i];
                var shape = new Vector2Int[shapeTM.cells.Length];
                shapeTM.ForEachCells((index, localPos) => {
                    shape[index] = localPos;
                });
                var shapeModel = new ShapeModel {
                    index = i,
                    shape = shape,
                    sizeInt = shapeTM.sizeInt,
                    centerFloat = shapeTM.GetCenterFloat()
                };
                block.shapeComponent.Add(shapeModel);
            }

            return block;
        }

        public static CellMod Cell_Spawn(bool showNumber,
                                         IDRecordService idRecordService,
                                         AssetsInfraContext assetsInfraContext,
                                         Vector2Int pos,
                                         Transform root) {

            var prefab = showNumber ?
            assetsInfraContext.Mod_GetNumberCell() :
            assetsInfraContext.Mod_GetCell();
            var cell = GameObject.Instantiate(prefab, root).GetComponent<CellMod>();
            cell.Ctor();

            // Set Pos
            cell.Pos_SetPosInt(pos);

            return cell;
        }

        public static RoleEntity Role_Spawn(TemplateInfraContext templateInfraContext,
                                 AssetsInfraContext assetsInfraContext,
                                 IDRecordService idRecordService,
                                 int typeID,
                                 Vector2 pos) {

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
            role.hpMax = roleTM.hpMax;
            role.hp = role.hpMax;
            role.typeName = roleTM.typeName;

            // Set Move
            role.moveDurationSec = roleTM.moveDurationSec;
            role.moveEasingType = roleTM.moveEasingType;
            role.moveEasingMode = roleTM.moveEasingMode;

            // Rename
            role.gameObject.name = $"Role - {role.typeName} - {role.entityID}";

            // Set Pos
            role.Pos_SetPos(pos);
            role.Pos_RecordLastFramePos();

            // Set Size
            role.Size_SetSize(roleTM.size);

            // Set FSM
            role.FSM_EnterIdle();

            // Set Mesh
            role.Mesh_Set(roleTM.mesh);
            role.Mesh_SetMaterial(roleTM.meshMaterial);

            return role;
        }

    }

}