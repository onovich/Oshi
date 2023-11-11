using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alter {

    public static class GameBusiness {

        public static void Init(GameBusinessContext ctx) {

            ctx.gameEntity = new GameEntity();

            var inputEntity = ctx.inputEntity;
            inputEntity.Ctor();
            inputEntity.Keybinding_Set(InputKeyEnum.MoveLeft, new KeyCode[] { KeyCode.A });
            inputEntity.Keybinding_Set(InputKeyEnum.MoveRight, new KeyCode[] { KeyCode.D });
            inputEntity.Keybinding_Set(InputKeyEnum.MoveUp, new KeyCode[] { KeyCode.W });
            inputEntity.Keybinding_Set(InputKeyEnum.MoveDown, new KeyCode[] { KeyCode.S });

        }

        static void RestartGame(GameBusinessContext ctx) {

            // Tear Down

            // - Block

            // Start Game
            StartGame(ctx);

        }

        public static void StartGame(GameBusinessContext ctx) {

            ctx.gameEntity.random = new System.Random(101052099);
            ctx.gameEntity.intervalTime = 0.01f;
            ctx.gameEntity.stage = GameStage.Prepare;

            // UI
            UIApp.GameInfo_Open(ctx.uiAppContext);

        }

        public static void Tick(GameBusinessContext ctx, float dt) {

            var gameEntity = ctx.gameEntity;
            var player = ctx.playerEntity;
            if (player == null) {
                return;
            }

            ProcessInput(ctx, dt);

            LogicTick(ctx, dt);

            float restTime = dt;
            float intervalTime = gameEntity.intervalTime;
            for (; restTime >= intervalTime; restTime -= intervalTime) {
                LogicFixedTick(ctx, intervalTime);
            }
            LogicFixedTick(ctx, restTime);

            LateTick(ctx, dt);

        }

        static void ProcessInput(GameBusinessContext ctx, float dt) {
            InputEntity inputEntity = ctx.inputEntity;
            inputEntity.ProcessInput(dt);
        }

        static void LogicTick(GameBusinessContext ctx, float dt) {

            // Game
            var player = ctx.playerEntity;

            // Block

            // UI

        }

        static void LogicFixedTick(GameBusinessContext ctx, float fixdt) {

            // - Battle Stage
            var stage = ctx.gameEntity.stage;
            if (stage == GameStage.Proceccing) {
                GameStageDomain.ApplyResult(ctx);
            }

            // - Block
            ctx.Block_ForEach(block => {
                // TODO: 
                // 1. 掉落
                // 2. 消除
            });

            Physics2D.Simulate(fixdt);
        }

        static void LateTick(GameBusinessContext ctx, float dt) {

            // Owner UI
            // UIApp.GameInfo_SetLevel(ctx.uiAppContext, level);

            // Block - 移除

            InputEntity inputEntity = ctx.inputEntity;
            inputEntity.Reset();

        }

        // ==== UI Event ====
        // - UI Event: GameResult
        public static void OnUIGameResultClickRestart(GameBusinessContext ctx) {
            // UIApp.GameResult_Close(ctx.uiAppContext);
            // RestartGame(ctx);
        }

    }

}