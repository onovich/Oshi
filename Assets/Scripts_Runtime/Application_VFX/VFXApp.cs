using System;
using System.Threading.Tasks;
using Oshi.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Oshi {
    public static class VFXApp {

        public static void Init(VFXAppContext ctx) {

        }

        public static async Task LoadAssets(VFXAppContext ctx) {
            try {
                await ctx.vfxCore.LoadAssets();
            } catch (Exception e) {
                GLog.LogError(e.ToString());
            }
        }

        public static void ReleaseAssets(VFXAppContext ctx) {
            ctx.vfxCore.ReleaseAssets();
        }

        public static void LateTick(VFXAppContext ctx, float dt) {
            ctx.vfxCore.Tick(dt);
        }

        public static void PlayRainVFX(VFXAppContext ctx) {
            if (ctx.weather_rain_vfx_id == -1) {
                var config = ctx.templateInfraContext.Config_Get();
                var vfx = config.weatherRainVFX;
                var id = TryPreSpawnVFX_ToWorldPos(ctx, vfx.name, 0f, Vector2.zero);
                ctx.weather_rain_vfx_id = id;
                PlayVFXManualy(ctx, id);
            }else{
                PlayVFXManualy(ctx, ctx.weather_rain_vfx_id);
            }
        }

        public static void StopRainVFX(VFXAppContext ctx) {
            if (ctx.weather_rain_vfx_id == -1) {
                return;
            }
            StopVFXManualy(ctx, ctx.weather_rain_vfx_id);
        }

        public static void AddVFXToWorld(VFXAppContext ctx, string vfxName, float duration, Vector2 pos) {
            ctx.vfxCore.TrySpawnAndPlayVFX_ToWorldPos(vfxName, duration, pos);
        }

        public static void AddVFXToTarget(VFXAppContext ctx, string vfxName, float duration, Transform target) {
            ctx.vfxCore.TrySpawnAndPlayVFX_ToTarget(vfxName, duration, target, Vector3.zero);
        }

        public static int TryPreSpawnVFX_ToWorldPos(VFXAppContext ctx, string vfxName, float duration, Vector2 pos) {
            return ctx.vfxCore.TryPreSpawnVFX_ToWorldPos(vfxName, duration, pos);
        }

        public static void PlayVFXManualy(VFXAppContext ctx, int preSpawnVFXID) {
            ctx.vfxCore.TryPlayManualy(preSpawnVFXID);
        }

        public static void StopVFXManualy(VFXAppContext ctx, int preSpawnVFXID) {
            ctx.vfxCore.TryStopManualy(preSpawnVFXID);
        }

        public static void TearDown(VFXAppContext ctx) {
            ctx.vfxCore.TearDown();
        }

    }

}