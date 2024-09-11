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

[AddComponentMenu("unity����")]// �������������ʾ
public class UnityAttribute : MonoBehaviour
{
    [SerializeField]//ʵ����
    private int int1 = 1;

    [NonSerialized]//��ʵ����
    public int int2;

    [HideInInspector]//�ڼ��������
    public int int3;

    [Range(0, 10)]//����ֵ��Χ��0~10���ڼ�����������һ��������
    public static int int4;

    [Tooltip("�����ͣ���ֶ�ʱ��ʾ����")]
    public int int5;

    [Header("����һ���ָ�����")]
    public int int6;

    [Space(10)]//��������ֶοճ�10��λ���
    public int int7;

    [FormerlySerializedAs("oldname_int8")]//���ֶ����ı�ʱ�������ɵ��ֶ�����ȷ�������л��ͼ�����
    public int int8;

    [ContextMenuItem("����int9��ֵ", "Action1")]//�Ҽ����ֶο��Դ���Action1
    public int int9 = 1;

    //[SerializationReference]
    //public UnityAttribute2 unityAttribute2;

    #region ������

    [ContextMenu("����int1��ֵ")]//���Դ���������ұ�������ֱ�ӵ��øýű�
    private void ResetValues()
    {
        int1 = 0;
    }

    [GUITarget(1)]
    private void SetGUI()
    {
    }

    [Obsolete]//����ѹ�ʱ�ķ��� �� ��
    private void Action1()
    {
        int9 = 0;
    }

    [RuntimeInitializeOnLoadMethod]//����ʱ��ʼ��ʱ����
    private static void Action2()
    {
        int4 = 2;
        Debug.Log("RunTime Action");
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]//�Զ���Gizmos���� //����һ�����ڳ�����ͼ�ɼ������壬����ͨ������С���߰�ť��������
    private static void Action3(UnityAttribute scr, GizmoType gizmoType)
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(scr.transform.position + new Vector3(1, 1, 1), new Vector3(.1f, .1f, .1f));//����һ��cube��λ�ã���С��
    }

    [InitializeOnLoadMethod]//�ڱ���������ʱ���þ�̬����
    private static void Action4()
    {
        Debug.Log("Edier methor loaded");
    }

    [PostProcessBuild] // �ڹ�����ɺ���ø÷���
    [PostProcessScene] // �ڳ����������ø÷���
    private void Action5()
    {
        Debug.Log("������� �������������");
    }

    //[UnityTest] //����һ��unity���Է���
    //IEnumerator Action6()
    //{
    //    yield return 0;
    //    Assert.Pass();
    //}

    #endregion ������
}

//[ExecuteInEditMode]//������ģʽ������
//[RequireComponent(typeof(Light))]//�Զ����ָ�������
//[DisallowMultipleComponent]//��ֹ��������Ӷ����ͬ�����
//[CreateAssetMenu(fileName = "�½�����", menuName = "Custom/Data")]//��unity�˵������һ��ѡ��
//[ImageEffectAllowedInSceneView]//����ͼ��Ч���ڳ�����ͼ����Ч
//[InitializeOnLoad]//����������ʱ��ʼ��
//[SelectionBase]//ʹGameObject�ڼ���������Ϊѡ��Ļ���//�ڼ����ѡ�����������Ӷ���ʱ��ѡ��GameObject
//public class UnityAttribute2 : MonoBehaviour
//{
//}

////[CustomEditor()]

//[DefaultExecutionOrder(1000)]//���ýű���Ĭ��ִ��˳��
//public class UnityAttribute3 : ScriptableObject
//{
//}

//�Զ�������  AttributeUsageָ�����Կ���Ӧ�õ�Ŀ�꣨�࣬�������ֶεȣ����Ƿ���Լ̳У��Ƿ���Զ��ʹ��
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class UnityAttribute4 : Attribute
{
    public string Desctiption { get; }  //Desctiption�Ǹ����ԵĲ�����������Ӷ��

    public UnityAttribute4(string desctiption)
    {
        Desctiption = desctiption;
    }
}

//�Զ������ԵĴ���
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

//[AssemblyIsEditorAssembly]  //�Զ����� ���Գ�����Ч