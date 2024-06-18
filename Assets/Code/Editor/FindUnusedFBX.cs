using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FindUnusedFBX : EditorWindow {
    private string[] allFBXFiles;
    private List<string> usedFBXFiles = new List<string>();
    private List<string> unusedFBXFiles = new List<string>();

    [MenuItem("Tools/Find Unused FBX Files")]
    public static void ShowWindow() {
        GetWindow<FindUnusedFBX>("Find Unused FBX Files");
    }

    private void OnGUI() {
        if (GUILayout.Button("Find Unused FBX Files")) {
            FindUnusedFBXFiles();
        }

        if (unusedFBXFiles.Count > 0) {
            GUILayout.Label("Unused FBX Files:");
            foreach (var file in unusedFBXFiles) {
                GUILayout.Label(file);
            }
        }
    }

    private void FindUnusedFBXFiles() {
        allFBXFiles = Directory.GetFiles(Application.dataPath, "*.fbx", SearchOption.AllDirectories);
        usedFBXFiles.Clear();
        unusedFBXFiles.Clear();

        // Find all FBX files used in scenes
        string[] scenePaths = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories);
        foreach (var scenePath in scenePaths) {
            var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath.Substring(Application.dataPath.Length - 6));
            var dependencies = EditorUtility.CollectDependencies(new Object[] { scene });
            foreach (var dependency in dependencies) {
                if (dependency is GameObject) {
                    var model = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(dependency);
                    if (!string.IsNullOrEmpty(model) && model.EndsWith(".fbx")) {
                        usedFBXFiles.Add(model);
                    }
                }
            }
        }

        // Identify unused FBX files
        foreach (var fbxFile in allFBXFiles) {
            var relativePath = "Assets" + fbxFile.Substring(Application.dataPath.Length).Replace("\\", "/");
            if (!usedFBXFiles.Contains(relativePath)) {
                unusedFBXFiles.Add(relativePath);
            }
        }

        Debug.Log("Unused FBX Files Count: " + unusedFBXFiles.Count);
    }
}
