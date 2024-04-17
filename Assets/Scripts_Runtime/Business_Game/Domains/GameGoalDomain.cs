using UnityEngine;

namespace Alter {

    public static class GameGoalDomain {

        public static GoalEntity Spawn(GameBusinessContext ctx, int typeID, int index, Vector2Int pos, Vector2Int size) {
            var goal = GameFactory.Goal_Spawn(ctx.templateInfraContext,
                                              ctx.assetsInfraContext,
                                              typeID,
                                              index,
                                              pos,
                                              size);

            ctx.goalRepo.Add(goal);
            return goal;
        }

        public static void UnSpawn(GameBusinessContext ctx, GoalEntity goal) {
            ctx.goalRepo.Remove(goal);
            goal.TearDown();
        }

    }

}