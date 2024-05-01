#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Drawing;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace Oshi {

    public class MapEM : SerializedMonoBehaviour {

        [Header("Map Sequence")]
        public MapTM[] mapTMArr;

        [Button("Bake")]
        void Bake() {
            for (int i = 0; i < mapTMArr.Length; i++) {
                var current = mapTMArr[i];
                var next = i + 1 < mapTMArr.Length ? mapTMArr[i + 1] : null;
                BakeMapSequence(current, next);
            }
            AssetDatabase.SaveAssets();
        }

        void BakeMapSequence(MapTM current, MapTM next) {
            if (next == null) {
                current.isLastMap = true;
                return;
            }
            current.nextMapTypeID = next.typeID;
            current.isLastMap = false;
            EditorUtility.SetDirty(current);
        }

    }

}
#endif