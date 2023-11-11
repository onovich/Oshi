#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

[CustomEditor(typeof(AddressableImportConfig))]
public class AddressableImportConfigEditor : Editor {
    public override void OnInspectorGUI() {

        // 这将更新序列化对象的表示，确保所有的修改都是最新的
        serializedObject.Update();

        AddressableImportConfig config = (AddressableImportConfig)target;

        // 获取数组的SerializedProperty
        SerializedProperty arrayProperty = serializedObject.FindProperty("modelArray");

        // 显示数组大小的输入框

        // 数组大小配置
        int newSize = EditorGUILayout.IntField("Array Size", arrayProperty.arraySize);
        if (newSize != arrayProperty.arraySize) {
            arrayProperty.arraySize = newSize; // 设置新的数组大小
            serializedObject.ApplyModifiedProperties(); // 应用属性修改
            serializedObject.Update();  // 需要再次调用 Update，因为我们刚刚修改了属性
        }

        // 获取Addressable Groups & Lables 的名称
        string[] groupNames = GetAddressableGroupNames();
        string[] labelNames = GetAddressableLabelNames();

        // 遍历数组并为每个成员创建编辑器界面
        for (int i = 0; i < arrayProperty.arraySize; i++) {
            SerializedProperty arrayElement = serializedObject.FindProperty(nameof(config.modelArray)).GetArrayElementAtIndex(i);
            SerializedProperty targetFolderProp = arrayElement.FindPropertyRelative(nameof(AddressableGroupEM.targetFolder));
            SerializedProperty fileTypeProp = arrayElement.FindPropertyRelative(nameof(AddressableGroupEM.fileType));
            SerializedProperty groupNameProp = arrayElement.FindPropertyRelative(nameof(AddressableGroupEM.groupName));
            SerializedProperty lableProp = arrayElement.FindPropertyRelative(nameof(AddressableGroupEM.lable));

            EditorGUILayout.PropertyField(targetFolderProp);
            EditorGUILayout.PropertyField(fileTypeProp);

            // Group Popup
            int groupIndex = Array.IndexOf(groupNames, groupNameProp.stringValue);
            int newGroupIndex = EditorGUILayout.Popup("Group", groupIndex, groupNames);
            if (newGroupIndex >= 0) {
                groupNameProp.stringValue = groupNames[newGroupIndex];
            }

            // Label Popup
            int labelIndex = Array.IndexOf(labelNames, lableProp.stringValue);
            int newLabelIndex = EditorGUILayout.Popup("Label", labelIndex, labelNames);
            if (newLabelIndex >= 0) {
                lableProp.stringValue = labelNames[newLabelIndex];
            }

            EditorGUILayout.Space();
        }

        // 应用对SerializedProperty的所有更改
        serializedObject.ApplyModifiedProperties();

        // 如果Inspector发生更改，则保存配置
        if (GUI.changed) {
            EditorUtility.SetDirty(config);
        }

    }

    string[] GetAddressableGroupNames() {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        List<string> groupNames = new List<string>();
        foreach (var group in settings.groups) {
            if (group != null) {
                groupNames.Add(group.Name);
            }
        }
        return groupNames.ToArray();
    }

    string[] GetAddressableLabelNames() {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        HashSet<string> labels = new HashSet<string>();

        foreach (var group in settings.groups) {
            foreach (var entry in group.entries) {
                foreach (var label in entry.labels) {
                    labels.Add(label);
                }
            }
        }
        return new List<string>(labels).ToArray();
    }

}

#endif