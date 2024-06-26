using System;
using System.Threading.Tasks;
using Oshi.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Oshi {
    public static class UIApp {

        public static void Init(UIAppContext ctx) {

        }

        public static async Task LoadAssets(UIAppContext ctx) {
            try {
                await ctx.uiCore.LoadAssets();
            } catch (Exception e) {
                GLog.LogError(e.ToString());
            }
        }

        public static void ReleaseAssets(UIAppContext ctx) {
            ctx.uiCore.ReleaseAssets();
        }

        public static void TearDown(UIAppContext ctx) {
            ctx.uiCore.TearDown();
        }

        // Tick
        public static void LateTick(UIAppContext ctx, float dt) {
            ctx.uiCore.LateTick(dt);
        }

        // Panel - Login
        public static void Login_Open(UIAppContext ctx, bool hasSave) {
            PanelLoginDomain.Open(ctx, hasSave);
        }

        public static void Login_Close(UIAppContext ctx) {
            PanelLoginDomain.Close(ctx);
        }

        // Panel - GameOver
        public static void GameOver_Open(UIAppContext ctx, GameResult result, bool isLastMap) {
            PanelGameOverDomain.Open(ctx, result, isLastMap);
        }

        public static void GameOver_Close(UIAppContext ctx) {
            PanelGameOverDomain.Close(ctx);
        }

        // Panel - GameInfo
        public static void GameInfo_Open(UIAppContext ctx, string title, bool showTime, bool showStep) {
            PanelGameInfoDomain.Open(ctx, title, showTime, showStep);
        }


        public static void GameInfo_ShowTime(UIAppContext ctx, bool show) {
            PanelGameInfoDomain.ShowTime(ctx, show);
        }

        public static void GameInfo_RefreshTime(UIAppContext ctx, float time) {
            PanelGameInfoDomain.RefreshTime(ctx, time);
        }

        public static void GameInfo_ShowStep(UIAppContext ctx, bool show) {
            PanelGameInfoDomain.ShowStep(ctx, show);
        }

        public static void GameInfo_SetTitle(UIAppContext ctx, string title) {
            PanelGameInfoDomain.SetTitle(ctx, title);
        }

        public static void GameInfo_RefreshStep(UIAppContext ctx, int counter) {
            PanelGameInfoDomain.RefreshGameStep(ctx, counter);
        }

        public static void GameInfo_Close(UIAppContext ctx) {
            PanelGameInfoDomain.Close(ctx);
        }

    }

}