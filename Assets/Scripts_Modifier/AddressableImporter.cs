#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;

public class AddressableImporter : EditorWindow {
    DefaultAsset targetFolder;
    FileType fileType = FileType.Prefab;

    string selectedLabel;
    string[] labelCollectionsArray;
    int labelSelectedIndex;

    string selectedGroup;
    string[] groupCollectionsArray;
    int groupSelectedIndex;

    enum FileType {
        Prefab = 0,
        Mp3 = 1,
        Png = 2,
        Asset = 3,
        Bytes = 4,
    }

    [MenuItem("Tools/Addressable Importer")]
    public static void ShowWindow() {
        GetWindow<AddressableImporter>("Addressable Importer");
    }

    private void OnEnable() {

        Refresh();

    }

    void Refresh() {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null || settings.groups == null || settings.GetLabels() == null) {
            return;
        }

        // 获取 Group
        var groupCollections = new List<string>();
        List<AddressableAssetGroup> allGroups = new List<AddressableAssetGroup>(settings.groups);
        foreach (AddressableAssetGroup group in allGroups) {
            groupCollections.Add(group.name);
        }
        groupCollectionsArray = groupCollections.ToArray();

        // 获取 Label
        var labelCollections = new List<string>();
        IEnumerable<string> allLabels = settings.GetLabels();
        foreach (string label in allLabels) {
            labelCollections.Add(label);
        }
        labelCollectionsArray = labelCollections.ToArray();
    }

    private void OnGUI() {

        if (GUILayout.Button("刷新分组和标签数据")) {
            Refresh();
        }

        // 标题
        GUILayout.Label("请选择需要批量导入 AA 的文件夹", EditorStyles.boldLabel);

        // 获取选择的文件夹 / 文件类型
        targetFolder = (DefaultAsset)EditorGUILayout.ObjectField("Folder", targetFolder, typeof(DefaultAsset), false);
        fileType = (FileType)EditorGUILayout.EnumPopup("File Extension", fileType);

        // 获取选择的 Label / Group
        int labellNewIndex = EditorGUILayout.Popup("Label", labelSelectedIndex, labelCollectionsArray);
        if (labellNewIndex != labelSelectedIndex) {
            labelSelectedIndex = labellNewIndex;
            selectedLabel = labelCollectionsArray[labelSelectedIndex];
            groupSelectedIndex = EditorGUILayout.Popup("Group", groupSelectedIndex, groupCollectionsArray);
            if (groupSelectedIndex < 0 || groupSelectedIndex >= groupCollectionsArray.Length) {
                groupSelectedIndex = 0;
            }
            selectedGroup = groupCollectionsArray[groupSelectedIndex];
        }

        // // 获取 AA 配置文件
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

        if (settings != null) {
            string[] groupNames = settings.groups.Select(group => group.Name).ToArray();
            groupSelectedIndex = EditorGUILayout.Popup("Group", groupSelectedIndex, groupNames);
        } else {
            EditorGUILayout.LabelField("No Addressable Asset Settings found.");
        }

        // 点击按钮
        if (GUILayout.Button("Import Addressables")) {
            ImportAddressables();
        }

    }

    private void ImportAddressables() {

        if (targetFolder == null) {
            Debug.LogError("No folder selected");
            return;
        }

        string FolderPath = AssetDatabase.GetAssetPath(targetFolder);

        // 获取目录下指定文件类型的所有文件
        string fileExtension = "." + fileType.ToString().ToLower();
        string[] files = Directory.GetFiles(FolderPath, "*" + fileExtension, SearchOption.AllDirectories);
        UnityEngine.Object[] assets = files.Select(file => AssetDatabase.LoadAssetAtPath<Object>(file)).ToArray();

        // 获取 Addressable 相关反射方法
        AddressableHelper.AddressableReflection(
            out object settings_obj, out object groups_obj, out PropertyInfo defaultGroup_prop, out object defaultGroup_obj,
            out MethodInfo method_CreateOrMoveEntry, out MethodInfo gatherTargetInfos_methodInfo, out MethodInfo setAaEntry_methodInfo
        );

        // 获取 AA 配置文件
        AddressableAssetSettings settings = settings_obj as AddressableAssetSettings;

        // 获取分组
        var targetGroup = settings.groups[groupSelectedIndex];

        // 获取创建文件 / 移动文件的方法
        MethodInfo methodCreateOrMoveEntry = settings_obj.GetType().GetMethod("CreateOrMoveEntry", BindingFlags.Instance | BindingFlags.Public);

        foreach (var asset in assets) {

            if (asset != null) {

                AddressableHelper.SetSimplifiedName(asset, settings_obj, targetGroup, methodCreateOrMoveEntry);
                AddressableHelper.SetLabel(asset, settings_obj, targetGroup, methodCreateOrMoveEntry, selectedLabel);

                AddressableHelper.SaveData(defaultGroup_prop, settings_obj, targetGroup, gatherTargetInfos_methodInfo, setAaEntry_methodInfo,
                 defaultGroup_obj, assets);

            }
        }

        // 存储配置
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();

    }

}

#endif