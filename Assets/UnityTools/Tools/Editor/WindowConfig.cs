#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
public class WindowConfig : EditorWindow
{
    [MenuItem("Tools/TestWindow/ Config01")]
    public static void ShowWindow()
    {
        WindowConfig windows = GetWindow<WindowConfig>();
        windows.titleContent = new GUIContent("ConfigEditor _Main");
    }

    private bool toggleState = false;
    private int scorllp;
    private float vLine = 140f;
    private string testInput = " input Text";
    private string[] options = new string[] { "option01", "option02", "option03", "option04", "option05" };

    private Vector2 optionPos = Vector2.zero;

    private GUIStyle gUIS0;
    private GUIStyle gUIS1;
    private GUIStyle gUIS2;

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();//横线,让所有在end之前的内容横向排列
        EditorGUILayout.BeginVertical(GUILayout.Width(vLine));//竖线------------------------------------------------------------------------------------------------------

        GUISpace();
        EditorGUILayout.LabelField("Button");
        if (GUILayout.Button("GUI按钮左", GUILayout.Width(100), GUILayout.Height(30)))//按钮
        {
            Debug.Log("Button is left");
        }

        GUISpace();
        EditorGUILayout.LabelField(" Text input,Text input,Text input,Text input,Text input,Text input,Text input,Text input,Text input");//生成串文本

        GUISpace();
        EditorGUILayout.LabelField(" downMenu");
        scorllp = EditorGUILayout.Popup(scorllp, options);//要使用int = 的方式来赋值生成下拉框,否则会无法选取

        GUISpace();
        GUILayout.Label("this's a scroll", EditorStyles.boldLabel);
        optionPos = EditorGUILayout.BeginScrollView(optionPos);//要使用vector2 = 的方法来赋值生成滑动视图,否则滑动条无法使用
        for (int i = 0; i < 10; i++)
        {
            GUILayout.Label("this's _" + i + " _index");
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();//-----------------------------------------------------------------------------------------------------------

        #region 分隔线========================================================================================================================================

        GUISpace();//横向的空格
        //DrawBoxLine();
        //DrawVerticaLine();
        DeawLineDirect(Color.black, new Vector2(vLine + 80, 20), new Vector2(vLine + 80, position.height - 10));

        #endregion 分隔线========================================================================================================================================

        EditorGUILayout.BeginVertical(GUILayout.Width(position.width - 250f));//竖线------------------------------------------------------------------------------------------------------

        GUISpace();
        EditorGUILayout.LabelField(" Button");
        if (gUIS0 == null)
        {
            gUIS0 = new GUIStyle(GUI.skin.button);
        }//按钮
        else
        {
            TextColorChange(gUIS0, Color.green, Color.gray, Color.red);
            BackGroundColorChange(gUIS0, Color.red, Color.gray, Color.green);
            if (GUILayout.Button("GUI按钮右", gUIS0, GUILayout.Width(100), GUILayout.Height(30)))//按钮,附带修改按钮的样式
            {
                Debug.Log("Button is right");
            }
        }

        GUISpace();
        EditorGUILayout.LabelField(" scrollP");
        if (gUIS1 == null)
        {
            gUIS1 = new GUIStyle(GUI.skin.textField);//输入框默认皮肤
        }//输入框
        else
        {
            TextColorChange(gUIS1, Color.red, Color.white, Color.blue);
            gUIS1.fixedHeight = 40;
            gUIS1.fixedWidth = 500;
            gUIS1.fontSize = 16;
            gUIS1.fontStyle = FontStyle.Bold;
            testInput = EditorGUILayout.TextField(testInput, gUIS1);
        }

        GUISpace();
        EditorGUILayout.LabelField("switch");
        if (gUIS2 == null)
        {
            gUIS2 = new GUIStyle(GUI.skin.toggle);//开关默认皮肤
        }//开关
        else
        {
            //BackGroundColorChange(gUIS2, Color.red, Color.green, Color.white);
            //OnBackGroundColorChange(gUIS2, Color.green, Color.red, Color.gray);
            toggleState = EditorGUILayout.Toggle(toggleState, gUIS2);
        }

        EditorGUILayout.EndVertical();//-----------------------------------------------------------------------------------------------------------
        EditorGUILayout.EndHorizontal();

        //EditorGUILayout.BeginBuildTargetSelectionGrouping();
        //EditorGUILayout.EndBuildTargetSelectionGrouping();

        //EditorGUILayout.BeginFoldoutHeaderGroup();
        //EditorGUILayout.EndFoldoutHeaderGroup();

        //EditorGUILayout.BeginScrollView();
        //EditorGUILayout.EndScrollView();

        //EditorGUILayout.BeginVertical();
        //EditorGUILayout.EndVertical();

        //EditorGUILayout.BeginFadeGroup();
        //EditorGUILayout.EndFadeGroup();

        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.EndHorizontal();

        //EditorGUILayout.BeginToggleGroup();
        //EditorGUILayout.EndToggleGroup();
    }

