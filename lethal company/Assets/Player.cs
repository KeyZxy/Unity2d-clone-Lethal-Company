using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed = 3.5f; // ������·�ٶ�  
    public float runSpeed = 7f; // ���ñ����ٶ�   
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public float pickupRange = 2f; // ʰȡ��Χ  
    public Transform holdParent; // ��ű�ʰȡ����ĸ�����  
    public GameObject pickedObject = null; // ��ǰʰȡ������  
    public bool isHoldingObject = false;

    public float Hp = 100f;//Ѫ��  
    public float maxHp = 100f;

    public int Coin;//���  

   //��õĵ�������  
    public string skillName;

    // �����ӵ�����  
    public GameObject bulletPrefab; // �ӵ�Ԥ�Ƽ�  
    public Transform bulletSpawnPoint; // �ӵ������  
    public int remainingShots = 0; // ʣ�෢�����  

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
            rb.velocity = moveInput * runSpeed;
        }
        else
        {
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

        // ����������  
        if (remainingShots > 0&&skillName=="Gun")
        {
            
            if(Input.GetKeyDown(KeyCode.Space)) 
            {
                ShootBullet();
            }
        }

        // �����Ƴ���  
        MouseLook();
        
    }

   

    void ShootBullet()
    {
        // �����ӵ�  
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - bulletSpawnPoint.position).normalized;
        bulletScript.BasicSet(direction, 1); // �����ӵ�������ӵ����  

        remainingShots--; // ���ٷ������  
    }

    void TryPickupObject()
    {
        // ����ʰȡ������  
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Gold") || collider.CompareTag("Silver") || collider.CompareTag("Normal") || collider.CompareTag("Pickup"))
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
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - rb.position;

        // ������귽����Ч��   
        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle; // ֱ�����ýǶ�  
        }
    }

    public void ChangeHealth(float health)
    {
        Hp += health;
        if (Hp < 0)
        {
            Hp = 0;
            HandleDeath();
        }

        if (Hp > maxHp)
        {
            Hp = maxHp;
        }
    }
    private void HandleDeath()
    {
        Debug.Log("Player has died.");
        // ���磺������ҿ��ơ���������������  
    }

    public void ChangeCoin(int coin)
    {
        Coin += coin;
    }
}