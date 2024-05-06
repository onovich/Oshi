namespace Oshi {

    public static class DBInfra {

        public static void GameStage_Save(DBInfraContext ctx, DBGameStageModel model) {
            DBGameStageDomain.Save(ctx, model);
        }

        public static DBGameStageModel GameStage_Load(DBInfraContext ctx) {
            return DBGameStageDomain.Load(ctx);
        }

    }

}