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

        [Header("Shake Config")]
        public float cameraShakeFrequency_roleDamage;
        public float cameraShakeAmplitude_roleDamage;
        public float cameraShakeDuration_roleDamage;
        public EasingType cameraShakeEasingType_roleDamage;
        public EasingMode cameraShakeEasingMode_roleDamage;

    }

}