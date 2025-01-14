using System.Collections.Generic;
using UnityEngine;

public class QuadTreeManager : MonoBehaviour
{
    public Rect worldBounds; // ���緶Χ
    private QuadTreeNode quadTreeRoot;

    private void Start()
    {
        CalculateWorldBounds();
        quadTreeRoot = new QuadTreeNode(0, worldBounds);

        // �������嵽�Ĳ���
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (var renderer in renderers)
        {
            quadTreeRoot.Insert(renderer);
        }
    }

    void CalculateWorldBounds()
    {
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        if (renderers.Length == 0) return;

        // ��ʼ��Ϊ��һ������ı߽�
        Bounds worldBounds3D = renderers[0].bounds;

        // ��չ��Χ�԰�����������
        foreach (var renderer in renderers)
        {
            worldBounds3D.Encapsulate(renderer.bounds);
        }

        // ת�� 3D Bounds �� 2D Rect
        worldBounds = new Rect(
            worldBounds3D.min.x, // ���½� X
            worldBounds3D.min.z, // ���½� Z
            worldBounds3D.size.x, // ���
            worldBounds3D.size.z  // �߶�
        );
    }

    public List<Renderer> GetRenderersInRange(Vector2 position, float range)
    {
        List<Renderer> result = new List<Renderer>();
        Rect queryRect = new Rect(position.x - range, position.y - range, range * 2, range * 2);
        quadTreeRoot.QueryRange(queryRect, result);
        return result;
    }

    private void OnDrawGizmos()
    {
        if (worldBounds != Rect.zero)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(
                new Vector3(worldBounds.center.x, 0, worldBounds.center.y),
                new Vector3(worldBounds.width, 0, worldBounds.height)
            );
        }
    }

}
