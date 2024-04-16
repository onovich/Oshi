using System;
using UnityEngine;

namespace Chouten {

    [CreateAssetMenu(fileName = "SO_Wave", menuName = "Chouten/WaveTM")]
    public class WaveTM : ScriptableObject {

        public int typeID;
        public float[] spawnTimeArr;
        public int[] roleTypeIDArr;

    }

}