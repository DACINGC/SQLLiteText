using System.Collections.Generic;
using UnityEngine;

public class QuadTreeNode
{
    public Rect bounds; // 节点边界
    public List<Renderer> objects; // 当前节点存储的物体
    public QuadTreeNode[] children; // 四个子节点

    private const int MAX_OBJECTS = 4; // 单节点最多存储的物体数量
    private const int MAX_LEVELS = 5; // 最大递归深度

    private int level; // 当前节点的深度

    public QuadTreeNode(int level, Rect bounds)
    {
        this.level = level;
        this.bounds = bounds;
        this.objects = new List<Renderer>();
        this.children = null; // 初始时没有子节点
    }

    // 拆分当前节点为四个子节点
    public void Subdivide()
    {
        float halfWidth = bounds.width / 2f;
        float halfHeight = bounds.height / 2f;

        children = new QuadTreeNode[4];
        children[0] = new QuadTreeNode(level + 1, new Rect(bounds.x, bounds.y, halfWidth, halfHeight));
        children[1] = new QuadTreeNode(level + 1, new Rect(bounds.x + halfWidth, bounds.y, halfWidth, halfHeight));
        children[2] = new QuadTreeNode(level + 1, new Rect(bounds.x, bounds.y + halfHeight, halfWidth, halfHeight));
        children[3] = new QuadTreeNode(level + 1, new Rect(bounds.x + halfWidth, bounds.y + halfHeight, halfWidth, halfHeight));
    }

    // 插入一个渲染器到四叉树
    public void Insert(Renderer renderer)
    {
        if (children != null)
        {
            int index = GetChildIndex(renderer.bounds);
            if (index != -1)
            {
                children[index].Insert(renderer);
                return;
            }
        }

        objects.Add(renderer);

        if (objects.Count > MAX_OBJECTS && level < MAX_LEVELS)
        {
            if (children == null)
            {
                Subdivide();
            }

            int i = 0;
            while (i < objects.Count)
            {
                int index = GetChildIndex(objects[i].bounds);
                if (index != -1)
                {
                    children[index].Insert(objects[i]);
                    objects.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }


    // 获取与物体相关的子节点索引
    private int GetChildIndex(Bounds bounds)
    {
        int index = -1;

        float verticalMidpoint = this.bounds.x + this.bounds.width / 2f;
        float horizontalMidpoint = this.bounds.y + this.bounds.height / 2f;

        bool topQuadrant = bounds.center.z > horizontalMidpoint;
        bool bottomQuadrant = bounds.center.z < horizontalMidpoint;

        if (bounds.center.x < verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 2;
            }
            else if (bottomQuadrant)
            {
                index = 0;
            }
        }
        else if (bounds.center.x > verticalMidpoint)
        {
            if (topQuadrant)
            {
                index = 3;
            }
            else if (bottomQuadrant)
            {
                index = 1;
            }
        }

        return index;
    }


    // 查询与范围相交的所有渲染器
    public void QueryRange(Rect range, List<Renderer> result)
    {
        if (!bounds.Overlaps(range))
        {
            return;
        }

        foreach (var obj in objects)
        {
            // 将 Bounds 转换为 Rect
            Rect objRect = new Rect(
                obj.bounds.center.x - obj.bounds.extents.x,
                obj.bounds.center.z - obj.bounds.extents.z,
                obj.bounds.size.x,
                obj.bounds.size.z
            );

            if (range.Overlaps(objRect))
            {
                result.Add(obj);
            }
        }

        if (children != null)
        {
            foreach (var child in children)
            {
                child.QueryRange(range, result);
            }
        }
    }

    public void DrawDebug()
    {
        // 绘制当前节点的边界
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(bounds.center.x, 0, bounds.center.y), new Vector3(bounds.width, 0, bounds.height));

        // 如果有子节点，递归绘制
        if (children != null)
        {
            foreach (var child in children)
            {
                child.DrawDebug();
            }
        }
    }


}
