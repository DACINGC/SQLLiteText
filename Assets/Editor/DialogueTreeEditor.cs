using UnityEditor;
using UnityEngine;

public class DialogueTreeEditor : EditorWindow
{
    private DialogueTree dialogueTree;
    private Vector2 scrollPosition;

    [MenuItem("对话系统/对话树编辑器")]
    public static void ShowWindow()
    {
        GetWindow<DialogueTreeEditor>("对话树编辑器");
    }

    private void OnGUI()
    {
        if (dialogueTree == null)
        {
            GUILayout.Label("请加载一个对话树");
            if (GUILayout.Button("加载对话树"))
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
        GUILayout.Label($"节点ID: {node.nodeID}");
        node.speakerName = EditorGUILayout.TextField("说话者名字", node.speakerName);
        node.dialogueText = EditorGUILayout.TextArea(node.dialogueText, GUILayout.Height(100));
        GUILayout.EndVertical();
    }

    private void LoadDialogueTree()
    {
        string path = EditorUtility.OpenFilePanel("加载对话树", "Assets", "asset");
        if (!string.IsNullOrEmpty(path))
        {
            path = "Assets" + path.Substring(Application.dataPath.Length);
            dialogueTree = AssetDatabase.LoadAssetAtPath<DialogueTree>(path);
        }
    }
}