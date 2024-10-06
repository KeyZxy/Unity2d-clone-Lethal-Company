using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 10f;         // �ӵ��ٶ�  
    public float LiveTime = 10f;      // �ӵ�����ʱ��  
    public float force = 500f;         // �ӵ�ʩ�ӵ����������ڻ��˵�Ч����  
    public int damage = 1;             // �ӵ���ɵ��˺�  

    private Vector2 _Direction;        // �ӵ�����  
    private float _HasLiveTime = 0f;   // �ӵ����ʱ�����  
    private Rigidbody2D _Rigidbody;     // �ӵ��� Rigidbody2D ���  
    private int owner;                  // �ӵ���ӵ���ߣ�������һ���ˣ�  

    void Start()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Rigidbody.velocity = _Direction * Speed;
    }

    void Update()
    {
        _HasLiveTime += Time.deltaTime;
        if (_HasLiveTime > LiveTime)
        {
            Destroy(gameObject); // ��������ʱ��������ӵ�  
        }
    }

    public void BasicSet(Vector2 dir, int ownerGet)
    {
        owner = ownerGet;
        _Direction = dir.normalized; // ȷ�������ǵ�λ����  
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Romm" || tag == "OutSide" || tag == "Start"||tag=="Wall"||tag=="Door"||tag=="Normal"||tag=="Silver"||tag=="Gold")
        {
            Destroy(gameObject); // �����ض���ǩ����ʱ�����ӵ�  
        }
        else if (tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // ���õ��˵��˺�������  
            }
            Destroy(gameObject); // �������˺������ӵ�  
        }
        else
        {
            
            //Destroy(gameObject); 
        }
    }
}