using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "对话系统/对话节点", order = 1)]
public class DialogueNode : ScriptableObject
{
    [Header("节点ID")]
    [Tooltip("节点的唯一标识符")]
    public string nodeID;

    [Header("说话者名字")]
    [Tooltip("当前对话的发言者名字")]
    public string speakerName;

    [Header("对话内容")]
    [Tooltip("当前节点显示的对话文本")]
    [TextArea(3, 10)] public string dialogueText;

    [Header("玩家选项")]
    [Tooltip("玩家可以选择的选项列表")]
    public List<DialogueOption> options;

    [Header("默认跳转节点")]
    [Tooltip("如果没有选项，对话结束后跳转的下一个节点")]
    public string nextNodeID;

    [Header("条件跳转")]
    [Tooltip("根据条件跳转到不同的节点")]
    public List<DialogueCondition> conditions;

    [Header("节点开始事件")]
    [Tooltip("在节点开始时触发的事件")]
    public DialogueEvent onNodeStart;

    [Header("节点结束事件")]
    [Tooltip("在节点结束时触发的事件")]
    public DialogueEvent onNodeEnd;
}

[System.Serializable]
public class DialogueOption
{
    [Header("选项文本")]
    [Tooltip("玩家看到的选项内容")]
    public string optionText;

    [Header("跳转节点")]
    [Tooltip("选择该选项后跳转的节点")]
    public string nextNodeID;
}

[System.Serializable]
public class DialogueCondition
{
    [Header("条件ID")]
    [Tooltip("用于判断条件的唯一标识符")]
    public string conditionID;

    [Header("跳转节点")]
    [Tooltip("满足条件后跳转的节点")]
    public string nextNodeID;
}

[System.Serializable]
public class DialogueEvent
{
    [Header("事件名称")]
    [Tooltip("触发的事件名称")]
    public string eventName;

    [Header("事件参数")]
    [Tooltip("事件的附加参数")]
    public string eventData;
}