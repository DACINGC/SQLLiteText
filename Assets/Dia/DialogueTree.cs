using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueTree", menuName = "对话系统/对话树", order = 2)]
public class DialogueTree : ScriptableObject
{
    public List<DialogueNode> nodes = new List<DialogueNode>();
    public string startNodeID; // 对话树的起始节点
}