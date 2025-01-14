using System.Collections.Generic;
using UnityEngine;

public class AoiCullingWithQuadTree : MonoBehaviour
{
    public Camera playerCamera;
    public Transform player;
    public float queryRange = 50f; // ��ѯ��Χ

    private QuadTreeManager quadTreeManager;

    private void Start()
    {
        quadTreeManager = FindObjectOfType<QuadTreeManager>();
    }

    private void Update()
    {
        // ��ѯ�����Χ����Ⱦ���
        Vector2 playerPos = new Vector2(player.position.x, player.position.z);
        List<Renderer> nearbyRenderers = quadTreeManager.GetRenderersInRange(playerPos, queryRange);

        // ������Ⱦ���Ŀɼ���
        foreach (var renderer in nearbyRenderers)
        {
            renderer.enabled = true; // ������ҷ�Χ�ڵ���Ⱦ��
        }
    }
}
