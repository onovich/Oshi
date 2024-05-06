using System.Collections.Generic;
using UnityEngine;

namespace Oshi {

    public static class GameStageDomain {

        public static void TryUnlock(GameBusinessContext ctx, int mapTypeID) {
            var gameStage = ctx.gameStageEntity;
            if (!gameStage.HasUnlocked(mapTypeID)) {
                gameStage.unlockedMapTypeIDList.Add(mapTypeID);
            }
        }

        public static void SetLastPlayedMapTypeID(GameBusinessContext ctx, int mapTypeID) {
            ctx.gameStageEntity.lastPlayedMapTypeID = mapTypeID;
        }

        public static List<int> GetUnlockedMapTypeIDList(GameBusinessContext ctx) {
            return ctx.gameStageEntity.unlockedMapTypeIDList;
        }

    }

}