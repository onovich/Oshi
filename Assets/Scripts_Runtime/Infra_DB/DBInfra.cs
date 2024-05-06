namespace Oshi {

    public static class DBInfra {

        public static string GameStage_Save(DBInfraContext ctx, DBGameStageModel model) {
            return DBGameStageDomain.Save(ctx, model);
        }

        public static bool GameStage_TryLoad(DBInfraContext ctx, out DBGameStageModel model) {
            return DBGameStageDomain.TryLoad(ctx, out model);
        }

        public static void TearDown(DBInfraContext ctx) {
            ctx.Clear();
        }

    }

}