using System.Collections.Generic;

namespace Oshi {

    public static class GameStageDomain {

        public static void TryUnlock(GameBusinessContext ctx, int mapTypeID) {
            var gameStage = ctx.gameStageEntity;
            if (!gameStage.HasUnlocked(mapTypeID)) {
                gameStage.unlockedMapTypeIDList.Add(mapTypeID);
                GameSaveDomain.GameStage_Save(ctx);
            }
        }

        public static void SetLastPlayedMapTypeID(GameBusinessContext ctx, int mapTypeID) {
            ctx.gameStageEntity.lastPlayedMapTypeID = mapTypeID;
            GameSaveDomain.GameStage_Save(ctx);
        }

        public static int GetLastPlayedMapTypeID(GameBusinessContext ctx) {
            return ctx.gameStageEntity.lastPlayedMapTypeID;
        }

        public static List<int> GetUnlockedMapTypeIDList(GameBusinessContext ctx) {
            return ctx.gameStageEntity.unlockedMapTypeIDList;
        }

    }

}