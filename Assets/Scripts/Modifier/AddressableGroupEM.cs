#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

    [Serializable]
    public struct AddressableGroupEM {
        public DefaultAsset targetFolder;
        public FileType fileType;
        public string groupName;
        public string lable;

    }

#endif