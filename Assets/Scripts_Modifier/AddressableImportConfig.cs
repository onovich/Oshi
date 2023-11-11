#if UNITY_EDITOR

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "so_config_aa_import", menuName = "AAImporter/Setting")]
public class AddressableImportConfig : ScriptableObject {

    public AddressableGroupEM[] modelArray;

}

#endif