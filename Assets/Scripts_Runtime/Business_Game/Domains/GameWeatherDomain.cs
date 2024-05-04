using TenonKit.Prism;
using UnityEngine;

namespace Oshi {

    public static class GameWeatherDomain {

        public static void EnterWeather(GameBusinessContext ctx, WeatherType type) {
            switch (type) {
                case WeatherType.Rain:
                    EnterRain(ctx);
                    break;
                default:
                    EnterNormal(ctx);
                    break;
            }
        }

        public static void EnterRain(GameBusinessContext ctx) {
            VFXApp.PlayRainVFX(ctx.vfxContext);
        }

        public static void EnterNormal(GameBusinessContext ctx) {
            VFXApp.StopRainVFX(ctx.vfxContext);
        }

        public static Color32 GetWeatherColor(GameBusinessContext ctx) {
            var map = ctx.currentMapEntity;
            var weather = map.weatherType;
            var config = ctx.templateInfraContext.Config_Get();

            if (weather == WeatherType.Rain) {
                return config.weatherRainColor;
            }

            return config.weatherNormalColor;
        }

    }

}