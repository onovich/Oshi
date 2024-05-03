using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public static class GamePPDomain {

        public static void ApplyFadingIn(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;
            var config = ctx.templateInfraContext.Config_Get();

            var startColor = Color.white;
            var endColor = Color.black;
            var duration = fsm.fadingIn_duration;
            var enterTime = fsm.fadingIn_enterTime;

            if (enterTime > duration) {
                fsm.PlayerTurn_Enter();
                return;
            }

            var currentColor = EasingHelper.EasingColor(startColor, endColor, enterTime, duration, config.fadingInEasingType, config.fadingInEasingMode);
            PPApp.ColorAdjustMents_SetColor(ctx.ppAppContext, currentColor);

            fsm.FadingIn_IncTimer(dt);
        }

        public static void ApplyFadingOut(GameBusinessContext ctx, float dt, Action onEnd) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;
            var config = ctx.templateInfraContext.Config_Get();

            var startColor = Color.black;
            var endColor = Color.white;
            var duration = fsm.fadingOut_duration;
            var enterTime = fsm.fadingOut_enterTime;

            if (enterTime > duration) {
                onEnd.Invoke();
                return;
            }

            var currentColor = EasingHelper.EasingColor(startColor, endColor, enterTime, duration, config.fadingOutEasingType, config.fadingOutEasingMode);
            PPApp.ColorAdjustMents_SetColor(ctx.ppAppContext, currentColor);

            fsm.FadingOut_IncTimer(dt);
        }

    }

}