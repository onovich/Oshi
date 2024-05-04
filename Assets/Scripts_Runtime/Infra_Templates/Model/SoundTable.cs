using UnityEngine;

namespace VTD {

    [CreateAssetMenu(fileName = "SoundTable", menuName = "Oshi/SoundTable")]
    public class SoundTable : ScriptableObject {

        [Header("Role SE")]
        public AudioClip roleMove;
        public float roleMoveVolume;
        public AudioClip roleDie;
        public float roleDieVolume;

        [Header("Block SE")]
        public AudioClip blockMove;
        public float blockMoveVolume;
        public AudioClip blockDie;
        public float blockDieVolume;

        [Header("BGM")]
        public AudioClip[] bgmLoop;
        public float[] bgmVolume;

    }

}