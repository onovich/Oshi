using UnityEngine;

namespace Oshi {

    public static class GameBusiness {

        public static void Init(GameBusinessContext ctx) {

        }

        public static void EnterLogin(GameBusinessContext ctx) {
            // GameStage
            var loadSucc = GameSaveDomain.GameStage_TryLoad(ctx, out var gameStage);
            // UI
            UIApp.Login_Open(ctx.uiContext, loadSucc);
            // BGM
            var soundTable = ctx.templateInfraContext.SoundTable_Get();
            SoundApp.BGM_PlayLoop(ctx.soundContext, soundTable.bgmLoop[0], 0, soundTable.bgmVolume[0], true);
        }

        public static void ExitLogin(GameBusinessContext ctx) {
            UIApp.Login_Close(ctx.uiContext);
        }

        public static void ExitApplication(GameBusinessContext ctx) {
            ExitLogin(ctx);
            Application.Quit();
        }

        public static void Login_OnLoadGameClick(GameBusinessContext ctx) {
            var gameStage = ctx.gameStageEntity;
            EnterGame(ctx, true, gameStage.lastPlayedMapTypeID);
        }

        public static void EnterGame(GameBusinessContext ctx, bool isForceEnter, int testMapTypeID) {
            var config = ctx.templateInfraContext.Config_Get();
            var mapTypeID =
            isForceEnter ? testMapTypeID :
            config.originalMapTypeID;

            var soundTable = ctx.templateInfraContext.SoundTable_Get();
            SoundApp.BGM_PlayLoop(ctx.soundContext, soundTable.bgmLoop[1], 1, soundTable.bgmVolume[0], true);
            SoundApp.BGM_PlayLoop(ctx.soundContext, soundTable.bgmLoop[2], 2, soundTable.bgmVolume[0], true);
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
            var owner = ctx.Role_GetOwner();

            if (status == GameStatus.PlayerTurn) {

                if (game.fsmComponent.playerTurn_isEntering) {
                    game.fsmComponent.playerTurn_isEntering = false;
                }
                // Owner
                GameRoleFSMController.FixedTickFSM(ctx, owner, dt, () => {
                    game.fsmComponent.EnvirTurn_Enter();
                });
            } else if (status == GameStatus.EnvirTurn) {
                if (game.fsmComponent.envirTurn_isEntering) {
                    game.fsmComponent.envirTurn_isEntering = false;
                }
                // Block
                var blockLen = ctx.blockRepo.TakeAll(out var blockArr);
                for (int i = 0; i < blockLen; i++) {
                    var block = blockArr[i];
                    GameBlockDomain.CheckAndResetBlock(ctx, block);
                }
                // Gate
                var gateLen = ctx.gateRepo.TakeAll(out var gateArr);
                for (int i = 0; i < gateLen; i++) {
                    var gate = gateArr[i];
                    GameGateDomain.CheckAndResetGate(ctx, gate);
                }
                // Goal
                var goalLen = ctx.goalRepo.TakeAll(out var goalArr);
                for (int i = 0; i < goalLen; i++) {
                    var goal = goalArr[i];
                    GameGoalDomain.CheckAndResetGoal(ctx, goal);
                }
                // Path
                var pathMoveDone = true;
                var pathLen = ctx.pathRepo.TakeAll(out var pathArr);
                for (int i = 0; i < pathLen; i++) {
                    var path = pathArr[i];
                    GamePathDomain.ApplyMoving(ctx, path, dt, out var isEnd);
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
                // Result
                GameGameDomain.ApplyCheckGameResult(ctx);
            } else if (status == GameStatus.GameOver) {
                GameGameDomain.ApplyGameOver(ctx, dt);
            } else if (status == GameStatus.MapOver) {
                GameGameDomain.ApplyMapOver(ctx, dt);
            }

            GameRoleDomain.CheckAndApplyAllRoleDead(ctx);
            GameGameDomain.ApplyManualRestartGame(ctx);
            GameGameDomain.ApplyManualExitApplication(ctx);

            // Roles
            var roleLen = ctx.roleRepo.TakeAll(out var roleArr);
            for (int i = 0; i < roleLen; i++) {
                var role = roleArr[i];
                GameRoleDomain.CheckAndUnSpawn(ctx, role);
            }
        }

        static void FixedTick(GameBusinessContext ctx, float dt) {
            Physics2D.Simulate(dt);
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
            }

            if (status != GameStatus.NotInGame) {
                // Camera
                CameraApp.LateTick(ctx.cameraContext, dt);

                // VFX
                VFXApp.LateTick(ctx.vfxContext, dt);
            }

            if (status == GameStatus.PlayerTurn || status == GameStatus.EnvirTurn) {
                // Record
                GameRecordDomain.UndoRecord(ctx);
            }

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
            GameGameDomain.ExitApplication(ctx);
            GLog.Log("Application.Quit");
        }

    }

}