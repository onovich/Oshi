using MortiseFrame.Capsule;

namespace Oshi {

    public static class DBGameStageDomain {

        public static string Save(DBInfraContext ctx, DBGameStageModel model) {
            return ctx.saveCore.Save(model);
        }

        public static bool TryLoad(DBInfraContext ctx, out DBGameStageModel model) {
            var key = ctx.GetKey(typeof(DBGameStageModel));
            var succ = ctx.saveCore.TryLoad(key, out ISave save);
            model = default;
            if (succ) {
                model = (DBGameStageModel)save;
            }
            return succ;
        }

    }

}