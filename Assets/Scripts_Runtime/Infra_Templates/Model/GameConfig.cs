using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_GameConfig", menuName = "Oshi/GameConfig")]
    public class GameConfig : ScriptableObject {

        [Header("Game Config")]
        public float gameResetEnterTime;

        [Header("Map Config")]
        public int testMapTypeID;
        public int originalMapTypeID;

        [Header("DeadZone Config")]
        public Vector2 cameraDeadZoneNormalizedSize;

        [Header("Weather Config")]
        public GameObject weatherRainVFX;
        public Color32 weatherRainColor;
        public Color32 weatherRainCameraColor;

        public Color32 weatherNormalColor;
        public Color32 weatherNormalCameraColor;

        [Header("PP Config")]
        public EasingType fadingInEasingType;
        public EasingMode fadingInEasingMode;
        public float fadingInDuration;

        public EasingType fadingOutEasingType;
        public EasingMode fadingOutEasingMode;
        public float fadingOutDuration;

        public EasingType manualFadingOutEasingType;
        public EasingMode manualFadingOutEasingMode;
        public float manualFadingOutDuration;

        [Header("Shake Config")]
        public float cameraShakeFrequency_roleDamage;
        public float cameraShakeAmplitude_roleDamage;
        public float cameraShakeDuration_roleDamage;
        public EasingType cameraShakeEasingType_roleDamage;
        public EasingMode cameraShakeEasingMode_roleDamage;

    }

}