#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
/// <summary>
/// shader替换工具  在Tools选项
/// </summary>
public class ShaderReplacer : EditorWindow
{
    private Shader shader1;
    private Shader shader2;

    [MenuItem("Tools/Replace Shaders")]
    public static void ShowWindow()
    {
        GetWindow<ShaderReplacer>("Replace Shaders");
    }

    private void OnGUI()
    {
        GUILayout.Label("Shader Replacement Tool", EditorStyles.boldLabel);

        shader1 = (Shader)EditorGUILayout.ObjectField("Shader to Replace", shader1, typeof(Shader), false);
        shader2 = (Shader)EditorGUILayout.ObjectField("New Shader", shader2, typeof(Shader), false);

        if (GUILayout.Button("Replace Shaders"))
        {
            ReplaceShaders();
        }
    }

    private void ReplaceShaders()
    {
        if (shader1 == null || shader2 == null)
        {
            Debug.LogError("Both shaders must be assigned.");
            return;
        }

        int materialCount = 0;

        // Find all materials in the project
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material.shader == shader1)
            {
                material.shader = shader2;
                materialCount++;
                EditorUtility.SetDirty(material);
            }
        }

        AssetDatabase.SaveAssets();

        Debug.Log($"Replaced shaders on {materialCount} materials.");
    }
}

#endif