using UnityEngine;

namespace Oshi {

    public static class GameBusiness {

        public static void Init(GameBusinessContext ctx) {

        }

        public static void StartGame(GameBusinessContext ctx) {
            GameGameDomain.NewGame(ctx);
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
            if (status == GameStatus.Gaming) {
                GameInputDomain.Owner_BakeInput(ctx, ctx.Role_GetOwner());
            }
        }

        static void PreTick(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            var map = ctx.currentMapEntity;
            if (status == GameStatus.Gaming) {

                // Roles
                var roleLen = ctx.roleRepo.TakeAll(out var roleArr);
                for (int i = 0; i < roleLen; i++) {
                    var role = roleArr[i];
                    GameRoleDomain.CheckAndUnSpawn(ctx, role);
                }

                // Block
                var blockLen = ctx.blockRepo.TakeAll(out var blockArr);
                for (int i = 0; i < blockLen; i++) {
                    var block = blockArr[i];
                    GameBlockDomain.CheckAndResetBlock(ctx, block);
                }

                // Result
                GameGameDomain.ApplyGameResult(ctx);
            }
            if (status == GameStatus.GameOver) {
                GameGameDomain.ApplyGameOver(ctx, dt);
            }
        }

        static void FixedTick(GameBusinessContext ctx, float fixdt) {
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            if (status == GameStatus.Gaming) {
                // Roles
                var roleLen = ctx.roleRepo.TakeAll(out var roleArr);
                for (int i = 0; i < roleLen; i++) {
                    var role = roleArr[i];
                    GameRoleFSMController.FixedTickFSM(ctx, role, fixdt);
                }

                // Path
                var pathLen = ctx.pathRepo.TakeAll(out var pathArr);
                for (int i = 0; i < pathLen; i++) {
                    var path = pathArr[i];
                    GamePathFSMController.FixedTickFSM(ctx, path, fixdt);
                }

                // Path Carry
                for (int i = 0; i < pathLen; i++) {
                    var path = pathArr[i];
                    GamePathDomain.ApplyCarryTraveler(ctx, path);
                }

                Physics2D.Simulate(fixdt);
            }
        }

        static void LateTick(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            var owner = ctx.Role_GetOwner();
            var map = ctx.currentMapEntity;
            if (status == GameStatus.Gaming) {

                // Camera
                CameraApp.LateTick(ctx.cameraContext, dt);

                // Game Info
                var totalStep = map.gameTotalStep;
                var showTime = map.limitedByTime;
                var step = owner.step;
                var showStep = map.limitedByStep;

                // UI
                if (showTime) UIApp.GameInfo_RefreshTime(ctx.uiContext, game.fsmComponent.gaming_gameTime);
                if (showStep) UIApp.GameInfo_RefreshStep(ctx.uiContext, totalStep - step);
            }
            // VFX
            VFXApp.LateTick(ctx.vfxContext, dt);
        }

        public static void TearDown(GameBusinessContext ctx) {
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            if (status == GameStatus.Gaming) {
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
            if (status == GameStatus.Gaming) {
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
            if (status == GameStatus.Gaming) {
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

        public static void UIGameOver_OnExitGameClick(GameBusinessContext ctx) {
            ExitGame(ctx);
            Application.Quit();
            GLog.Log("Application.Quit");
        }

    }

}