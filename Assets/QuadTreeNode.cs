using System.Collections.Generic;
using UnityEngine;

public class QuadTreeNode
{
    public Rect bounds; // �ڵ�߽�
    public List<Renderer> objects; // ��ǰ�ڵ�洢������
    public QuadTreeNode[] children; // �ĸ��ӽڵ�

    private const int MAX_OBJECTS = 4; // ���ڵ����洢����������
    private const int MAX_LEVELS = 5; // ���ݹ����

    private int level; // ��ǰ�ڵ�����

    public QuadTreeNode(int level, Rect bounds)
    {
        this.level = level;
        this.bounds = bounds;
        this.objects = new List<Renderer>();
        this.children = null; // ��ʼʱû���ӽڵ�
    }

    // ��ֵ�ǰ�ڵ�Ϊ�ĸ��ӽڵ�
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

    // ����һ����Ⱦ�����Ĳ���
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


    // ��ȡ��������ص��ӽڵ�����
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


    // ��ѯ�뷶Χ�ཻ��������Ⱦ��
    public void QueryRange(Rect range, List<Renderer> result)
    {
        if (!bounds.Overlaps(range))
        {
            return;
        }

        foreach (var obj in objects)
        {
            // �� Bounds ת��Ϊ Rect
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
        // ���Ƶ�ǰ�ڵ�ı߽�
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(bounds.center.x, 0, bounds.center.y), new Vector3(bounds.width, 0, bounds.height));

        // ������ӽڵ㣬�ݹ����
        if (children != null)
        {
            foreach (var child in children)
            {
                child.DrawDebug();
            }
        }
    }


}
