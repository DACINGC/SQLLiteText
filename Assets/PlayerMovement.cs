using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // ����ƶ��ٶ�
    public float rotationSpeed = 700f; // �����ת�ٶ�

    private Rigidbody rb; // ��Ҹ���

    private void Start()
    {
        // ��ȡ��ҵ� Rigidbody ���
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // ��ȡ��ҵ�����
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Arrow Up/Down
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Arrow Left/Right

        // �����ƶ�����
        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        // �ƶ����
        if (moveDirection.magnitude >= 0.1f)
        {
            // ������ҳ����ƶ��ķ���
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // �ƶ����
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}
