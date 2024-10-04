using UnityEngine;  

public class Slime : Enemy  
{  
    public bool playerInside = false; // ���������������ж�����Ƿ��ڷ�����  
    private Player playerScript; // �洢��ҵ� Player �ű����� 
    protected void Start()  
    {
        player = GameObject.FindWithTag("Player"); // �ҵ����  
        if (player != null)
        {
            playerScript = player.GetComponent<Player>(); // ��ȡ��ҵ� Player ���  
        }
        base.Start(); // ȷ���ӻ������ Start ����  
    }  

    void Update()  
    {
        player = GameObject.FindWithTag("Player"); // �ҵ����  
        if (player != null)
        {
            playerScript = player.GetComponent<Player>(); // ��ȡ��ҵ� Player ���  
        }
        if (player == null)  
        {  
            player = GameObject.FindWithTag("Player"); // �ҵ����  
        }  

        // �������Ƿ��ڷ�����  
        CheckPlayerInside();  

        // �������ڷ����ڣ�����׷��  
        if (playerInside)  
        {  
            ChasePlayer();  
        }  
    }  

    protected override void Patrol() // ��д��Ѳ���߼�����ʵ��  
    {  
        // Slime ������Ѳ��  
    }  

    protected override void ReturnToOrigin() // ��д�ķ���ԭ���߼�����ʵ��  
    {  
        // Slime ������ԭ��  
    }  
    protected override void CheckVision()
    {

    }
    private void CheckPlayerInside() // �������Ƿ��ڷ�����  
    {  
        if (roomCollider != null && player != null)  
        {  
            playerInside = roomCollider.bounds.Contains(player.transform.position);  
        }  
        else  
        {  
            playerInside = false; // ���û�з�����ײ�����ң�����Ϊ false  
        }  
    }  

    protected override void ChasePlayer() // ׷������߼�  
    {  
        
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.transform.position, speed * 2 * Time.deltaTime);  

            // ���� Slime �ƶ��ڵ�ǰ������  
            if (roomCollider != null && roomCollider.bounds.Contains(targetPosition))  
            {  
                transform.position = targetPosition;  
            }  

            // ��� Slime �ӽ���ң����Թ���  
            if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)  
            {
            //AttackPlayer();
        }  
        
    }  
    
}