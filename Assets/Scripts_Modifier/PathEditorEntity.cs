#if UNITY_EDITOR
using UnityEngine;

namespace Oshi.Modifier {

    public class PathEditorEntity : MonoBehaviour {

        [SerializeField] public PathTM pathTM;
        [SerializeField] public GameObject traveler;
        [SerializeField] public bool isCircleLoop;
        [SerializeField] public bool isPingPongLoop;

        public Vector3[] GetPathNodeArr() {
            var pathNodeArr = new Vector3[transform.childCount];
            for (int i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i);
                pathNodeArr[i] = child.position.RoundToVector2Int().ToVector3Int();
                child.transform.position = pathNodeArr[i];
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
            var offset = transform.GetChild(0).position - traveler.transform.position;
            for (int i = 0; i < transform.childCount; i++) {
                var child = transform.GetChild(i);
                child.position -= offset;
                child.position = child.position.RoundToVector2Int().ToVector3Int();
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
            if (pathNodeArr == null) return;
            for (int i = 0; i < pathNodeArr.Length; i++) {
                var pos = pathNodeArr[i];
                var halfSize = GetTravelerSize(traveler) / 2;
                Gizmos.color = i == 0 ? Color.yellow : Color.yellow;
                var size = i == 0 ? Vector3.one * .2f : Vector3.one * .1f;
                Gizmos.DrawCube(pos + halfSize.ToVector3(), size);
                var next = Vector3.zero;
                if (isCircleLoop) {
                    next = pathNodeArr[(i + 1) % pathNodeArr.Length];
                } else {
                    next = i == pathNodeArr.Length - 1 ? pathNodeArr[i] : pathNodeArr[i + 1];
                }
                Gizmos.color = pathTM.lineColor;
                Gizmos.DrawLine(pos + halfSize.ToVector3(), next + halfSize.ToVector3());
            }

            if (traveler == null) return;
            var block = traveler.GetComponent<BlockEditorEntity>();
            var wall = traveler.GetComponent<WallEditorEntity>();
            var goal = traveler.GetComponent<GoalEditorEntity>();
            var spike = traveler.GetComponent<SpikeEditorEntity>();
            var offset = pathNodeArr[pathNodeArr.Length - 1] - pathNodeArr[0];
            if (block != null) block.OnDrawGhost(offset);
            if (wall != null) wall.OnDrawGhost(offset);
            if (goal != null) goal.OnDrawGhost(offset);
            if (spike != null) spike.OnDrawGhost(offset);
        }

        public Vector2 GetTravelerSize(GameObject traveler) {
            if (traveler.TryGetComponent<BlockEditorEntity>(out var block)) return block.blockTM.shapeArr[0].sizeInt;
            if (traveler.TryGetComponent<WallEditorEntity>(out var wall)) return wall.wallTM.shapeArr[0].sizeInt;
            if (traveler.TryGetComponent<GoalEditorEntity>(out var goal)) return goal.goalTM.shapeArr[0].sizeInt;
            if (traveler.TryGetComponent<SpikeEditorEntity>(out var spike)) return spike.spikeTM.shapeArr[0].sizeInt;
            return Vector2.zero;
        }

    }

}
#endif