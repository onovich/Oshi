using System;
using UnityEngine;
using UnityEngine.UI;

namespace Oshi.UI {

    public static class PanelGameOverDomain {

        public static void Open(UIAppContext ctx, GameResult result, bool isLastMap) {
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

            panel.OnClickNextLevelHandle += () => {
                ctx.evt.GameOver_OnNextLevelClick();
            };

            SetResult(ctx, result, isLastMap);
        }

        static void SetResult(UIAppContext ctx, GameResult result, bool isLastMap) {
            var has = ctx.UniquePanel_TryGet<Panel_GameOver>(out var panel);
            if (!has) {
                return;
            }
            if (result == GameResult.Win && !isLastMap) {
                panel.SetResult("YOU WIN!");
                panel.ShowExitBtn(false);
                panel.ShowNextLevelBtn(true);
            } else if (result == GameResult.Lose) {
                panel.SetResult("YOU LOSE!");
                panel.ShowExitBtn(true);
                panel.ShowNextLevelBtn(false);
            } else {
                panel.SetResult("YOU WIN ALL!");
                panel.ShowExitBtn(true);
                panel.ShowNextLevelBtn(false);
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