using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

public class SceneSearchTool : EditorWindow
{
    private string searchQuery = "";
    private List<string> scenePaths = new List<string>();

    [MenuItem("Tools/Scene Search Tool")]
    public static void ShowWindow()
    {
        GetWindow<SceneSearchTool>("Scene Search Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("씬 검색", EditorStyles.boldLabel);

        // 이전 검색어와 비교하여 변경된 경우에만 검색 실행
        string newSearchQuery = EditorGUILayout.TextField("검색어", searchQuery);
        if (newSearchQuery != searchQuery)
        {
            searchQuery = newSearchQuery;
            SearchScenes();
        }

        // 검색된 씬 경로를 버튼으로 표시
        foreach (string scenePath in scenePaths)
        {
            if (GUILayout.Button(scenePath))
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }

    private void SearchScenes()
    {
        scenePaths.Clear();
        string[] allScenes = AssetDatabase.FindAssets("t:Scene");

        foreach (string guid in allScenes)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase))
            {
                scenePaths.Add(path);
            }
        }
    }
}
