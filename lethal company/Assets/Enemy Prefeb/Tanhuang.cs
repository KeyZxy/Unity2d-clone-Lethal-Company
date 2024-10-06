using UnityEngine;

public class Tanhuang : Enemy
{
    public bool playerInside = false; // 判断玩家是否在房间内  
    private Player playerScript; // 存储玩家的 Player 脚本引用  
    public bool canAttack = true; // 控制弹簧怪是否可以攻击
    private bool isInSight = false; // 是否在玩家视野内

    protected void Start()
    {
        player = GameObject.FindWithTag("Player"); // 找到玩家  
        if (player != null)
        {
            playerScript = player.GetComponent<Player>(); // 获取玩家的 Player 组件  
        }
        base.Start(); // 调用基类的 Start 方法  
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // 找到玩家  
        }
        if (player != null)
        {
            playerScript = player.GetComponent<Player>(); // 获取玩家的 Player 组件  
        }

        // 检查玩家是否在房间内  
        CheckPlayerInside();

        float distance = Vector2.Distance(transform.position, player.transform.position);

        // 如果玩家在房间内，并且不在玩家视野内，且距离满足条件，弹簧怪可以进行追击
        if (playerInside && distance >= playerScript.vision && canAttack && !isInSight)
        {
            ChasePlayer();
        }

        // 重置 isInSight 为 false，每帧都需要重新检测
        isInSight = false;
    }

    public void SetInPlayerSight(bool inSight)
    {
        isInSight = inSight; // 设置是否在玩家视野内
    }

    protected override void Patrol() // 重写的巡逻逻辑，空实现  
    {
        // 弹簧怪不进行巡逻  
    }

    protected override void ReturnToOrigin() // 重写的返回原点逻辑，空实现  
    {
        // 弹簧怪不返回原点  
    }

    protected override void CheckVision()
    {
        // 重写但不实现
    }

    private void CheckPlayerInside() // 检查玩家是否在房间内  
    {
        if (roomCollider != null && player != null)
        {
            playerInside = roomCollider.bounds.Contains(player.transform.position);
        }
        else
        {
            playerInside = false; // 如果没有房间碰撞体或玩家，设置为 false  
        }
    }

    protected override void ChasePlayer() // 追击玩家逻辑  
    {
        Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.transform.position, speed * 2 * Time.deltaTime);

        // 限制弹簧怪移动在当前房间内  
        if (roomCollider != null && roomCollider.bounds.Contains(targetPosition))
        {
            transform.position = targetPosition;
        }
    }
}
