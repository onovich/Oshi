using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Path", menuName = "Oshi/PathTM")]
    public class PathTM : ScriptableObject {

        public int typeID;
        public EasingType easingType;
        public EasingMode easingMode;
        public float movingDuration;
    }

}