using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Player : MonoBehaviour
{
    public float walkSpeed = 3.5f; // 设置走路速度  
    public float runSpeed = 7f; // 设置奔跑速度 
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public float pickupRange = 1f; // 拾取范围  
    public Transform holdParent; // 存放被拾取物体的父物体  
    public GameObject pickedObject = null; // 当前拾取的物体  
    private bool isHoldingObject = false;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main; // 获取主摄像机
    }

    // Update is called once per frame
    void Update()
    {
        // 获取输入  
        moveInput.x = Input.GetAxis("Horizontalplayer");
        moveInput.y = Input.GetAxis("Verticalplayer");

        // 调整移动速度  
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // 如果按下 Shift 键，则设置为奔跑速度  
            rb.velocity = moveInput * runSpeed;
        }
        else
        {
            // 否则使用走路速度  
            rb.velocity = moveInput * walkSpeed;
        }

        // 检查拾取物体  
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isHoldingObject)
            {
                DropObject();
            }
            else
            {
                TryPickupObject();
            }
        }
        // 鼠标控制朝向  
        MouseLook();
    }
    void TryPickupObject()
    {
        // Check for objects in the pickup range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Pickup")) // Ensure object is pickable by checking its tag
            {
                pickedObject = collider.gameObject;
                pickedObject.transform.SetParent(holdParent);
                pickedObject.transform.localPosition = Vector3.zero;
                isHoldingObject = true;
                break;
            }
        }
    }

    void DropObject()
    {
        if (pickedObject != null)
        {
            pickedObject.transform.SetParent(null);
            pickedObject = null;
            isHoldingObject = false;
        }
    }
    void MouseLook()
    {
        // 获取鼠标位置相对于世界坐标  
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 计算指向  
        Vector2 direction = mousePosition - rb.position;
        // 如果鼠标方向有效，计算角度  
        if (direction.sqrMagnitude > 0.01f) // 用平方的大小来避免太小的鼠标偏移  
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // 使用 Quaternion.RotateTowards 将玩家旋转到目标角度  
            rb.rotation = angle; // 直接设置角度  
        }
    }
}
