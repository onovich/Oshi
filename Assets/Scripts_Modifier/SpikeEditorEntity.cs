#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class SpikeEditorEntity : MonoBehaviour {

        [SerializeField] public SpikeTM spikeTM;

        public Vector2Int GetPosInt() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public void Rename(int index) {
            gameObject.name = $"Spike - {spikeTM.typeName} - {index}";
        }

        public Vector2Int GetSizeInt() {
            var sizeInt = GetComponent<SpriteRenderer>().size.RoundToVector2Int();
            GetComponent<SpriteRenderer>().size = sizeInt;
            return sizeInt;
        }

    }

}
#endif