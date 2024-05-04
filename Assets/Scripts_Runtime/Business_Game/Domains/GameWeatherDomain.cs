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
            var soundTable = ctx.templateInfraContext.SoundTable_Get();
            SoundApp.BGS_PlayLoop(ctx.soundContext, soundTable.bgsRainLoop, soundTable.bgsrainVolume);
            var camera = ctx.cameraContext.mainCamera;
            camera.backgroundColor = GetCameraColor(ctx);
        }

        public static void EnterNormal(GameBusinessContext ctx) {
            VFXApp.StopRainVFX(ctx.vfxContext);
            SoundApp.BGS_Stop(ctx.soundContext);
            var camera = ctx.cameraContext.mainCamera;
            camera.backgroundColor = GetCameraColor(ctx);
        }

        public static Color32 GetCameraColor(GameBusinessContext ctx) {
            var map = ctx.currentMapEntity;
            var weather = map.weatherType;
            var config = ctx.templateInfraContext.Config_Get();

            if (weather == WeatherType.Rain) {
                return config.weatherRainCameraColor;
            }

            return config.weatherNormalCameraColor;
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