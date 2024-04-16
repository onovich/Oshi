using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Chouten {

    public class WaveModel {

        public int typeID;
        public float[] spawnTimeArr;
        public int[] roleTypeIDArr;

        public int waveIndex;

        public void Ctor() {
            waveIndex = 0;
        }

        public void IncWaveIndex() {
            waveIndex++;
        }

    }

}