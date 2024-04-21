using System;
using MortiseFrame.Swing;
using UnityEngine;

namespace Oshi {

    [CreateAssetMenu(fileName = "SO_Role", menuName = "Oshi/RoleTM")]
    public class RoleTM : ScriptableObject {

        [Header("Role Info")]
        public int typeID;
        public string typeName;
        public AllyStatus allyStatus;

        [Header("Role Attr")]
        public Vector2Int size;
        public float moveSpeed;
        public int hpMax;

        [Header("Role Move")]
        public float moveDurationSec;
        public EasingType moveEasingType;
        public EasingMode moveEasingMode;

        [Header("Role Render")]
        public GameObject deadVFX;
        public float deadVFXDuration;
        public Sprite mesh;
        public Material meshMaterial;
        
    }

}