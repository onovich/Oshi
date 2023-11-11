namespace Alter {

    public static class PanelGameInfoDomain {

        public static void Open(UIAppContext ctx) {
            var panel = UIFactory.UniquePanel_Open<Panel_GameInfo>(ctx);
            panel.Ctor();
        }

        public static void SetLevel(UIAppContext ctx, int level) {
            var panel = ctx.UniquePanel_Get<Panel_GameInfo>();
            if (panel != null) {
                panel.Level_Set(level);
            }
        }

        public static void Close(UIAppContext ctx) {
            UIFactory.UniquePanel_Close<Panel_GameInfo>(ctx);
        }

    }
}