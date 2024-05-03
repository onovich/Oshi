using UnityEngine;

namespace Oshi {

    public static class GameGameDomain {

        public static void NewGame(GameBusinessContext ctx, int mapTypeID) {

            var config = ctx.templateInfraContext.Config_Get();

            // Map
            var has = ctx.templateInfraContext.Map_TryGet(mapTypeID, out var mapTM);
            if (!has) {
                GLog.LogError($"MapTM Not Found {mapTypeID}");
            }
            var map = GameMapDomain.Spawn(ctx, mapTypeID);

            // Game
            var game = ctx.gameEntity;
            game.fsmComponent.FadingIn_Enter(config.fadingInDuration);

            // Terrain
            //// Terrain Wall
            var terrainWallTMArr = mapTM.terrainWallTMArr;
            var terrainWallSpawnPosArr = mapTM.terrainWallSpawnPosArr;
            for (int i = 0; i < terrainWallTMArr.Length; i++) {
                var terrainTM = terrainWallTMArr[i];
                var pos = terrainWallSpawnPosArr[i];
                map.Terrain_SetWall(pos, terrainTM.tileBase, terrainTM.typeID);
            }

            //// Terrain Spike
            var terrainSpikeTMArr = mapTM.terrainSpikeTMArr;
            var terrainSpikeSpawnPosArr = mapTM.terrainSpikeSpawnPosArr;
            for (int i = 0; i < terrainSpikeTMArr.Length; i++) {
                var terrainTM = terrainSpikeTMArr[i];
                var pos = terrainSpikeSpawnPosArr[i];
                map.Terrain_SetSpike(pos, terrainTM.tileBase, terrainTM.typeID);
            }

            //// Terrain Goal
            var terrainGoalTMArr = mapTM.terrainGoalTMArr;
            var terrainGoalSpawnPosArr = mapTM.terrainGoalSpawnPosArr;
            for (int i = 0; i < terrainGoalTMArr.Length; i++) {
                var terrainTM = terrainGoalTMArr[i];
                var pos = terrainGoalSpawnPosArr[i];
                map.Terrain_SetGoal(pos, terrainTM.tileBase, terrainTM.typeID);
            }

            // Role
            var player = ctx.playerEntity;

            // - Owner
            var spawnPoint = mapTM.spawnPoint;
            var owner = GameRoleDomain.Spawn(ctx,
                                             mapTM.ownerRoleTypeID,
                                             spawnPoint);
            player.ownerRoleEntityID = owner.entityID;
            ctx.ownerSpawnPoint = spawnPoint;

            // Block
            var blockTMArr = mapTM.blockTMArr;
            var blockPosArr = mapTM.blockPosArr;
            var blockIndexArr = mapTM.blockIndexArr;
            for (int i = 0; i < blockTMArr.Length; i++) {
                var blockTM = blockTMArr[i];
                var pos = blockPosArr[i];
                var index = blockIndexArr[i];
                var _ = GameBlockDomain.Spawn(ctx, blockTM.typeID, index, pos);
            }

            // Wall
            var wallTMArr = mapTM.wallTMArr;
            var wallPosArr = mapTM.wallPosArr;
            var wallIndexArr = mapTM.wallIndexArr;
            if (wallTMArr != null) {
                for (int i = 0; i < wallTMArr.Length; i++) {
                    var wallTM = wallTMArr[i];
                    var pos = wallPosArr[i];
                    var index = wallIndexArr[i];
                    var _ = GameWallDomain.Spawn(ctx, wallTM.typeID, index, pos);
                }
            }

            // Goal
            var goalTMArr = mapTM.goalTMArr;
            var goalPosArr = mapTM.goalPosArr;
            var goalIndexArr = mapTM.goalIndexArr;
            for (int i = 0; i < goalTMArr.Length; i++) {
                var goalTM = goalTMArr[i];
                var pos = goalPosArr[i];
                var index = goalIndexArr[i];
                var _ = GameGoalDomain.Spawn(ctx, goalTM.typeID, index, pos);
            }

            // Spike
            var spikeTMArr = mapTM.spikeTMArr;
            var spikePosArr = mapTM.spikePosArr;
            var spikeIndexArr = mapTM.spikeIndexArr;
            if (spikeTMArr != null) {
                for (int i = 0; i < spikeTMArr.Length; i++) {
                    var spikeTM = spikeTMArr[i];
                    var pos = spikePosArr[i];
                    var index = spikeIndexArr[i];
                    var _ = GameSpikeDomain.Spawn(ctx, spikeTM.typeID, index, pos);
                }
            }

            // Path
            var pathTMArr = mapTM.pathTMArr;
            var pathIndexArr = mapTM.pathIndexArr;
            var pathSpawnTMArr = mapTM.pathSpawnTMArr;
            var pathTravelerTypeArr = mapTM.pathTravelerTypeArr;
            var pathTravelerIndexArr = mapTM.pathTravelerIndexArr;
            var pathIsCircleLoopArr = mapTM.pathIsCircleLoopArr;
            var pathIsPingPongLoopArr = mapTM.pathIsPingPongLoopArr;
            var pathTravelerHalfSizeArr = mapTM.pathTravelerHalfSizeArr;
            if (pathTMArr != null) {
                for (int i = 0; i < pathTMArr.Length; i++) {
                    var pathTM = pathTMArr[i];
                    if (pathTM == null) {
                        GLog.LogError($"PathTM Not Found: {i}");
                    }
                    var index = pathIndexArr[i];
                    var spawn = pathSpawnTMArr[i];
                    var travelerType = pathTravelerTypeArr[i];
                    var travelerIndex = pathTravelerIndexArr[i];
                    var isCircleLoop = pathIsCircleLoopArr[i];
                    var isPingPongLoop = pathIsPingPongLoopArr[i];
                    var travelerHalfSize = pathTravelerHalfSizeArr[i];
                    var _ = GamePathDomain.Spawn(ctx,
                                                 pathTM.typeID,
                                                 index,
                                                 isCircleLoop,
                                                 isPingPongLoop,
                                                 spawn.pathNodeArr,
                                                 travelerType,
                                                 travelerIndex,
                                                 travelerHalfSize);
                }
            }

            // Camera
            CameraApp.Init(ctx.cameraContext, owner.transform, Vector2.zero, mapTM.cameraConfinerWorldMax, mapTM.cameraConfinerWorldMin);

            // UI
            UIApp.GameInfo_Open(ctx.uiContext, mapTM.typeName, map.limitedByTime, map.limitedByStep);

            // Cursor

        }

