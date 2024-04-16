using System;
using UnityEngine;
using UnityEngine.UI;

namespace Alter.UI {

    public static class PanelGameOverDomain {

        public static void Open(UIAppContext ctx, GameResult result) {
            var has = ctx.UniquePanel_TryGet<Panel_GameOver>(out var panel);
            if (has) {
                return;
            }

            panel = ctx.uiCore.UniquePanel_Open<Panel_GameOver>();
            panel.Ctor();

            panel.OnClickRestartGameHandle += () => {
                ctx.evt.GameOver_OnRestartGameClick();
            };

            panel.OnClickExitGameHandle += () => {
                ctx.evt.GameOver_OnExitGameClick();
            };

            SetResult(ctx, result);
        }

        static void SetResult(UIAppContext ctx, GameResult result) {
            var has = ctx.UniquePanel_TryGet<Panel_GameOver>(out var panel);
            if (!has) {
                return;
            }
            if (result == GameResult.Win) {
                panel.SetResult("YOU WIN!");
            } else {
                panel.SetResult("YOU LOSE!");
            }
        }

        public static void Close(UIAppContext ctx) {
            var has = ctx.UniquePanel_TryGet<Panel_GameOver>(out var panel);
            if (!has) {
                return;
            }
            ctx.uiCore.UniquePanel_Close<Panel_GameOver>();
        }

    }

}