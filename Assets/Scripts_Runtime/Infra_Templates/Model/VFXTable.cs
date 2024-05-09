using UnityEngine;

namespace VTD {

    [CreateAssetMenu(fileName = "VFXTable", menuName = "Oshi/VFXTable")]
    public class VFXTable : ScriptableObject {

        [Header("Role VFX")]
        public GameObject roleDeadVFX;
        public float roleDeadVFXDuration;

        [Header("Block VFX")]
        public GameObject blockDeadVFX;
        public float blockDeadVFXDuration;

        [Header("Gate VFX")]
        public GameObject gateDeadVFX;
        public float gateDeadVFXDuration;

        [Header("Goal VFX")]
        public GameObject goalDeadVFX;
        public float goalDeadVFXDuration;

    }

}