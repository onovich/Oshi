#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Oshi.Modifier{

    public class ImageScalerEditor : EditorWindow {

        private DefaultAsset sourceFolder;
        private DefaultAsset destinationFolder;
        private int scaleMultiplier = 1;
        private int scaleDownMultiplier = 1;
        private bool overwriteOriginal = true;
        private bool scaleDown = false;

        [MenuItem("Tools/Image Scaler")]
        public static void ShowWindow() {
            GetWindow<ImageScalerEditor>("Image Scaler");
        }

        void OnGUI() {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            sourceFolder = (DefaultAsset)EditorGUILayout.ObjectField("Source Folder", sourceFolder, typeof(DefaultAsset), false);
            scaleMultiplier = EditorGUILayout.IntField("Scale Multiplier", scaleMultiplier);
            scaleDown = EditorGUILayout.Toggle("Scale Down", scaleDown);
            if (scaleDown) {
                scaleDownMultiplier = EditorGUILayout.IntField("Scale Down Multiplier", scaleDownMultiplier);
            }
            overwriteOriginal = EditorGUILayout.Toggle("Overwrite Original", overwriteOriginal);

            if (!overwriteOriginal) {
                destinationFolder = (DefaultAsset)EditorGUILayout.ObjectField("Destination Folder", destinationFolder, typeof(DefaultAsset), false);
            }

            if (GUILayout.Button("Scale Images")) {
                ScaleImages();
            }
        }

        private void ScaleImages() {
            if (sourceFolder == null || (!overwriteOriginal && destinationFolder == null)) {
                Debug.LogError("Source or destination folder is not set.");
                return;
            }

            string fullSourcePath = AssetDatabase.GetAssetPath(sourceFolder);
            string fullDestinationPath = overwriteOriginal ? fullSourcePath : AssetDatabase.GetAssetPath(destinationFolder);

            if (!Directory.Exists(fullSourcePath)) {
                Debug.LogError("Source folder path does not exist.");
                return;
            }

            if (!overwriteOriginal && !Directory.Exists(fullDestinationPath)) {
                Directory.CreateDirectory(fullDestinationPath);
            }

            string[] files = Directory.GetFiles(fullSourcePath, "*.png", SearchOption.AllDirectories);
            foreach (string file in files) {
                if (scaleDown) {
                    // 缩小
                    ScaleImage(file, fullDestinationPath, overwriteOriginal, 1.0f / scaleDownMultiplier);
                } else {
                    // 放大
                    ScaleImage(file, fullDestinationPath, overwriteOriginal, scaleMultiplier);
                }
            }

            AssetDatabase.Refresh();
        }

        private void ScaleImage(string filePath, string destinationPath, bool overwrite, float scale) {
            Texture2D texture = new Texture2D(2, 2);
            byte[] fileData = File.ReadAllBytes(filePath);
            if (texture.LoadImage(fileData)) {
                int newWidth = Mathf.CeilToInt(texture.width * scale);
                int newHeight = Mathf.CeilToInt(texture.height * scale);
                Texture2D scaledTexture = NearestNeighborScale(texture, newWidth, newHeight);
                byte[] bytes = scaledTexture.EncodeToPNG();

                if (overwrite) {
                    File.WriteAllBytes(filePath, bytes); // Overwrite the original file
                } else {
                    string savePath = Path.Combine(destinationPath, Path.GetFileName(filePath));
                    File.WriteAllBytes(savePath, bytes); // Save to new destination
                }

                Debug.Log($"Scaled and saved image to: {(overwrite ? filePath : Path.Combine(destinationPath, Path.GetFileName(filePath)))}");

                // Clean up
                DestroyImmediate(scaledTexture); // Make sure to destroy the new texture to avoid memory leaks
            } else {
                Debug.LogError($"Failed to load image: {filePath}");
            }
            DestroyImmediate(texture); // Clean up the original texture
        }

        static Texture2D NearestNeighborScale(Texture2D tex, int newWidth, int newHeight) {
            Texture2D newTex = new Texture2D(newWidth, newHeight, tex.format, false);
            for (int y = 0; y < newHeight; y++) {
                for (int x = 0; x < newWidth; x++) {
                    int oldX = (int)(x * (float)tex.width / newWidth);
                    int oldY = (int)(y * (float)tex.height / newHeight);
                    newTex.SetPixel(x, y, tex.GetPixel(oldX, oldY));
                }
            }
            newTex.Apply();
            return newTex;
        }

    }
    
}
#endif