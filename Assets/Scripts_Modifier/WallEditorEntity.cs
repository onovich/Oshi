#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Oshi.Modifier {

    public class WallEditorEntity : MonoBehaviour {

        [SerializeField] public WallTM wallTM;

        public Vector2Int GetPosInt() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public void Rename(int index) {
            gameObject.name = $"Wall - {wallTM.typeName} - {index}";
        }

        public Vector2Int GetSizeInt() {
            var sizeInt = GetComponent<SpriteRenderer>().size.RoundToVector2Int();
            GetComponent<SpriteRenderer>().size = sizeInt;
            return sizeInt;
        }

    }

}
#endif