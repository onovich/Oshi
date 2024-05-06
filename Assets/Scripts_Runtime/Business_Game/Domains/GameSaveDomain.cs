
using UnityEngine;

namespace Oshi {

    public static class GameSaveDomain {

        public static void GameStage_Save(GameBusinessContext ctx) {
            var gameStage = ctx.gameStageEntity;
            var model = gameStage.Save();
            var path = DBInfra.GameStage_Save(ctx.dbInfraContext, model);
            Debug.Log($"GameStage saved to {path}");
        }

        public static bool GameStage_TryLoad(GameBusinessContext ctx, out GameStageEntity gameStage) {
            var succ = DBInfra.GameStage_TryLoad(ctx.dbInfraContext, out DBGameStageModel model);
            gameStage = ctx.gameStageEntity;
            if (succ) {
                gameStage.Load(model);
            }
            return succ;
        }

    }

}