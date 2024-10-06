using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 10f;         // 子弹速度  
    public float LiveTime = 10f;      // 子弹生存时间  
    public float force = 500f;         // 子弹施加的力（可用于击退等效果）  
    public int damage = 1;             // 子弹造成的伤害  

    private Vector2 _Direction;        // 子弹方向  
    private float _HasLiveTime = 0f;   // 子弹存活时间计数  
    private Rigidbody2D _Rigidbody;     // 子弹的 Rigidbody2D 组件  
    private int owner;                  // 子弹的拥有者（例如玩家或敌人）  

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
            Destroy(gameObject); // 超过生存时间后销毁子弹  
        }
    }

    public void BasicSet(Vector2 dir, int ownerGet)
    {
        owner = ownerGet;
        _Direction = dir.normalized; // 确保方向是单位向量  
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
            Destroy(gameObject); // 碰到特定标签物体时销毁子弹  
        }
        else if (tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); // 调用敌人的伤害处理方法  
            }
            Destroy(gameObject); // 碰到敌人后销毁子弹  
        }
        else
        {
            
            //Destroy(gameObject); 
        }
    }
}