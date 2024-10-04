using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI; // �ǵ����� UI �����ռ�  

public class Player : MonoBehaviour
{
    public float walkSpeed = 3.5f;
    public float runSpeed = 7f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    public float vision = 4.0f;//�����Ұ

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

    public InputField inputField; // ���һ�����������  
    private bool isInputFieldActive = false; // ������Ƿ񼤻�  

    public RawImage rawImage; // ���һ��С��ͼ����  
    private bool isRawImageActive = false; // С��ͼ�Ƿ񼤻�  

    public bool canLookAround = true; // �����Ƿ��������곯��  

    //public MiniMap miniMap;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        // ȷ��һ��ʼ������ǹرյ�  
        inputField.gameObject.SetActive(false);

        Gun.gameObject.SetActive(false);
    }

    void Update()
    {
        // ��ȡ����  
        moveInput.x = Input.GetAxis("Horizontalplayer");
        moveInput.y = Input.GetAxis("Verticalplayer");
        if (moveInput != Vector2.zero)
        {
            //miniMap.UpdateMiniMap(moveInput);
        }

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
        if (remainingShots > 0 && skillName == "Gun")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ShootBullet();
            }
        }

        // ����л���������״̬  
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

        // �������Ƿ�����Ұ��Χ  
        CheckEnemyInSight();
        // ���������ڲ��״̬�����������������  
        //if (!isInputFieldActive)
        //{
        //    MouseLook();
        //}
    }
    //�������Ƿ�����Ұ��Χ��
    private void CheckEnemyInSight()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, vision);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy")) // ȷ�����ɹ��С�Enemy����ǩ  
            {
                Vector2 direction = collider.transform.position - transform.position;
                if (Vector2.Angle(direction, transform.up) <= 90f) // ���������ǰ����ҰΪ45��  
                {
                    // ����Ұ��Χ��ʵ�������߼�������֪ͨ���ɹֲ��ܹ���  
                    Tanhuang tanhuang = collider.GetComponent<Tanhuang>();
                    if (tanhuang != null)
                    {
                        tanhuang.canAttack = false; // ���õ��ɹֲ��ܹ���  
                    }
                }
            }
        }
    }

    public void SetMouseLookEnabled(bool enabled)
    {
        canLookAround = enabled; // �������鿴����  
    }
    void ToggleInputField()
    {
        canLookAround=isInputFieldActive;
        isInputFieldActive = !isInputFieldActive; // �л�״̬  
        inputField.gameObject.SetActive(isInputFieldActive); // �������������  

        // �������򱻼��ѡ��������Ա����û�ֱ�ӿ�ʼ����  
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
        // �����ӵ�  
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - bulletSpawnPoint.position).normalized;
        bulletScript.BasicSet(direction, 1);

        remainingShots--;
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