using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f; // 敌人的移动速度
    public int EnemyHp = 10;
    public float changeTime = 3.0f; // 改变方向的时间间隔
    private float timer;

    public GameObject player; // 存放主角对象
    public float patrolRadius = 5f; // 巡逻路径半径
    public float visionRadius = 10f; // 敌人的视野半径
    public Transform[] patrolPoints; // 巡逻路径上的各个路点

    private int currentPatrolIndex = 0; // 当前巡逻点的索引
    private bool chasingPlayer = false; // 是否在追击玩家
    private bool hasAttacked = false; // 是否已经攻击过
    public Vector2 origin; // 初始位置
    private bool returningToOrigin = false; // 是否正在返回初始位置
    public BoxCollider2D roomCollider; // 当前房间的碰撞体

    void Start()
    {
        timer = changeTime;
        origin = transform.position; // 记录敌人的初始位置
        //roomCollider = GetComponentInParent<BoxCollider2D>(); // 获取当前房间的碰撞体

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true; // 设置为运动学  
        }
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // 确保寻找带有"Player"标签的对象  
        }
        if (!chasingPlayer && !returningToOrigin) // 如果没在追击玩家，也没在返回初始位置，执行巡逻逻辑
        {
            Patrol();
        }

        if (!hasAttacked) // 如果还没有攻击过，检查视野范围
        {
            CheckVision();
        }

        if (returningToOrigin) // 如果正在返回原点
        {
            ReturnToOrigin();
        }
    }
    public void TakeDamage(int damage)
    {
        Debug.Log("敌人的生命值减少了 1 点");
        EnemyHp -= damage; // 减少生命值  
        if (EnemyHp <= 0)
        {
            
            Die(); // 调用死亡方法  
        }
    }

    private void Die()
    {
        Debug.Log("敌人已死亡");
        // 处理敌人死亡的逻辑，例如播放动画、掉落物品等  
        Destroy(gameObject); // 销毁敌人对象  
    }
    private void Patrol() // 敌人在路点之间巡逻
    {
        if (patrolPoints.Length == 0) return; // 没有设置路点时，直接返回

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // 限制敌人移动在当前房间内
        if (roomCollider != null && roomCollider.bounds.Contains(newPosition))
        {
            transform.position = newPosition;
        }

        // 当敌人到达当前路点时，转向下一个路点
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // 依次循环路点
        }
    }

    private void CheckVision() // 检查玩家是否进入视野范围
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= visionRadius) // 玩家在视野范围内
        {
            chasingPlayer = true; // 进入追击状态
            ChasePlayer(); // 执行追击逻辑
        }
        else
        {
            chasingPlayer = false; // 玩家离开视野范围时，停止追击
        }
    }

    private void ChasePlayer() // 追击玩家
    {
        if (chasingPlayer && !hasAttacked)
        {
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.transform.position, speed * 2 * Time.deltaTime);

            // 限制敌人移动在当前房间内
            if (roomCollider != null && roomCollider.bounds.Contains(targetPosition))
            {
                transform.position = targetPosition;
            }

            // 如果敌人接近玩家，尝试触发攻击
            if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)
            {
                // 在 OnCollisionEnter2D 中处理实际的碰撞检测和伤害
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) // 检测与玩家的碰撞
    {
        Player playerScript = other.gameObject.GetComponent<Player>(); // 获取玩家脚本
        if (playerScript != null && !hasAttacked) // 如果玩家存在且没有攻击过
        {
            playerScript.ChangeHealth(-1); // 减少玩家生命值
            Debug.Log("玩家的生命值减少了 1 点");

            // 攻击后停止追击并返回原点
            hasAttacked = true;
            chasingPlayer = false;
            returningToOrigin = true; // 标记为返回原点状态
        }
    }

    private void ReturnToOrigin() // 返回初始位置
    {
        transform.position = Vector2.MoveTowards(transform.position, origin, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, origin) < 0.1f) // 如果到达了原点
        {
            returningToOrigin = false; // 停止返回
            //hasAttacked = false; // 重置攻击状态，允许再次攻击
        }
    }
}
