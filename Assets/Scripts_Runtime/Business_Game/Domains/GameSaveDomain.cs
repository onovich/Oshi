namespace Oshi {

    public static class GameSaveDomain {

        public static void GameStage_Save(GameBusinessContext ctx) {
            var gameStage = ctx.gameStageEntity;
            var model = gameStage.Save();
            DBInfra.GameStage_Save(ctx.dbInfraContext, model);
        }

        public static GameStageEntity GameStage_Load(GameBusinessContext ctx) {
            var model = DBInfra.GameStage_Load(ctx.dbInfraContext);
            var gameStage = ctx.gameStageEntity;
            gameStage.Load(model);
            return gameStage;
        }

    }

}