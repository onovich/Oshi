using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Chouten {

    [CreateAssetMenu(fileName = "SO_GameConfig", menuName = "Chouten/GameConfig")]
    public class GameConfig : ScriptableObject {

        // Game
        [Header("Game Config")]
        public float gameResetEnterTime;
        public float gameTotalTime;

        // Role
        [Header("Role Config")]
        public int ownerRoleTypeID;
        public int originalMapTypeID;

        // Camera
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