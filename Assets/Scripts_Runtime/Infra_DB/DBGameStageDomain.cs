namespace Oshi {

    public static class DBGameStageDomain {

        public static void Save(DBInfraContext ctx, DBGameStageModel model) {
            ctx.saveCore.Save(model);
        }

        public static DBGameStageModel Load(DBInfraContext ctx) {
            var key = ctx.GetKey(typeof(DBGameStageModel));
            return (DBGameStageModel)ctx.saveCore.Load(key);
        }

    }

}