using System.Collections.Generic;
using UnityEngine;

public class AoiCullingWithQuadTree : MonoBehaviour
{
    public Camera playerCamera;
    public Transform player;
    public float queryRange = 50f; // 查询范围

    private QuadTreeManager quadTreeManager;

    private void Start()
    {
        quadTreeManager = FindObjectOfType<QuadTreeManager>();
    }

    private void Update()
    {
        // 查询玩家周围的渲染组件
        Vector2 playerPos = new Vector2(player.position.x, player.position.z);
        List<Renderer> nearbyRenderers = quadTreeManager.GetRenderersInRange(playerPos, queryRange);

        // 更新渲染器的可见性
        foreach (var renderer in nearbyRenderers)
        {
            renderer.enabled = true; // 启用玩家范围内的渲染器
        }
    }
}
