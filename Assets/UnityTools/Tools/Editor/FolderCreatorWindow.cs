#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// 文件夹生成
/// </summary>
public class FolderCreatorWindow : EditorWindow
{
    private string folderName = "NewFolder";
    private string selectedPath = "Assets";

    [MenuItem("Tools/生成自定义文件夹")]
    public static void ShowWindow()
    {
        GetWindow(typeof(FolderCreatorWindow), false, "文件夹生成");
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("确认名称和路径:", EditorStyles.boldLabel);
        folderName = EditorGUILayout.TextField("文件夹名称:", folderName);
        GUILayout.Space(10);
        selectedPath = EditorGUILayout.TextField("选择路径:", selectedPath);
        if (GUILayout.Button("选择路径"))
        {
            selectedPath = EditorUtility.OpenFolderPanel("选择路径", selectedPath, "Assets");
        }
        GUILayout.Space(10);

        if (GUILayout.Button("开始生成"))
        {
            CreateFolderStructure(folderName, selectedPath);
        }
    }

    private void CreateFolderStructure(string folderName, string selectedPath)
    {
        string directoryPath = Path.Combine(selectedPath, folderName);
        Directory.CreateDirectory(directoryPath);

        string[] subFolders = { "Scripts", "Prefabs", "Models", "Materials", "Textures", "Shaders", "Screen" };
        foreach (string subFolder in subFolders)
        {
            string subFolderPath = Path.Combine(directoryPath, subFolder);
            Directory.CreateDirectory(subFolderPath);
        }

        AssetDatabase.Refresh();
        Debug.Log("文件夹生成在: " + directoryPath);
    }
}

#endif