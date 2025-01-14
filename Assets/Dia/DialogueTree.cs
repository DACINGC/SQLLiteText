using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueTree", menuName = "�Ի�ϵͳ/�Ի���", order = 2)]
public class DialogueTree : ScriptableObject
{
    public List<DialogueNode> nodes = new List<DialogueNode>();
    public string startNodeID; // �Ի�������ʼ�ڵ�
}