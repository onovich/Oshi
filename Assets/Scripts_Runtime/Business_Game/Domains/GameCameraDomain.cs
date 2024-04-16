namespace Alter {

    public static class GameCameraDomain {

        public static void ShakeOnce(GameBusinessContext ctx) {

            var config = ctx.templateInfraContext.Config_Get();

            var shakeFrequency = config.cameraShakeFrequency_roleDamage;
            var shakeAmplitude = config.cameraShakeAmplitude_roleDamage;
            var shakeDuration = config.cameraShakeDuration_roleDamage;
            var easingType = config.cameraShakeEasingType_roleDamage;
            var easingMode = config.cameraShakeEasingMode_roleDamage;

            var cameraID = ctx.cameraContext.mainCameraID;
            CameraApp.ShakeOnce(ctx.cameraContext,cameraID, shakeFrequency, shakeAmplitude, shakeDuration, easingType, easingMode);

        }

    }

}