using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject player; // 玩家对象
    public float attackRange = 1.0f; // 攻击范围
    public float moveSpeed = 3.0f;   // 移动速度

    // Update is called once per frame
    void Update()
    {
        // 计算敌人和玩家之间的距离
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // 如果距离大于攻击范围，则敌人向玩家移动
        if (distanceToPlayer > attackRange)
        {
            // 获取朝向玩家的方向向量
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // 计算移动位置
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

            // 更新敌人朝向，确保始终面向玩家
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
