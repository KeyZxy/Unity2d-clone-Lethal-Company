using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float walkSpeed = 3.5f; // 设置走路速度  
    public float runSpeed = 7f; // 设置奔跑速度   
    private Vector2 moveInput;
    private Rigidbody2D rb;

    public float pickupRange = 2f; // 拾取范围  
    public Transform holdParent; // 存放被拾取物体的父物体  
    public GameObject pickedObject = null; // 当前拾取的物体  
    public bool isHoldingObject = false;

    public float Hp = 100f;//血量  
    public float maxHp = 100f;

    public int Coin;//金币  

   //获得的道具能力  
    public string skillName;

    // 用于子弹功能  
    public GameObject bulletPrefab; // 子弹预制件  
    public Transform bulletSpawnPoint; // 子弹发射点  
    public int remainingShots = 0; // 剩余发射次数  

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
            rb.velocity = moveInput * runSpeed;
        }
        else
        {
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

        // 检查射击输入  
        if (remainingShots > 0&&skillName=="Gun")
        {
            
            if(Input.GetKeyDown(KeyCode.Space)) 
            {
                ShootBullet();
            }
        }

        // 鼠标控制朝向  
        MouseLook();
        
    }

   

    void ShootBullet()
    {
        // 发射子弹  
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - bulletSpawnPoint.position).normalized;
        bulletScript.BasicSet(direction, 1); // 设置子弹方向与拥有者  

        remainingShots--; // 减少发射次数  
    }

    void TryPickupObject()
    {
        // 检查可拾取的物体  
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

        // 计算鼠标方向有效性   
        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle; // 直接设置角度  
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
        // 例如：禁用玩家控制、播放死亡动画等  
    }

    public void ChangeCoin(int coin)
    {
        Coin += coin;
    }
}