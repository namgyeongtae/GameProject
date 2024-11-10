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
        GUILayout.Label("�� �˻�", EditorStyles.boldLabel);

        // ���� �˻���� ���Ͽ� ����� ��쿡�� �˻� ����
        string newSearchQuery = EditorGUILayout.TextField("�˻���", searchQuery);
        if (newSearchQuery != searchQuery)
        {
            searchQuery = newSearchQuery;
            SearchScenes();
        }

        // �˻��� �� ��θ� ��ư���� ǥ��
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
