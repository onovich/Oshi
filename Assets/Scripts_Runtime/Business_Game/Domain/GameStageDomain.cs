namespace Alter {

    public static class GameStageDomain {

        public static void ApplyResult(GameBusinessContext ctx) {

            var game = ctx.gameEntity;

            var stage = game.stage;
            if (stage != GameStage.Proceccing) {
                return;
            }

            // TODO: Apply Result

        }

    }

}