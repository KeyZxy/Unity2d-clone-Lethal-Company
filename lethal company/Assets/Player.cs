using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI; // 记得引入 UI 命名空间  

public class Player : MonoBehaviour
{
    public float walkSpeed = 3.5f;
    public float runSpeed = 7f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public float vision = 4.0f;//玩家视野

    public float pickupRange = 2f;
    public Transform holdParent;
    public GameObject pickedObject = null;
    public bool isHoldingObject = false;

    public float Hp = 100f;
    public float maxHp = 100f;

    public int Coin;

    public string skillName;

    public GameObject bulletPrefab;
    public GameObject Gun;
    public Transform bulletSpawnPoint;
    public int remainingShots = 0;

    private Camera mainCamera;

    public InputField inputField; // 添加一个输入框引用  
    private bool isInputFieldActive = false; // 输入框是否激活  

    public RawImage rawImage; // 添加一个小地图引用  
    private bool isRawImageActive = false; // 小地图是否激活  

    public bool canLookAround = true; // 控制是否可以用鼠标朝向  

    //public MiniMap miniMap;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        // 确保一开始输入框是关闭的  
        inputField.gameObject.SetActive(false);

        Gun.gameObject.SetActive(false);
    }

    void Update()
    {
        // 获取输入  
        moveInput.x = Input.GetAxis("Horizontalplayer");
        moveInput.y = Input.GetAxis("Verticalplayer");
        if (moveInput != Vector2.zero)
        {
            //miniMap.UpdateMiniMap(moveInput);
        }

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
        if (remainingShots > 0 && skillName == "Gun")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootBullet();
            }
        }

        // 检查切换到输入框的状态  
        if (Input.GetKeyDown(KeyCode.F))
        {
            //canLookAround = false;
            ToggleInputField();
            ToggleRawImage();

        }
        if (canLookAround)
        {
            MouseLook();
        }

        // 检测敌人是否在视野范围  
        CheckEnemyInSight();
        // 如果输入框处于不活动状态，则继续处理鼠标控制  
        //if (!isInputFieldActive)
        //{
        //    MouseLook();
        //}
    }
    //检查敌人是否在视野范围内
    private void CheckEnemyInSight()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, vision);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy")) // 确保弹簧怪有“Enemy”标签  
            {
                Vector2 direction = collider.transform.position - transform.position;
                if (Vector2.Angle(direction, transform.up) <= 90f) // 假设玩家面前的视野为45度  
                {
                    // 在视野范围内实现其他逻辑，比如通知弹簧怪不能攻击  
                    Tanhuang tanhuang = collider.GetComponent<Tanhuang>();
                    if (tanhuang != null)
                    {
                        tanhuang.canAttack = false; // 设置弹簧怪不能攻击  
                    }
                }
            }
        }
    }

    public void SetMouseLookEnabled(bool enabled)
    {
        canLookAround = enabled; // 设置鼠标查看能力  
    }
    void ToggleInputField()
    {
        canLookAround=isInputFieldActive;
        isInputFieldActive = !isInputFieldActive; // 切换状态  
        inputField.gameObject.SetActive(isInputFieldActive); // 激活或禁用输入框  

        // 如果输入框被激活，选中输入框以便让用户直接开始输入  
        if (isInputFieldActive)
        {
            inputField.Select();
            inputField.ActivateInputField();
        }
        
    }
    void ToggleRawImage()
    {
        canLookAround = isRawImageActive;
        isRawImageActive = !isRawImageActive;
        rawImage.gameObject.SetActive(isRawImageActive);
    }

    void ShootBullet()
    {
        // 发射子弹  
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - bulletSpawnPoint.position).normalized;
        bulletScript.BasicSet(direction, 1);

        remainingShots--;
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

        if (direction.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
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
    }

    public void ChangeCoin(int coin)
    {
        Coin += coin;
    }
}