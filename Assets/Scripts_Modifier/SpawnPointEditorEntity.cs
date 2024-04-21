#if UNITY_EDITOR
using UnityEngine;

namespace Oshi.Modifier {

    public class SpawnPointEditorEntity : MonoBehaviour {

        public void Rename() {
            this.gameObject.name = $"Spawn Point";
        }

        public Vector2Int GetPos() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public Vector2Int GetSizeInt() {
            var size = transform.localScale;
            var sizeInt = size.RoundToVector2Int();
            return sizeInt;
        }

    }

}
#endif