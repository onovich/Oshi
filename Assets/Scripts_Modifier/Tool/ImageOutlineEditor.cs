#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Alter.Modifier {

    public class ImageOutlineEditor : EditorWindow {
        private Texture2D selectedImage;
        private Color outlineColor = Color.white;
        private bool overwriteOriginal = false;

        [MenuItem("Tools/Image Outline Processor")]
        public static void ShowWindow() {
            GetWindow<ImageOutlineEditor>("Image Outline Processor");
        }

        private void OnGUI() {
            GUILayout.Label("Select Image", EditorStyles.boldLabel);
            selectedImage = (Texture2D)EditorGUILayout.ObjectField("Image", selectedImage, typeof(Texture2D), false);

            outlineColor = EditorGUILayout.ColorField("Outline Color", outlineColor);
            overwriteOriginal = EditorGUILayout.Toggle("Overwrite Original", overwriteOriginal);

            if (GUILayout.Button("Process Image") && selectedImage != null) {
                string path = AssetDatabase.GetAssetPath(selectedImage);
                bool originalIsReadable = SetTextureReadable(path, true); // Make texture readable
                Texture2D outlinedImage = AddOutlineToImage(selectedImage, outlineColor);
                SaveOutlinedImage(outlinedImage, path, overwriteOriginal);
                SetTextureReadable(path, originalIsReadable); // Restore original texture readability
            }
        }

        private Texture2D AddOutlineToImage(Texture2D image, Color outlineColor) {
            int width = image.width;
            int height = image.height;
            Texture2D outlinedImage = new Texture2D(width, height, TextureFormat.ARGB32, false);
            outlinedImage.filterMode = image.filterMode;

            // Copy the original image
            CopyImage(image, outlinedImage);

            // Add outline within the original image dimensions
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    if (image.GetPixel(x, y).a > 0.1f) {
                        AddOutlinePixel(image, outlinedImage, x, y, outlineColor);
                    }
                }
            }

            outlinedImage.Apply();
            return outlinedImage;
        }

        private void CopyImage(Texture2D source, Texture2D destination) {
            int width = source.width;
            int height = source.height;

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    destination.SetPixel(x, y, source.GetPixel(x, y));
                }
            }
        }

        private void AddOutlinePixel(Texture2D source, Texture2D destination, int x, int y, Color outlineColor) {
            int width = source.width;
            int height = source.height;

            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    int nx = x + i;
                    int ny = y + j;

                    if ((i == 0 && j == 0) || nx < 0 || nx >= width || ny < 0 || ny >= height || Mathf.Abs(i) == Mathf.Abs(j))
                        continue;

                    if (source.GetPixel(nx, ny).a <= 0.1f) {
                        destination.SetPixel(nx, ny, outlineColor);
                    }
                }
            }
        }

        private void SaveOutlinedImage(Texture2D image, string originalPath, bool overwrite) {
            byte[] bytes = image.EncodeToPNG();
            string directory = Path.GetDirectoryName(originalPath);
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(originalPath);
            string newFilePath = overwrite ? originalPath : Path.Combine(directory, $"{filenameWithoutExtension}_outlined.png");

            File.WriteAllBytes(newFilePath, bytes);

            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(newFilePath, ImportAssetOptions.ForceUpdate);

            ApplyOriginalTextureSettings(newFilePath, originalPath);

            AssetDatabase.Refresh();
        }

        private void ApplyOriginalTextureSettings(string newFilePath, string originalPath) {
            var originalImporter = AssetImporter.GetAtPath(originalPath) as TextureImporter;
            var newImporter = AssetImporter.GetAtPath(newFilePath) as TextureImporter;
            if (originalImporter != null && newImporter != null) {
                newImporter.spritePixelsPerUnit = originalImporter.spritePixelsPerUnit;
                newImporter.maxTextureSize = originalImporter.maxTextureSize;
                newImporter.textureCompression = originalImporter.textureCompression;
                newImporter.filterMode = originalImporter.filterMode;
                var settings = originalImporter.GetDefaultPlatformTextureSettings();
                newImporter.SetPlatformTextureSettings(settings);

                newImporter.SaveAndReimport();
            }
        }

        private bool SetTextureReadable(string assetPath, bool isReadable) {
            var textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (textureImporter != null) {
                bool originalIsReadable = textureImporter.isReadable;
                if (textureImporter.isReadable != isReadable) {
                    textureImporter.isReadable = isReadable;
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
                return originalIsReadable;
            }
            return false;
        }

    }

}
#endif