    /// <summary>
    /// 空出一段距离
    /// </summary>
    /// <param name="spacing"></param>
    private void GUISpace(int spacing = 20)
    {
        GUILayout.Space(spacing);
    }

    /// <summary>
    /// 用宽度为1像素的矩形模拟分隔线
    /// </summary>
    private void DrawVerticaLine(float lineLeight, Color lineColor)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, lineLeight);//获取一个矩形
        rect.width = 1;
        EditorGUI.DrawRect(rect, lineColor);
    }

    /// <summary>
    /// 用宽度为1像素的Box模拟分隔线
    /// </summary>
    private void DrawBoxLine()
    {
        GUILayout.Box("????", GUILayout.Height(position.height), GUILayout.Width(1));//用宽度为1像素的box模拟细分隔线
    }

    /// <summary>
    /// 直接绘制一条线
    /// </summary>
    /// <param name="lineColor"></param>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    private void DeawLineDirect(Color lineColor, Vector2 startPos, Vector2 endPos)
    {
        Handles.color = lineColor;//设置线条颜色
        Handles.DrawLine(startPos, endPos);//直接绘制竖线
        Handles.color = default;//还原颜色
    }

    /// <summary>
    /// 默认状态下的 文本颜色修改
    /// </summary>
    /// <param name="gUIStyle"></param>
    /// <param name="normal"></param>
    /// <param name="active"></param>
    private void TextColorChange(GUIStyle gUIStyle, Color normal, Color hover, Color active)
    {
        gUIStyle.normal.textColor = normal;
        gUIStyle.hover.textColor = hover;
        gUIStyle.active.textColor = active;
    }

    /// <summary>
    /// 默认状态下的 背景颜色修改
    /// </summary>
    /// <param name="gUIStyle"></param>
    /// <param name="normal"></param>
    /// <param name="hover"></param>
    /// <param name="active"></param>
    /// <param name="multiplier"></param>//               按下之前颜色    按下后的颜色   按下时的颜色
    private void BackGroundColorChange(GUIStyle gUIStyle, Color normal, Color hover, Color active, float multiplier = 0.5f)
    {
        gUIStyle.normal.background = MakeTex(normal * multiplier);
        gUIStyle.hover.background = MakeTex(hover * multiplier);
        gUIStyle.active.background = MakeTex(active * multiplier);
    }

    /// <summary>
    ///  激活状态下的 背景颜色修改
    /// </summary>
    /// <param name="gUIStyle"></param>
    /// <param name="normal"></param>
    /// <param name="hover"></param>
    /// <param name="active"></param>
    /// <param name="multiplier"></param>//                 按下之前颜色    按下后的颜色   按下时的颜色
    private void OnBackGroundColorChange(GUIStyle gUIStyle, Color normal, Color hover, Color active, float multiplier = 0.5f)
    {
        gUIStyle.onNormal.background = MakeTex(normal * multiplier);
        gUIStyle.onHover.background = MakeTex(hover * multiplier);
        gUIStyle.onActive.background = MakeTex(active * multiplier);
    }

    /// <summary>
    /// 创建一个2D纹理图像,给像素点赋值颜色
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    private Texture2D MakeTex(Color color, int w = 1, int h = 1)
    {
        Color[] pix = new Color[w * h];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = color;
        }
        Texture2D result = new Texture2D(w, h);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}

#endif