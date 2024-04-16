#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Chouten.Modifier {

    public class PngSlicerEditor : EditorWindow {

        private DefaultAsset sourceFolder;
        private DefaultAsset destinationFolder;
        private int tileWidth = 16;
        private int tileHeight = 16;
        private bool verticalReverse = false;
        private bool horizontalReverse = false;

        [MenuItem("Tools/PNG Slicer")]
        public static void ShowWindow() {
            GetWindow<PngSlicerEditor>("PNG Slicer");
        }

        void OnGUI() {
            GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            sourceFolder = (DefaultAsset)EditorGUILayout.ObjectField("Source Folder", sourceFolder, typeof(DefaultAsset), false);
            destinationFolder = (DefaultAsset)EditorGUILayout.ObjectField("Destination Folder", destinationFolder, typeof(DefaultAsset), false);
            tileWidth = EditorGUILayout.IntField("Tile Width", tileWidth);
            tileHeight = EditorGUILayout.IntField("Tile Height", tileHeight);
            verticalReverse = EditorGUILayout.Toggle("Reverse Vertically", verticalReverse);
            horizontalReverse = EditorGUILayout.Toggle("Reverse Horizontally", horizontalReverse);

            if (GUILayout.Button("Slice PNGs")) {
                SlicePngs();
            }
        }

        private void SlicePngs() {
            if (sourceFolder == null || destinationFolder == null) {
                Debug.LogError("Source or destination path is not set.");
                return;
            }

            string fullSourcePath = AssetDatabase.GetAssetPath(sourceFolder);
            string fullDestinationPath = AssetDatabase.GetAssetPath(destinationFolder);

            if (!Directory.Exists(fullSourcePath)) {
                Debug.LogError("Source path does not exist.");
                return;
            }

            if (!Directory.Exists(fullDestinationPath)) {
                Directory.CreateDirectory(fullDestinationPath);
            }

            string[] files = Directory.GetFiles(fullSourcePath, "*.png");
            foreach (string file in files) {
                SlicePng(file, fullDestinationPath);
            }
        }

        private void SlicePng(string filePath, string destinationPath) {
            Texture2D sourceImage = LoadPNG(filePath);
            if (sourceImage == null) return;

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            int id = 0;

            int xStart = 0, xEnd = sourceImage.width, xStep = tileWidth;
            int yStart = 0, yEnd = sourceImage.height, yStep = tileHeight;

            if (horizontalReverse) {
                xStart = sourceImage.width - tileWidth;
                xEnd = -tileWidth;
                xStep = -tileWidth;
            }

            if (verticalReverse) {
                yStart = sourceImage.height - tileHeight;
                yEnd = -tileHeight;
                yStep = -tileHeight;
            }

            for (int y = yStart; verticalReverse ? y >= 0 : y + tileHeight <= sourceImage.height; y += yStep) {
                for (int x = xStart; horizontalReverse ? x >= 0 : x + tileWidth <= sourceImage.width; x += xStep) {
                    Texture2D tile = new Texture2D(tileWidth, tileHeight);
                    var pixels = sourceImage.GetPixels(x, y, tileWidth, tileHeight);
                    tile.SetPixels(pixels);
                    tile.Apply();

                    if (!IsTextureTransparent(tile)) {
                        byte[] bytes = tile.EncodeToPNG();
                        File.WriteAllBytes(Path.Combine(destinationPath, $"{fileName}_{id}.png"), bytes);
                        id++;
                    }

                    DestroyImmediate(tile);
                }
            }
        }

        private bool IsTextureTransparent(Texture2D texture) {
            Color[] pixels = texture.GetPixels();
            foreach (Color pixel in pixels) {
                if (pixel.a != 0)
                    return false;
            }
            return true;
        }

        private Texture2D LoadPNG(string filePath) {
            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath)) {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); // This will auto-resize the texture dimensions.
            }
            return tex;
        }

    }

}
#endif