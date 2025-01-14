using System.Collections.Generic;
using UnityEngine;

public class QuadTreeManager : MonoBehaviour
{
    public Rect worldBounds; // 世界范围
    private QuadTreeNode quadTreeRoot;

    private void Start()
    {
        CalculateWorldBounds();
        quadTreeRoot = new QuadTreeNode(0, worldBounds);

        // 插入物体到四叉树
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

        // 初始化为第一个物体的边界
        Bounds worldBounds3D = renderers[0].bounds;

        // 扩展范围以包含所有物体
        foreach (var renderer in renderers)
        {
            worldBounds3D.Encapsulate(renderer.bounds);
        }

        // 转换 3D Bounds 到 2D Rect
        worldBounds = new Rect(
            worldBounds3D.min.x, // 左下角 X
            worldBounds3D.min.z, // 左下角 Z
            worldBounds3D.size.x, // 宽度
            worldBounds3D.size.z  // 高度
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
