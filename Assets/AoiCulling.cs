using UnityEngine;

public class AoiCulling : MonoBehaviour
{
    public Camera playerCamera; // ��������
    private CullingGroup cullingGroup; // Culling Group ʵ��
    private BoundingSphere[] boundingSpheres; // �洢ÿ������İ�Χ��
    private Renderer[] renderers; // �洢�����е�������Ⱦ��

    private void Start()
    {
        // ��ʼ�� CullingGroup
        cullingGroup = new CullingGroup();
        cullingGroup.targetCamera = playerCamera; // ����Ŀ�������

        // ��ȡ������������Ⱦ�������˵� Tag Ϊ "player" ������
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();
        renderers = System.Array.FindAll(allRenderers, renderer => renderer.gameObject.tag != "player");
        boundingSpheres = new BoundingSphere[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            // ������Χ����������İ�Χ��λ�úͰ뾶
            boundingSpheres[i] = new BoundingSphere(renderers[i].bounds.center, renderers[i].bounds.extents.magnitude);
        }

        // ���� CullingGroup �İ�Χ��
        cullingGroup.SetBoundingSpheres(boundingSpheres);
        cullingGroup.SetBoundingSphereCount(renderers.Length);

        // ע��ص�
        cullingGroup.onStateChanged = OnCullingStateChanged;

        // �ֶ�����һ���޳��߼�
        UpdateCullingState();
    }

    private void Update()
    {
        // ��ÿһ֡���� CullingGroup���Զ������޳���
        cullingGroup.SetDistanceReferencePoint(playerCamera.transform.position);
    }

    private void UpdateCullingState()
    {
        // �ֶ���������Ŀɼ���
        for (int i = 0; i < renderers.Length; i++)
        {
            bool isVisible = cullingGroup.GetDistance(i) < 0; // �ж������Ƿ�����׶����
            renderers[i].enabled = isVisible;
        }
    }

    private void OnCullingStateChanged(CullingGroupEvent cullingGroupEvent)
    {
        // �����Ƿ�����׶���ڵ�״̬�����仯ʱ����
        Renderer renderer = renderers[cullingGroupEvent.index];

        if (cullingGroupEvent.isVisible)
        {
            renderer.enabled = true; // �����������׶���ڣ�������Ⱦ
        }
        else
        {
            renderer.enabled = false; // ������岻����׶���ڣ�������Ⱦ
        }
    }

    private void OnDestroy()
    {
        // ���� CullingGroup
        if (cullingGroup != null)
        {
            cullingGroup.Dispose();
        }
    }
}
