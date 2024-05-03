using System;
using UnityEngine;
using UnityEngine.UI;

namespace Oshi.UI {

    public static class PanelGameInfoDomain {

        public static void Open(UIAppContext ctx, string title, bool showTime, bool showStep) {
            var has = ctx.UniquePanel_TryGet<Panel_GameInfo>(out var panel);
            if (has) {
                return;
            }

            panel = ctx.uiCore.UniquePanel_Open<Panel_GameInfo>();
            panel.Ctor();

            panel.OnRestartBtnClickHandle += () => {
                ctx.evt.GameInfo_OnRestartBtnClick();
            };

            panel.SetTitle(title);
            panel.ShowTime(showTime);
            panel.ShowStep(showStep);
        }

        public static void SetTitle(UIAppContext ctx, string title) {
            var has = ctx.UniquePanel_TryGet<Panel_GameInfo>(out var panel);
            if (!has) {
                return;
            }
            panel.SetTitle(title);
        }

        public static void ShowTime(UIAppContext ctx, bool show) {
            var has = ctx.UniquePanel_TryGet<Panel_GameInfo>(out var panel);
            if (!has) {
                return;
            }
            panel.ShowTime(show);
        }

        public static void RefreshTime(UIAppContext ctx, float time) {
            var has = ctx.UniquePanel_TryGet<Panel_GameInfo>(out var panel);
            if (!has) {
                return;
            }
            panel.RefreshTime(time);
        }

        public static void ShowStep(UIAppContext ctx, bool show) {
            var has = ctx.UniquePanel_TryGet<Panel_GameInfo>(out var panel);
            if (!has) {
                return;
            }
            panel.ShowStep(show);
        }

        public static void RefreshGameStep(UIAppContext ctx, int counter) {
            var has = ctx.UniquePanel_TryGet<Panel_GameInfo>(out var panel);
            if (!has) {
                return;
            }
            panel.RefreshGameStageCounter(counter);
        }

        public static void Close(UIAppContext ctx) {
            var has = ctx.UniquePanel_TryGet<Panel_GameInfo>(out var panel);
            if (!has) {
                return;
            }
            ctx.uiCore.UniquePanel_Close<Panel_GameInfo>();
        }

    }

}