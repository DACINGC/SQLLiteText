using UnityEngine;

public class AoiCulling : MonoBehaviour
{
    public Camera playerCamera; // 玩家摄像机
    private CullingGroup cullingGroup; // Culling Group 实例
    private BoundingSphere[] boundingSpheres; // 存储每个物体的包围球
    private Renderer[] renderers; // 存储场景中的所有渲染器

    private void Start()
    {
        // 初始化 CullingGroup
        cullingGroup = new CullingGroup();
        cullingGroup.targetCamera = playerCamera; // 设置目标摄像机

        // 获取场景中所有渲染器，过滤掉 Tag 为 "player" 的物体
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();
        renderers = System.Array.FindAll(allRenderers, renderer => renderer.gameObject.tag != "player");
        boundingSpheres = new BoundingSphere[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            // 创建包围球，设置物体的包围盒位置和半径
            boundingSpheres[i] = new BoundingSphere(renderers[i].bounds.center, renderers[i].bounds.extents.magnitude);
        }

        // 设置 CullingGroup 的包围球
        cullingGroup.SetBoundingSpheres(boundingSpheres);
        cullingGroup.SetBoundingSphereCount(renderers.Length);

        // 注册回调
        cullingGroup.onStateChanged = OnCullingStateChanged;

        // 手动调用一次剔除逻辑
        UpdateCullingState();
    }

    private void Update()
    {
        // 在每一帧更新 CullingGroup（自动进行剔除）
        cullingGroup.SetDistanceReferencePoint(playerCamera.transform.position);
    }

    private void UpdateCullingState()
    {
        // 手动更新物体的可见性
        for (int i = 0; i < renderers.Length; i++)
        {
            bool isVisible = cullingGroup.GetDistance(i) < 0; // 判断物体是否在视锥体内
            renderers[i].enabled = isVisible;
        }
    }

    private void OnCullingStateChanged(CullingGroupEvent cullingGroupEvent)
    {
        // 物体是否在视锥体内的状态发生变化时调用
        Renderer renderer = renderers[cullingGroupEvent.index];

        if (cullingGroupEvent.isVisible)
        {
            renderer.enabled = true; // 如果物体在视锥体内，启用渲染
        }
        else
        {
            renderer.enabled = false; // 如果物体不在视锥体内，禁用渲染
        }
    }

    private void OnDestroy()
    {
        // 销毁 CullingGroup
        if (cullingGroup != null)
        {
            cullingGroup.Dispose();
        }
    }
}
