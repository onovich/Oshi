#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace Alter.Modifier {

    public class GoalEditorEntity : MonoBehaviour {

        [SerializeField] public GoalTM goalTM;

        public Vector2Int GetPosInt() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public void Rename(int index) {
            gameObject.name = $"Goal - {goalTM.typeName} - {index}";
        }

        public Vector2Int GetSizeInt() {
            var sizeInt = GetComponent<SpriteRenderer>().size.RoundToVector2Int();
            GetComponent<SpriteRenderer>().size = sizeInt;
            return sizeInt;
        }

    }

}
#endif