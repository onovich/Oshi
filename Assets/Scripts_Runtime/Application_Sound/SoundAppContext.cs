using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Oshi {

    public class SoundAppContext {

        public Transform root;
        public AudioSource audioSourcePrefab;

        public AudioSource[] bgmPlayer;
        public AudioSource[] roleGenericPlayer; // Move / Die 
        public AudioSource[] blockGenericPlayer; // Move / Die
        public AudioSource bgsPlayer;
        public AsyncOperationHandle assetsHandle;

        public SoundAppContext(Transform soundRoot) {
            roleGenericPlayer = new AudioSource[8];
            blockGenericPlayer = new AudioSource[8];
            bgmPlayer = new AudioSource[4];
            this.root = soundRoot;
        }

    }

}