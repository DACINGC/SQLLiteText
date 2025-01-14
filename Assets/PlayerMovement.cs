using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 玩家移动速度
    public float rotationSpeed = 700f; // 玩家旋转速度

    private Rigidbody rb; // 玩家刚体

    private void Start()
    {
        // 获取玩家的 Rigidbody 组件
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 获取玩家的输入
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Arrow Up/Down
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Arrow Left/Right

        // 计算移动方向
        Vector3 moveDirection = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        // 移动玩家
        if (moveDirection.magnitude >= 0.1f)
        {
            // 控制玩家朝向移动的方向
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // 移动玩家
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}
