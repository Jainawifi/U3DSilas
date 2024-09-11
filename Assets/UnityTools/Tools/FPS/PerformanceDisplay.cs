using UnityEngine;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

/// <summary>
///  检测Fps和内存占用
/// </summary>
public class PerformanceDisplay : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private GUIStyle guiStyle = new GUIStyle();
    private Rect rect;


    private void Start()
    {
        int w = Screen.width, h = Screen.height;
        rect = new Rect(0, 0, w, h * 2 / 100);
        guiStyle.alignment = TextAnchor.UpperLeft;
        guiStyle.fontSize = h * 2 / 50;
        guiStyle.normal.textColor = Color.white;
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        float fps = 1.0f / deltaTime;
        long totalMemory = System.GC.GetTotalMemory(false) / (1024 * 1024); // 转换为 MB
        Process currentProcess = Process.GetCurrentProcess();
        long privateMemorySize = currentProcess.PrivateMemorySize64 / (1024 * 1024); // 转换为 MB

        string systemMemoryInfo = "";

        string text = string.Format("FPS: {0:0.} \nTotal Memory: {1} MB \nPrivate Memory Size: {2} MB{3}", fps, totalMemory, privateMemorySize, systemMemoryInfo);
        GUI.Label(rect, text, guiStyle);
    }
}