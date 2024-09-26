using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Player : MonoBehaviour
{
    public float walkSpeed = 3.5f; // ������·�ٶ�  
    public float runSpeed = 7f; // ���ñ����ٶ� 
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public float pickupRange = 1f; // ʰȡ��Χ  
    public Transform holdParent; // ��ű�ʰȡ����ĸ�����  
    public GameObject pickedObject = null; // ��ǰʰȡ������  
    private bool isHoldingObject = false;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main; // ��ȡ�������
    }

    // Update is called once per frame
    void Update()
    {
        // ��ȡ����  
        moveInput.x = Input.GetAxis("Horizontalplayer");
        moveInput.y = Input.GetAxis("Verticalplayer");

        // �����ƶ��ٶ�  
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            // ������� Shift ����������Ϊ�����ٶ�  
            rb.velocity = moveInput * runSpeed;
        }
        else
        {
            // ����ʹ����·�ٶ�  
            rb.velocity = moveInput * walkSpeed;
        }

        // ���ʰȡ����  
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
        // �����Ƴ���  
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
        // ��ȡ���λ���������������  
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // ����ָ��  
        Vector2 direction = mousePosition - rb.position;
        // �����귽����Ч������Ƕ�  
        if (direction.sqrMagnitude > 0.01f) // ��ƽ���Ĵ�С������̫С�����ƫ��  
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // ʹ�� Quaternion.RotateTowards �������ת��Ŀ��Ƕ�  
            rb.rotation = angle; // ֱ�����ýǶ�  
        }
    }
}
