using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Chouten {

    [CreateAssetMenu(fileName = "SO_Map", menuName = "Chouten/MapTM")]
    public class MapTM : ScriptableObject {

        public int typeID;

        public Vector2Int mapSize;

        // Wave
        public WaveTM leftWaveTM;
        public WaveTM rightWaveTM;

        // Point
        public Vector2 middlePoint;
        public Vector2 leftBound;
        public Vector2 rightBound;

        // Camera
        public Vector2 cameraConfinerWorldMax;
        public Vector2 cameraConfinerWorldMin;

    }

}