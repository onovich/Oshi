#if UNITY_EDITOR
using UnityEngine;

namespace Chouten.Modifier {

    public class SpawnPointEditorEntity : MonoBehaviour {

        public void Rename(int index) {
            string side = "";
            if (index == 0) {
                side = "Left";
            } else if (index == 1) {
                side = "Middle";
            } else if (index == 2) {
                side = "Right";
            }
            this.gameObject.name = $"Spawn Point {side}";
        }

        public Vector2 GetPos() {
            return transform.position;
        }

        public Vector2Int GetSizeInt() {
            var size = transform.localScale;
            var sizeInt = size.RoundToVector2Int();
            return sizeInt;
        }

    }

}
#endif