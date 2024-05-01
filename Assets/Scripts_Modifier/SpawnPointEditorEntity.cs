#if UNITY_EDITOR
using UnityEngine;

namespace Oshi.Modifier {

    public class SpawnPointEditorEntity : MonoBehaviour {

        public RoleTM roleTM;

        public void Rename() {
            this.gameObject.name = $"Spawn Point";
        }

        public Vector2Int GetPosInt() {
            var posInt = transform.position.RoundToVector2Int();
            transform.position = posInt.ToVector3Int();
            return posInt;
        }

        public Vector2Int GetSizeInt() {
            var size = transform.localScale;
            var sizeInt = size.RoundToVector2Int();
            return sizeInt;
        }

        void OnDrawGizmos() {
            if (roleTM == null) {
                return;
            }
            var pos = GetPosInt();
            Gizmos.DrawGUITexture(new Rect(pos.x, pos.y, 1, 1), roleTM.mesh.texture, 0, 0, 0, 0);
        }

    }

}
#endif