        public static void ApplyGameOver(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;
            if (fsm.gameOver_isEntering) {
                fsm.GameOver_Enter(fsm.mapOver_result);
                fsm.gameOver_isEntering = false;
            }
        }

        public static void ApplyMapOver(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;

            fsm.MapOver_DecTimer(dt);
            var enterTime = fsm.mapOver_enterTime;
            var map = ctx.currentMapEntity;
            if (enterTime <= 0) {
                // Game Over
                if (map.isLastMap) {
                    fsm.GameOver_Enter(fsm.mapOver_result);
                    return;
                }
                // Fading Out
                var config = ctx.templateInfraContext.Config_Get();
                fsm.FadingOut_Enter(config.fadingOutDuration, config.fadingOutEasingType, config.fadingOutEasingMode, fsm.mapOver_result);
            }
        }

        public static void RestartGame(GameBusinessContext ctx) {
            var mapTypeID = ctx.currentMapEntity.typeID;
            ExitGame(ctx);
            NewGame(ctx, mapTypeID);
        }

        public static void ApplyManualRestartGame(GameBusinessContext ctx) {
            var input = ctx.inputEntity;
            if (input.isPressRestart) {
                var game = ctx.gameEntity;
                var fsm = game.fsmComponent;
                var config = ctx.templateInfraContext.Config_Get();
                fsm.FadingOut_Enter(config.manualFadingOutDuration, config.manualFadingOutEasingType, config.manualFadingOutEasingMode, GameResult.Lose);
            }
        }

        public static void NextLevel(GameBusinessContext ctx) {
            var game = ctx.gameEntity;
            var mapTypeID = ctx.currentMapEntity.nextMapTypeID;
            ExitGame(ctx);
            NewGame(ctx, mapTypeID);
        }

