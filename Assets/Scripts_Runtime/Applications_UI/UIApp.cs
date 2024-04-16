using System;
using System.Threading.Tasks;
using Chouten.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Chouten {
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

        // Tick
        public static void LateTick(UIAppContext ctx, float dt) {
            ctx.uiCore.LateTick(dt);
        }

        // Panel - Login
        public static void Login_Open(UIAppContext ctx) {
            PanelLoginDomain.Open(ctx);
        }

        public static void Login_Close(UIAppContext ctx) {
            PanelLoginDomain.Close(ctx);
        }

        // Panel - GameOver
        public static void GameOver_Open(UIAppContext ctx, GameResult result) {
            PanelGameOverDomain.Open(ctx, result);
        }

        public static void GameOver_Close(UIAppContext ctx) {
            PanelGameOverDomain.Close(ctx);
        }

        // Panel - GameInfo
        public static void GameInfo_Open(UIAppContext ctx, int hpMax) {
            PanelGameInfoDomain.Open(ctx, hpMax);
        }

        public static void GameInfo_RefreshHP(UIAppContext ctx, int hp) {
            PanelGameInfoDomain.RefreshHP(ctx, hp);
        }

        public static void GameInfo_RefreshTime(UIAppContext ctx, float time) {
            PanelGameInfoDomain.RefreshTime(ctx, time);
        }

        public static void GameInfo_Close(UIAppContext ctx) {
            PanelGameInfoDomain.Close(ctx);
        }

    }

}