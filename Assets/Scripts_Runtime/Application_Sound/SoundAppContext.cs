using System.Collections.Generic;
using UnityEngine;

namespace Oshi {

    public class SoundAppContext {

        public Transform root;
        public AudioSource audioSourcePrefab;

        public AudioSource bgmPlayer;
        public int bgmPlayerIndex;

        public AudioSource[] roleGenericPlayer; // Move / Die 
        public AudioSource[] blockGatherTreePlayer; // Move / Die

        public SoundAppContext(Transform soundRoot) {
            roleGenericPlayer = new AudioSource[8];
            blockGatherTreePlayer = new AudioSource[8];
            roleGenericPlayer = new AudioSource[4];
            this.root = soundRoot;
        }

    }

}