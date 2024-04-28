namespace Oshi {

    public static class GameMapDomain {

        public static MapEntity Spawn(GameBusinessContext ctx, int typeID) {
            var map = GameFactory.Map_Spawn(ctx.templateInfraContext, ctx.assetsInfraContext, typeID);
            ctx.currentMapEntity = map;
            return map;
        }

        public static void UnSpawn(GameBusinessContext ctx) {
            var map = ctx.currentMapEntity;
            map.TearDown();
            ctx.currentMapEntity = null;
        }

#if UNITY_EDITOR
        public static void OnDrawGizmos(GameBusinessContext ctx) {
            var map = ctx.currentMapEntity;
            if (map == null) {
                return;
            }
            map.DrawGizmos();
        }
#endif

    }

}