using System;
using UnityEngine;
using UnityEngine.UI;

namespace Alter.UI {

    public static class PanelLoginDomain {

        public static void Open(UIAppContext ctx) {

            Panel_Login panel = ctx.uiCore.UniquePanel_Open<Panel_Login>();
            panel.Ctor();

            panel.OnClickStartGameHandle += () => {
                ctx.evt.Login_OnStartGameClick();
            };

            panel.OnClickExitGameHandle += () => {
                ctx.evt.Login_OnExitGameClick();
            };

        }

        public static void Close(UIAppContext ctx) {
            var has = ctx.UniquePanel_TryGet<Panel_Login>(out var panel);
            if (!has) {
                return;
            }
            ctx.uiCore.UniquePanel_Close<Panel_Login>();
        }

    }

}