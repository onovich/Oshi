using UnityEngine;

namespace Chouten {

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

                // Game
                GameGameDomain.ApplyGameTime(ctx, dt);

                // Wave
                GameWaveDomain.ApplySpawnWaveEnemies(ctx, map, dt);

                // Roles
                var roleLen = ctx.roleRepo.TakeAll(out var roleArr);
                for (int i = 0; i < roleLen; i++) {
                    var role = roleArr[i];
                    GameRoleDomain.CheckAndUnSpawn(ctx, role);
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

                Physics2D.Simulate(fixdt);
            }
        }

        static void LateTick(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var status = game.fsmComponent.status;
            var owner = ctx.Role_GetOwner();
            if (status == GameStatus.Gaming) {

                // Camera
                CameraApp.LateTick(ctx.cameraContext, dt);

                // UI
                UIApp.GameInfo_RefreshTime(ctx.uiContext, game.fsmComponent.gaming_gameTime);
                UIApp.GameInfo_RefreshHP(ctx.uiContext, owner.hp);

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

        public static void OnDrawGizmos(GameBusinessContext ctx, bool drawCameraGizmos) {
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

        // UI
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