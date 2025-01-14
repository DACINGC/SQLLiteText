using UnityEditor;
using UnityEngine;

public class DialogueTreeEditor : EditorWindow
{
    private DialogueTree dialogueTree;
    private Vector2 scrollPosition;

    [MenuItem("�Ի�ϵͳ/�Ի����༭��")]
    public static void ShowWindow()
    {
        GetWindow<DialogueTreeEditor>("�Ի����༭��");
    }

    private void OnGUI()
    {
        if (dialogueTree == null)
        {
            GUILayout.Label("�����һ���Ի���");
            if (GUILayout.Button("���ضԻ���"))
            {
                LoadDialogueTree();
            }
            return;
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        foreach (var node in dialogueTree.nodes)
        {
            DrawNode(node);
        }
        GUILayout.EndScrollView();
    }

    private void DrawNode(DialogueNode node)
    {
        GUILayout.BeginVertical("box");
        GUILayout.Label($"�ڵ�ID: {node.nodeID}");
        node.speakerName = EditorGUILayout.TextField("˵��������", node.speakerName);
        node.dialogueText = EditorGUILayout.TextArea(node.dialogueText, GUILayout.Height(100));
        GUILayout.EndVertical();
    }

    private void LoadDialogueTree()
    {
        string path = EditorUtility.OpenFilePanel("���ضԻ���", "Assets", "asset");
        if (!string.IsNullOrEmpty(path))
        {
            path = "Assets" + path.Substring(Application.dataPath.Length);
            dialogueTree = AssetDatabase.LoadAssetAtPath<DialogueTree>(path);
        }
    }
}