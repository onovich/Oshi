using UnityEngine;

namespace Oshi {

    public static class GameBusiness {

        public static void Init(GameBusinessContext ctx) {

        }

        public static void StartGame(GameBusinessContext ctx, bool isTestMode) {
            var config = ctx.templateInfraContext.Config_Get();
            var mapTypeID = isTestMode ? config.testMapTypeID : config.originalMapTypeID;
            GameGameDomain.NewGame(ctx, mapTypeID);
        }

        public static void ExitGame(GameBusinessContext ctx) {
            GameGameDomain.ExitGame(ctx);
        }

        public static void Tick(GameBusinessContext ctx, float dt) {

            InputEntity inputEntity = ctx.inputEntity;

            ProcessInput(ctx, dt);
            PreTick(ctx, dt);

            const float intervalTime = 0.01f;
            ref float restSec = ref ctx.fixedRestSec;
            restSec += dt;
            if (restSec < intervalTime) {
                FixedTick(ctx, restSec);
                restSec = 0;
            } else {
                while (restSec >= intervalTime) {
                    restSec -= intervalTime;
                    FixedTick(ctx, intervalTime);
                }
            }
            LateTick(ctx, dt);
            inputEntity.Reset();

        }

        public static void ProcessInput(GameBusinessContext ctx, float dt) {
            GameInputDomain.Player_BakeInput(ctx, dt);

            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            if (status == GameStatus.PlayerTurn) {
                GameInputDomain.Owner_BakeInput(ctx, ctx.Role_GetOwner());
            }
        }

        static void PreTick(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            var map = ctx.currentMapEntity;
            if (status == GameStatus.EnvirTurn) {
                // Block
                var blockLen = ctx.blockRepo.TakeAll(out var blockArr);
                for (int i = 0; i < blockLen; i++) {
                    var block = blockArr[i];
                    GameBlockDomain.CheckAndResetBlock(ctx, block);
                }
            }
            if (status == GameStatus.GameOver) {
                GameGameDomain.ApplyGameOver(ctx, dt);
            }
            if (status == GameStatus.MapOver) {
                GameGameDomain.ApplyMapOver(ctx, dt);
            }
            GameGameDomain.ApplyManualRestartGame(ctx);
        }

        static void FixedTick(GameBusinessContext ctx, float fixdt) {
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            if (status == GameStatus.PlayerTurn) {
                // Owner
                var owner = ctx.Role_GetOwner();
                GameRoleFSMController.FixedTickFSM(ctx, owner, fixdt, () => {
                    game.fsmComponent.EnvirTurn_Enter();
                });
            } else if (status == GameStatus.EnvirTurn) {
                // Path
                var pathMoveDone = true;
                var pathLen = ctx.pathRepo.TakeAll(out var pathArr);
                for (int i = 0; i < pathLen; i++) {
                    var path = pathArr[i];
                    GamePathDomain.ApplyMoving(ctx, path, fixdt, out var isEnd);
                    pathMoveDone &= isEnd;
                }
                // Path Carry
                for (int i = 0; i < pathLen; i++) {
                    var path = pathArr[i];
                    GamePathDomain.ApplyCarryTraveler(ctx, path);
                }
                if (pathMoveDone) {
                    game.fsmComponent.PlayerTurn_Enter();
                }
            }
            GameRoleDomain.CheckAndApplyAllRoleDead(ctx);

            // Roles
            var roleLen = ctx.roleRepo.TakeAll(out var roleArr);
            for (int i = 0; i < roleLen; i++) {
                var role = roleArr[i];
                GameRoleDomain.CheckAndUnSpawn(ctx, role);
            }

            Physics2D.Simulate(fixdt);
        }

        static void LateTick(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            var owner = ctx.Role_GetOwner();
            var map = ctx.currentMapEntity;

            if (status == GameStatus.FadingIn) {
                GamePPDomain.ApplyFadingIn(ctx, dt);
            }

            if (status == GameStatus.FadingOut) {
                GamePPDomain.ApplyFadingOut(ctx, dt, (result) => {
                    // Restart
                    if (result == GameResult.Lose) {
                        GameGameDomain.RestartGame(ctx);
                    }
                    // Next Level
                    if (result == GameResult.Win) {
                        GameGameDomain.NextLevel(ctx);
                    }
                });
            }

            if (status == GameStatus.PlayerTurn) {
                // Camera
                CameraApp.LateTick(ctx.cameraContext, dt);

                // Game Info
                var totalStep = map.gameTotalStep;
                var showTime = map.limitedByTime;
                var step = owner.step;
                var showStep = map.limitedByStep;

                // UI
                if (showTime) UIApp.GameInfo_RefreshTime(ctx.uiContext, map.gameTotalTime);
                if (showStep) UIApp.GameInfo_RefreshStep(ctx.uiContext, totalStep - step);
            }

            if (status == GameStatus.EnvirTurn) {
                // Block Bloom
                var len = ctx.blockRepo.TakeAll(out var blockArr);
                for (int i = 0; i < len; i++) {
                    var block = blockArr[i];
                    GameBlockDomain.ApplyBloom(ctx, block);
                }
                // Result
                GameGameDomain.ApplyCheckGameResult(ctx);
            }

            // VFX
            VFXApp.LateTick(ctx.vfxContext, dt);
        }

        public static void TearDown(GameBusinessContext ctx) {
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            if (status == GameStatus.EnvirTurn) {
                ExitGame(ctx);
            }
        }

#if UNITY_EDITOR
        public static void OnDrawCameraGizmos(GameBusinessContext ctx, bool drawCameraGizmos) {
            if (ctx == null) {
                return;
            }
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            if (status != GameStatus.NotInGame) {
                if (drawCameraGizmos) {
                    CameraApp.OnDrawGizmos(ctx.cameraContext);
                }
            }
        }

        public static void OnDrawMapGizmos(GameBusinessContext ctx, bool drawMapGizmos) {
            if (ctx == null) {
                return;
            }
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            if (status != GameStatus.NotInGame) {
                if (drawMapGizmos) {
                    GameMapDomain.OnDrawGizmos(ctx);
                }
            }
        }
#endif

        // UI
        public static void UIGameInfo_OnRestartBtnClick(GameBusinessContext ctx) {
            GameGameDomain.RestartGame(ctx);
        }

        public static void UIGameOver_OnRestartGame(GameBusinessContext ctx) {
            UIApp.GameOver_Close(ctx.uiContext);
            GameGameDomain.RestartGame(ctx);
        }

        public static void UIGameOver_OnNextLevelClick(GameBusinessContext ctx) {
            UIApp.GameOver_Close(ctx.uiContext);
            GameGameDomain.NextLevel(ctx);
        }

        public static void UIGameOver_OnExitGameClick(GameBusinessContext ctx) {
            ExitGame(ctx);
            Application.Quit();
            GLog.Log("Application.Quit");
        }

    }

}