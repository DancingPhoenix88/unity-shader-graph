using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEditor;

namespace ZeroVector.Common {
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Texture2DArrayTools : EditorWindow {
        private bool exportFoldout = false;
        private Texture2DArray asset;
        private ExportType exportType = ExportType.PNG;
        private TargetFormat targetFormat = TargetFormat.RGBA32;

        private bool importFoldout = false;
        private bool texArrayFoldout = true;
        // God I hate this
        public List<Texture2D> importArray = new List<Texture2D>();
        private string exportPath;
        private TargetFormat encodeFormat = TargetFormat.RGBA32;
        private bool useMips = true;


        private Vector2 scrollPos; // ffs....

        private enum ExportType {
            PNG,
            TGA,
            EXR,
            JPG,
        }

        private enum TargetFormat {
            RGBA32 = TextureFormat.RGBA32,
            ARGB32 = TextureFormat.ARGB32,
            RGB24 = TextureFormat.RGB24,
            Alpha8 = TextureFormat.Alpha8
        }


        [MenuItem("Tools/Texture Array Tools")]
        private static void Init() {
            // Get existing open window or if none, make a new one:
            var window = (Texture2DArrayTools)GetWindow(typeof(Texture2DArrayTools));
            window.titleContent = new GUIContent("Texture Array Tools");
            window.minSize = new Vector2(350, 200);
            window.ShowUtility();
        }

        private void OnGUI() {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            exportFoldout = EditorGUILayout.Foldout(exportFoldout, "Extract Textures");
            if (exportFoldout) {
                EditorGUI.indentLevel += 1;
                // EditorGUILayout.LabelField("Export Array", EditorStyles.boldLabel);
                asset = (Texture2DArray)EditorGUILayout.ObjectField("Array asset", asset, typeof(Texture2DArray),
                    false);

                var assetPath = asset == null ? "" : AssetDatabase.GetAssetPath(asset);
                var targetExtractDir = asset == null ? "" : Path.GetDirectoryName(assetPath) + "\\ExportedTextures";
                if (asset != null) {
                    EditorGUILayout.LabelField(
                        $"Array: {asset.depth}x {asset.format.ToString()}, {asset.width}x{asset.height}");
                    EditorGUILayout.LabelField($"{assetPath}");
                }
                else EditorGUILayout.LabelField("No array referenced.");

                exportType = (ExportType)EditorGUILayout.EnumPopup("Export type", exportType);
                targetFormat = (TargetFormat)EditorGUILayout.EnumPopup("Format", targetFormat);

                using (new EditorGUI.DisabledScope(asset == null)) {
                    if (GUILayout.Button("Extract textures")) Extract(assetPath, targetExtractDir);
                }

                EditorGUILayout.LabelField("Exporting textures to:");
                EditorGUILayout.LabelField($"{targetExtractDir}");

                EditorGUI.indentLevel -= 1;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            importFoldout = EditorGUILayout.Foldout(importFoldout, "Merge Textures");
            // ReSharper disable once InvertIf
            if (importFoldout) {
                EditorGUI.indentLevel += 1;

                EditorGUILayout.LabelField("All textures must be of the same size and format.");
                EditorGUILayout.LabelField("(Whether that is the case will not be validated.)");

                // TEXTURES
                EditorGUILayout.Space();
                texArrayFoldout = EditorGUILayout.Foldout(texArrayFoldout, "Textures");
                if (texArrayFoldout) {
                    EditorGUI.indentLevel += 1;
                    var oldArrSize = importArray.Count;
                    var size = EditorGUILayout.IntField("Size", oldArrSize);
                    if (oldArrSize != size) {
                        if (size < 0) size = 0;
                        // Resizing an array. Because the Unity folks couldn't be arsed to make a fucking method to do this for us, apparently.
                        if (size > oldArrSize)
                            importArray.AddRange(Enumerable.Repeat<Texture2D>(null, size - oldArrSize));
                        else importArray.RemoveRange(size, oldArrSize - size);
                    }
                    // Now draw all the textures and so on.
                    for (var i = 0; i < importArray.Count; i++) {
                        importArray[i] = (Texture2D)EditorGUILayout.ObjectField($"Element {i}", importArray[i],
                            typeof(Texture2D), false);
                    }
                    EditorGUI.indentLevel -= 1;
                }

                // OTHER SHIT
                useMips = EditorGUILayout.Toggle("Use mips", useMips);
                encodeFormat = (TargetFormat)EditorGUILayout.EnumPopup("Encoding format", encodeFormat);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField($"Target path: {exportPath}");
                if (GUILayout.Button("Choose new destination")) {
                    exportPath = EditorUtility.SaveFilePanel(
                        "Save textures to an .asset array",
                        exportPath == "" ? Application.dataPath : exportPath,
                        "textureArray.asset",
                        "asset");
                }
                EditorGUILayout.LabelField("NB: You MUST save this in Assets or a subdirectory.");

                EditorGUILayout.Space();

                using (new EditorGUI.DisabledScope(exportPath == "" || importArray?.Count(x => x != null) < 1)) {
                    if (GUILayout.Button("Merge")) Merge();
                }

                EditorGUI.indentLevel -= 1;


                EditorGUI.indentLevel -= 1;
            }

            EditorGUILayout.EndScrollView();
        }

        private void Extract(string baseSourcePath, string baseTargetDir) {
            if (asset == null) return;

            if (!Directory.Exists(baseTargetDir)) Directory.CreateDirectory(baseTargetDir);
            var baseTargetPath = baseTargetDir + "\\" + Path.GetFileName(baseSourcePath);

            for (var i = 0; i < asset.depth; i++) {
                var tex = new Texture2D(asset.width, asset.height, (TextureFormat)targetFormat, true);
                tex.SetPixels(asset.GetPixels(i));
                tex.Apply();
                var texTargetPath = baseTargetPath + $"_tex_{i}.png";
                if (File.Exists(texTargetPath)) File.Delete(texTargetPath);

                var t = tex.EncodeToJPG();

                // ReSharper disable once ReturnTypeCanBeEnumerable.Local
                byte[] EncodeTex() {
                    switch (exportType) {
                    case ExportType.PNG: return tex.EncodeToPNG();
                    case ExportType.TGA: return tex.EncodeToTGA();
                    case ExportType.EXR: return tex.EncodeToEXR();
                    case ExportType.JPG: return tex.EncodeToJPG();
                    default:
                        throw new ArgumentOutOfRangeException();
                    }
                }

                File.WriteAllBytes(texTargetPath, EncodeTex());
            }

            Debug.Log("All textures extracted.");
        }

        private void Merge() {
            // Filter out any nulls in the list.
            var textureList = importArray.Where(x => x != null).ToList();

            // Make sure we're trying to export to somewhere in Assets
            if (!IsSubDirectoryOf(exportPath, Application.dataPath)) {
                Debug.LogError("Your destination folder does not appear to be in your project's Assets folder.");
                return;
            }

            if (textureList.Count < 1) return;
            if (File.Exists(exportPath)) File.Delete(exportPath);

            var array = new Texture2DArray(textureList[0].width, textureList[0].height, textureList.Count,
                (TextureFormat)encodeFormat, useMips);

            // Go over all the textures
            for (var i = 0; i < textureList.Count; i++) {
                // And over all the mips, if needed
                for (var j = 0; j < textureList[i].mipmapCount; j++) {
                    array.SetPixels(textureList[i].GetPixels(j), i, j);

                    // Only do mip 0 (the normal tex) if we don't want mips
                    if (!useMips) break;
                }
            }
            array.Apply();

            // CreateAsset only works with "local paths", local to the Assets folder, that is. Damn it, Unity.
            var localPath = "Assets" + exportPath.Remove(0, Application.dataPath.Length); // I'm a hacker
            AssetDatabase.CreateAsset(array, localPath);

            Debug.Log("All textures merged.");
        }


        // https://stackoverflow.com/a/23354773/668143
        private static bool IsSubDirectoryOf(string candidate, string other) {
            var isChild = false;
            try {
                var candidateInfo = new DirectoryInfo(candidate);
                var otherInfo = new DirectoryInfo(other);

                while (candidateInfo.Parent != null) {
                    if (candidateInfo.Parent.FullName == otherInfo.FullName) {
                        isChild = true;
                        break;
                    }
                    else candidateInfo = candidateInfo.Parent;
                }
            }
            catch (Exception error) {
                Debug.Log($"Unable to verify directories {candidate} and {other}: {error}");
                return false;
            }

            return isChild;
        }
    }
}