#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Alter.Modifier {

    public class RenamerEditor : EditorWindow {

        string stringToReplace = "";
        string replacementString = "";
        string selectedPath = "";

        [MenuItem("Tools/Renamer")]
        public static void ShowWindow() {
            GetWindow<RenamerEditor>("Renamer");
        }

        void OnGUI() {
            GUILayout.Label("Batch Renamer", EditorStyles.boldLabel);

            stringToReplace = EditorGUILayout.TextField("String to Replace", stringToReplace);
            replacementString = EditorGUILayout.TextField("Replacement String", replacementString);

            // 使用ObjectField选择文件夹，并直接在字段中显示路径
            EditorGUILayout.LabelField("Select Folder:");
            DefaultAsset folderAsset = AssetDatabase.LoadAssetAtPath<DefaultAsset>(selectedPath);
            var newFolderAsset = EditorGUILayout.ObjectField(folderAsset, typeof(DefaultAsset), false) as DefaultAsset;
            if (newFolderAsset != null) {
                string path = AssetDatabase.GetAssetPath(newFolderAsset);
                if (Directory.Exists(path)) {
                    selectedPath = path;
                    folderAsset = newFolderAsset;
                }
            }

            if (GUILayout.Button("Rename")) {
                RenameFiles();
            }
        }

        void RenameFiles() {
            if (string.IsNullOrEmpty(selectedPath)) {
                Debug.LogError("No path selected!");
                return;
            }

            var files = Directory.GetFiles(selectedPath, "*", SearchOption.AllDirectories);

            foreach (var file in files) {
                string normalizedFile = file.Replace("\\", "/");
                string relativePath = normalizedFile.Replace(Application.dataPath, "Assets");

                // 跳过.meta文件
                if (Path.GetExtension(relativePath) == ".meta") {
                    continue;
                }

                string fileName = Path.GetFileName(relativePath);

                if (fileName.Contains(stringToReplace)) {
                    string newName = fileName.Replace(stringToReplace, replacementString);
                    string newPath = Path.Combine(Path.GetDirectoryName(relativePath), newName).Replace("\\", "/");

                    string error = AssetDatabase.MoveAsset(relativePath, newPath);
                    if (!string.IsNullOrEmpty(error)) {
                        Debug.LogError("Error renaming file: " + error);
                    } else {
                        Debug.Log("Rename successful: " + newPath);
                    }
                }
            }

            AssetDatabase.Refresh();
        }

    }

}
#endif