        public static void ApplyCheckGameResult(GameBusinessContext ctx) {
            var owner = ctx.Role_GetOwner();
            var game = ctx.gameEntity;
            var config = ctx.templateInfraContext.Config_Get();

            // Check Role Unspawned
            var dead = CheckOwnerUnspawned(ctx);
            if (dead) {
                game.fsmComponent.MapOver_Enter(config.gameResetEnterTime, GameResult.Lose);
                return;
            }

            // Check Time Finish
            var timeFinish = CheckTimeFinish(ctx, Time.deltaTime);
            if (timeFinish) {
                game.fsmComponent.MapOver_Enter(config.gameResetEnterTime, GameResult.Lose);
                return;
            }

            // Check Step
            var stepFinish = CheckStepFinish(ctx);
            if (stepFinish) {
                game.fsmComponent.MapOver_Enter(config.gameResetEnterTime, GameResult.Lose);
                return;
            }

            // Check Goal
            var inGoal = CheckInGoal(ctx);
            if (inGoal) {
                game.fsmComponent.MapOver_Enter(config.gameResetEnterTime, GameResult.Win);
            }
        }

        static bool CheckOwnerUnspawned(GameBusinessContext ctx) {
            var owner = ctx.Role_GetOwner();
            if (owner == null) {
                return true;
            }
            return false;
        }

        static bool CheckInGoal(GameBusinessContext ctx) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;

            bool inGoal = true;
            var config = ctx.templateInfraContext.Config_Get();

            ctx.blockRepo.ForEach((block) => {
                inGoal &= GameBlockDomain.CheckAllInGoal(ctx, block);
            });
            return inGoal;
        }

        public static bool CheckTimeFinish(GameBusinessContext ctx, float dt) {
            var map = ctx.currentMapEntity;
            if (!map.limitedByTime) {
                return false;
            }
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;

            map.DecTimer(dt);
            var time = map.gameTotalTime;

            return time <= 0;
        }

        public static bool CheckStepFinish(GameBusinessContext ctx) {
            var map = ctx.currentMapEntity;
            if (!map.limitedByStep) {
                return false;
            }
            var totalStep = map.gameTotalStep;
            var owner = ctx.Role_GetOwner();
            var step = owner.step;

            return totalStep - step <= 0;
        }

        public static void ExitGame(GameBusinessContext ctx) {
            // Game
            var game = ctx.gameEntity;
            game.fsmComponent.NotInGame_Enter();

            // Map
            GameMapDomain.UnSpawn(ctx);

            // Role
            int roleLen = ctx.roleRepo.TakeAll(out var roleArr);
            for (int i = 0; i < roleLen; i++) {
                var role = roleArr[i];
                GameRoleDomain.UnSpawn(ctx, role);
            }

            // Block
            int blockLen = ctx.blockRepo.TakeAll(out var blockArr);
            for (int i = 0; i < blockLen; i++) {
                var block = blockArr[i];
                GameBlockDomain.UnSpawn(ctx, block);
            }

            // Wall
            int wallLen = ctx.wallRepo.TakeAll(out var wallArr);
            for (int i = 0; i < wallLen; i++) {
                var wall = wallArr[i];
                GameWallDomain.UnSpawn(ctx, wall);
            }

            // Goal
            int goalLen = ctx.goalRepo.TakeAll(out var goalArr);
            for (int i = 0; i < goalLen; i++) {
                var goal = goalArr[i];
                GameGoalDomain.UnSpawn(ctx, goal);
            }

            // Spike
            int spikeLen = ctx.spikeRepo.TakeAll(out var spikeArr);
            for (int i = 0; i < spikeLen; i++) {
                var spike = spikeArr[i];
                GameSpikeDomain.UnSpawn(ctx, spike);
            }

            // Path
            int pathLen = ctx.pathRepo.TakeAll(out var pathArr);
            for (int i = 0; i < pathLen; i++) {
                var path = pathArr[i];
                GamePathDomain.UnSpawn(ctx, path);
            }

            // Repo
            ctx.Reset();

            // UI
            UIApp.GameOver_Close(ctx.uiContext);
            UIApp.GameInfo_Close(ctx.uiContext);

        }

    }
}