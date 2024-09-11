using System;
using System.Collections;
using System.Reflection;
using System.Xml.Serialization;

using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.TestTools;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

[AddComponentMenu("unity属性")]// 在添加组件面板显示
public class UnityAttribute : MonoBehaviour
{
    [SerializeField]//实例化
    private int int1 = 1;

    [NonSerialized]//不实例化
    public int int2;

    [HideInInspector]//在检查器隐藏
    public int int3;

    [Range(0, 10)]//限制值范围在0~10，在检查器面板生成一个滑动条
    public static int int4;

    [Tooltip("光标悬停在字段时显示这行")]
    public int int5;

    [Header("生成一个分隔标题")]
    public int int6;

    [Space(10)]//与上面的字段空出10单位间隔
    public int int7;

    [FormerlySerializedAs("oldname_int8")]//当字段名改变时，保留旧的字段名以确保反序列化和兼容性
    public int int8;

    [ContextMenuItem("重置int9的值", "Action1")]//右键该字段可以触发Action1
    public int int9 = 1;

    //[SerializationReference]
    //public UnityAttribute2 unityAttribute2;

    #region 给方法

    [ContextMenu("重置int1的值")]//可以从类组件的右边三个点直接调用该脚本
    private void ResetValues()
    {
        int1 = 0;
    }

    [GUITarget(1)]
    private void SetGUI()
    {
    }

    [Obsolete]//标记已过时的方法 或 类
    private void Action1()
    {
        int9 = 0;
    }

    [RuntimeInitializeOnLoadMethod]//运行时初始化时调用
    private static void Action2()
    {
        int4 = 2;
        Debug.Log("RunTime Action");
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]//自定义Gizmos绘制 //绘制一个仅在场景视图可见的物体，可以通过场景小工具按钮控制显隐
    private static void Action3(UnityAttribute scr, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(scr.transform.position + new Vector3(1, 1, 1), new Vector3(.1f, .1f, .1f));//绘制一个cube（位置，大小）
    }

    [InitializeOnLoadMethod]//在编译器加载时调用静态函数
    private static void Action4()
    {
        Debug.Log("Edier methor loaded");
    }

    [PostProcessBuild] // 在构造完成后调用该方法
    [PostProcessScene] // 在场景处理后调用该方法
    private void Action5()
    {
        Debug.Log("构建完成 ，场景处理完成");
    }

    //[UnityTest] //定义一个unity测试方法
    //IEnumerator Action6()
    //{
    //    yield return 0;
    //    Assert.Pass();
    //}

    #endregion 给方法
}

//[ExecuteInEditMode]//编译器模式下运行
//[RequireComponent(typeof(Light))]//自动添加指定的组件
//[DisallowMultipleComponent]//防止对象上添加多个相同的组件
//[CreateAssetMenu(fileName = "新建数据", menuName = "Custom/Data")]//在unity菜单栏添加一个选项
//[ImageEffectAllowedInSceneView]//允许图像效果在场景视图中生效
//[InitializeOnLoad]//编译器加载时初始化
//[SelectionBase]//使GameObject在检查器面板作为选择的基础//在检查器选择这个组件的子对象时会选择GameObject
//public class UnityAttribute2 : MonoBehaviour
//{
//}

////[CustomEditor()]

//[DefaultExecutionOrder(1000)]//设置脚本的默认执行顺序
//public class UnityAttribute3 : ScriptableObject
//{
//}

//自定义属性  AttributeUsage指该属性可以应用的目标（类，方法，字段等），是否可以继承，是否可以多次使用
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class UnityAttribute4 : Attribute
{
    public string Desctiption { get; }  //Desctiption是该属性的参数，可以添加多个

    public UnityAttribute4(string desctiption)
    {
        Desctiption = desctiption;
    }
}

//自定义属性的处理
public class Attributeteader
{
    private Type type = typeof(UnityAttribute5);

    private void OnAttributeEnt()
    {
        if (Attribute.IsDefined(type, typeof(UnityAttribute4)))
        {
            UnityAttribute4 unityAttribute4 = (UnityAttribute4)Attribute.GetCustomAttribute(type, typeof(UnityAttribute4));
            Debug.Log($"field Attribute: {unityAttribute4.Desctiption}");
        }

        FieldInfo fieldInfo = type.GetField("myField");
        if (fieldInfo != null && Attribute.IsDefined(fieldInfo, typeof(UnityAttribute4)))
        {
            UnityAttribute4 unityAttribute4 = (UnityAttribute4)Attribute.GetCustomAttribute(fieldInfo, typeof(UnityAttribute4));
            Debug.Log($"field attribute; {unityAttribute4.Desctiption}");
        }

        FieldInfo fieldInfo1 = type.GetField("myMethod");
    }
}

[UnityAttribute4("aabc")]
public class UnityAttribute5 : MonoBehaviour
{
    [UnityAttribute4("a5 int1")]
    public int int51 = 0;

    [UnityAttribute4("Action51 logint51")]
    public void Action51()
    {
        Debug.Log(int51.ToString());
    }
}

//[AssemblyIsEditorAssembly]  //自动生成 仅对程序集有效