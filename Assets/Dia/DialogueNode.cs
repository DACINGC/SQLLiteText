using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "�Ի�ϵͳ/�Ի��ڵ�", order = 1)]
public class DialogueNode : ScriptableObject
{
    [Header("�ڵ�ID")]
    [Tooltip("�ڵ��Ψһ��ʶ��")]
    public string nodeID;

    [Header("˵��������")]
    [Tooltip("��ǰ�Ի��ķ���������")]
    public string speakerName;

    [Header("�Ի�����")]
    [Tooltip("��ǰ�ڵ���ʾ�ĶԻ��ı�")]
    [TextArea(3, 10)] public string dialogueText;

    [Header("���ѡ��")]
    [Tooltip("��ҿ���ѡ���ѡ���б�")]
    public List<DialogueOption> options;

    [Header("Ĭ����ת�ڵ�")]
    [Tooltip("���û��ѡ��Ի���������ת����һ���ڵ�")]
    public string nextNodeID;

    [Header("������ת")]
    [Tooltip("����������ת����ͬ�Ľڵ�")]
    public List<DialogueCondition> conditions;

    [Header("�ڵ㿪ʼ�¼�")]
    [Tooltip("�ڽڵ㿪ʼʱ�������¼�")]
    public DialogueEvent onNodeStart;

    [Header("�ڵ�����¼�")]
    [Tooltip("�ڽڵ����ʱ�������¼�")]
    public DialogueEvent onNodeEnd;
}

[System.Serializable]
public class DialogueOption
{
    [Header("ѡ���ı�")]
    [Tooltip("��ҿ�����ѡ������")]
    public string optionText;

    [Header("��ת�ڵ�")]
    [Tooltip("ѡ���ѡ�����ת�Ľڵ�")]
    public string nextNodeID;
}

[System.Serializable]
public class DialogueCondition
{
    [Header("����ID")]
    [Tooltip("�����ж�������Ψһ��ʶ��")]
    public string conditionID;

    [Header("��ת�ڵ�")]
    [Tooltip("������������ת�Ľڵ�")]
    public string nextNodeID;
}

[System.Serializable]
public class DialogueEvent
{
    [Header("�¼�����")]
    [Tooltip("�������¼�����")]
    public string eventName;

    [Header("�¼�����")]
    [Tooltip("�¼��ĸ��Ӳ���")]
    public string eventData;
}