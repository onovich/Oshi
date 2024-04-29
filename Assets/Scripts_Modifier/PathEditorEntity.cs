#if UNITY_EDITOR
using UnityEngine;

namespace Oshi.Modifier {

    public class PathEditorEntity : MonoBehaviour {

        [SerializeField] public GameObject traveler;

        public Vector2Int[] GetPathNodeArr() {
            var pathNodeArr = new Vector2Int[transform.childCount];
            for (int i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i);
                pathNodeArr[i] = child.position.RoundToVector2Int();
            }
            return pathNodeArr;
        }

        public void Rename(int index) {
            var travelerName = GetTravelerType().ToString();
            this.name = "Path - " + index + " - " + travelerName;
            for (int i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i);
                child.name = $"{this.name} - N - {i}";
            }
        }

        public EntityType GetTravelerType() {
            var block = traveler.GetComponent<BlockEditorEntity>();
            var wall = traveler.GetComponent<WallEditorEntity>();
            var goal = traveler.GetComponent<GoalEditorEntity>();
            var spike = traveler.GetComponent<SpikeEditorEntity>();
            if (block != null) return block.type;
            if (wall != null) return wall.type;
            if (goal != null) return goal.type;
            if (spike != null) return spike.type;
            return EntityType.None;
        }

        public int GetTravlerIndex() {
            var block = traveler.GetComponent<BlockEditorEntity>();
            var wall = traveler.GetComponent<WallEditorEntity>();
            var goal = traveler.GetComponent<GoalEditorEntity>();
            var spike = traveler.GetComponent<SpikeEditorEntity>();
            if (block != null) return block.index;
            if (wall != null) return wall.index;
            if (goal != null) return goal.index;
            if (spike != null) return spike.index;
            return -1;
        }

        void OnDrawGizmos() {
            if (transform.childCount == 0) return;
            var pathNodeArr = GetPathNodeArr();
            for (int i = 0; i < pathNodeArr.Length; i++) {
                var pos = pathNodeArr[i];
                Gizmos.color = i == 0 ? Color.white : Color.red;
                var size = i == 0 ? Vector3.one * .2f : Vector3.one * .1f;
                Gizmos.DrawCube(pos.ToVector3Int(), size);
                var next = pathNodeArr[(i + 1) % pathNodeArr.Length];
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(pos.ToVector3Int(), next.ToVector3Int());
            }
        }

    }

}
#endif