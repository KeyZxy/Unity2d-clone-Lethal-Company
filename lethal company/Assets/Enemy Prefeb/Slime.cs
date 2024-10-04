using UnityEngine;  

public class Slime : Enemy  
{  
    public bool playerInside = false; // 新增变量，用于判断玩家是否在房间内  
    private Player playerScript; // 存储玩家的 Player 脚本引用 
    protected void Start()  
    {
        player = GameObject.FindWithTag("Player"); // 找到玩家  
        if (player != null)
        {
            playerScript = player.GetComponent<Player>(); // 获取玩家的 Player 组件  
        }
        base.Start(); // 确保从基类调用 Start 方法  
    }  

    void Update()  
    {
        player = GameObject.FindWithTag("Player"); // 找到玩家  
        if (player != null)
        {
            playerScript = player.GetComponent<Player>(); // 获取玩家的 Player 组件  
        }
        if (player == null)  
        {  
            player = GameObject.FindWithTag("Player"); // 找到玩家  
        }  

        // 检查玩家是否在房间内  
        CheckPlayerInside();  

        // 如果玩家在房间内，进行追击  
        if (playerInside)  
        {  
            ChasePlayer();  
        }  
    }  

    protected override void Patrol() // 重写的巡逻逻辑，空实现  
    {  
        // Slime 不进行巡逻  
    }  

    protected override void ReturnToOrigin() // 重写的返回原点逻辑，空实现  
    {  
        // Slime 不返回原点  
    }  
    protected override void CheckVision()
    {

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

            // 限制 Slime 移动在当前房间内  
            if (roomCollider != null && roomCollider.bounds.Contains(targetPosition))  
            {  
                transform.position = targetPosition;  
            }  

            // 如果 Slime 接近玩家，尝试攻击  
            if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)  
            {
            //AttackPlayer();
        }  
        
    }  
    
}