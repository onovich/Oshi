using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    public static class GamePPDomain {

        public static void ApplyFadingIn(GameBusinessContext ctx, float dt) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;
            var config = ctx.templateInfraContext.Config_Get();

            var startColor = Color.black;
            var endColor = GameWeatherDomain.GetWeatherColor(ctx); ;
            var duration = fsm.fadingIn_duration;
            var enterTime = fsm.fadingIn_enterTime;

            if (enterTime > duration) {
                fsm.PlayerTurn_Enter();
                PPApp.ColorAdjustMents_SetColor(ctx.ppAppContext, endColor);
                return;
            }

            var currentColor = EasingHelper.EasingColor(startColor, endColor, enterTime, duration, config.fadingInEasingType, config.fadingInEasingMode);
            PPApp.ColorAdjustMents_SetColor(ctx.ppAppContext, currentColor);

            fsm.FadingIn_IncTimer(dt);
        }

        public static void ApplyFadingOut(GameBusinessContext ctx, float dt, Action<GameResult> onEnd) {
            var game = ctx.gameEntity;
            var fsm = game.fsmComponent;
            var config = ctx.templateInfraContext.Config_Get();

            var startColor = GameWeatherDomain.GetWeatherColor(ctx);
            var endColor = Color.black;
            var duration = fsm.fadingOut_duration;
            var enterTime = fsm.fadingOut_enterTime;
            var result = fsm.fadingOut_result;
            var easingMode = fsm.fadingOut_easingMode;
            var easingType = fsm.fadingOut_easingType;

            if (enterTime > duration) {
                onEnd.Invoke(result);
                PPApp.ColorAdjustMents_SetColor(ctx.ppAppContext, endColor);
                return;
            }

            var currentColor = EasingHelper.EasingColor(startColor, endColor, enterTime, duration, easingType, easingMode);
            PPApp.ColorAdjustMents_SetColor(ctx.ppAppContext, currentColor);

            fsm.FadingOut_IncTimer(dt);
        }

    }

}