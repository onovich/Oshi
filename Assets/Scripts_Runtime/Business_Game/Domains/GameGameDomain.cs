using UnityEngine;

namespace Alter {

    public static class GameGameDomain {

        public static void NewGame(GameBusinessContext ctx) {

            var config = ctx.templateInfraContext.Config_Get();

            // Game
            var game = ctx.gameEntity;
            game.fsmComponent.Gaming_Enter(config.gameTotalTime);

            // Map
            var mapTypeID = config.originalMapTypeID;
            var map = GameMapDomain.Spawn(ctx, mapTypeID);
            var has = ctx.templateInfraContext.Map_TryGet(mapTypeID, out var mapTM);
            if (!has) {
                GLog.LogError($"MapTM Not Found {mapTypeID}");
            }

            // Role
            var player = ctx.playerEntity;

            // - Owner
            var spawnPoint = mapTM.spawnPoint;
            var owner = GameRoleDomain.Spawn(ctx,
                                             config.ownerRoleTypeID,
                                             spawnPoint);
            player.ownerRoleEntityID = owner.entityID;
            ctx.ownerSpawnPoint = spawnPoint;

            // Block
            var blockTMArr = mapTM.blockTMArr;
            var blockPosArr = mapTM.blockPosArr;
            var blockSizeArr = mapTM.blockSizeArr;
            var blockIndexArr = mapTM.blockIndexArr;
            for (int i = 0; i < blockTMArr.Length; i++) {
                var blockTM = blockTMArr[i];
                var pos = blockPosArr[i];
                var size = blockSizeArr[i];
                var index = blockIndexArr[i];
                var _ = GameBlockDomain.Spawn(ctx, blockTM.typeID, index, pos, size);
            }

            // Wall
            var wallTMArr = mapTM.wallTMArr;
            var wallPosArr = mapTM.wallPosArr;
            var wallSizeArr = mapTM.wallSizeArr;
            var wallIndexArr = mapTM.wallIndexArr;
            for (int i = 0; i < wallTMArr.Length; i++) {
                var wallTM = wallTMArr[i];
                var pos = wallPosArr[i];
                var size = wallSizeArr[i];
                var index = wallIndexArr[i];
                var _ = GameWallDomain.Spawn(ctx, wallTM.typeID, index, pos, size);
            }

            // Goal
            var goalTMArr = mapTM.goalTMArr;
            var goalPosArr = mapTM.goalPosArr;
            var goalSizeArr = mapTM.goalSizeArr;
            var goalIndexArr = mapTM.goalIndexArr;
            for (int i = 0; i < goalTMArr.Length; i++) {
                var goalTM = goalTMArr[i];
                var pos = goalPosArr[i];
                var size = goalSizeArr[i];
                var index = goalIndexArr[i];
                var _ = GameGoalDomain.Spawn(ctx, goalTM.typeID, index, pos, size);
            }

            // Camera
            CameraApp.Init(ctx.cameraContext, owner.transform, Vector2.zero, mapTM.cameraConfinerWorldMax, mapTM.cameraConfinerWorldMin);

            // UI
            UIApp.GameInfo_Open(ctx.uiContext);

            // Cursor

        }

        public static void ApplyGameOver(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;

            fsm.GameOver_DecTimer(dt);

            var enterTime = fsm.gameOver_enterTime;
            if (enterTime <= 0) {
                UIApp.GameOver_Open(ctx.uiContext, fsm.gameOver_result);
            }
        }

        public static void RestartGame(GameBusinessContext ctx) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;
            ExitGame(ctx);
            NewGame(ctx);
        }

        public static void ApplyGameResult(GameBusinessContext ctx) {
            var owner = ctx.Role_GetOwner();
            if (owner.fsmCom.status != RoleFSMStatus.Idle) {
                return;
            }

            var game = ctx.gameEntity;
            var config = ctx.templateInfraContext.Config_Get();

            // Check Role Dead
            var dead = CheckRoleDead(ctx);
            if (dead) {
                game.fsmComponent.GameOver_Enter(config.gameResetEnterTime, GameResult.Lose);
                return;
            }

            // Check Time Finish
            var timeFinish = CheckTimeFinish(ctx, Time.deltaTime);
            if (timeFinish) {
                game.fsmComponent.GameOver_Enter(config.gameResetEnterTime, GameResult.Lose);
                return;
            }

            // Check Goal
            var inGoal = CheckGoal(ctx);
            if (inGoal) {
                game.fsmComponent.GameOver_Enter(config.gameResetEnterTime, GameResult.Win);
            }

        }

        static bool CheckRoleDead(GameBusinessContext ctx) {
            var owner = ctx.Role_GetOwner();
            if (owner == null || owner.needTearDown) {
                return true;
            }
            return false;
        }

        static bool CheckGoal(GameBusinessContext ctx) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;

            bool inGoal = true;
            var config = ctx.templateInfraContext.Config_Get();

            ctx.Block_ForEach((block) => {
                inGoal &= GameBlockDomain.ApplyCheckGoal(ctx, block);
            });
            return inGoal;
        }

        public static bool CheckTimeFinish(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;

            fsm.Gaming_DecTimer(dt);
            var time = fsm.gaming_gameTime;

            var config = ctx.templateInfraContext.Config_Get();
            return time <= 0;
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

            // Repo
            ctx.Reset();

            // UI
            UIApp.GameOver_Close(ctx.uiContext);
            UIApp.GameInfo_Close(ctx.uiContext);

        }

    }
}