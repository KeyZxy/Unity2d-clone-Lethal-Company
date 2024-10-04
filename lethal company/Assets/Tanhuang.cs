using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanhuang : Enemy
{
    public bool playerInside = false; // �ж�����Ƿ��ڷ�����  
    private Player playerScript; // �洢��ҵ� Player �ű�����  
    public bool canAttack = true; // ���Ƶ��ɹ��Ƿ���Թ���
    //private bool isInSight = false; // �жϵ��ɹ��Ƿ�����ҵ���Ұ��

    protected void Start()
    {
        player = GameObject.FindWithTag("Player"); // �ҵ����  
        if (player != null)
        {
            playerScript = player.GetComponent<Player>(); // ��ȡ��ҵ� Player ���  
        }
        base.Start(); // ���û���� Start ����  
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // �ҵ����  
        }
        if (player != null)
        {
            playerScript = player.GetComponent<Player>(); // ��ȡ��ҵ� Player ���  
        }

        // �������Ƿ��ڷ�����  
        CheckPlayerInside();

        float distance = Vector2.Distance(transform.position, player.transform.position);

        // ��鵯�ɹ��Ƿ��������Ұ��  
       // CheckIfInSight();

        // �������ڷ����ڣ����Ҳ��������Ұ�ڣ��Ҿ����������������ɹֿ��Խ���׷��
        if (playerInside  && distance >= playerScript.vision && canAttack )
        {
            ChasePlayer();
        }
    }

    // ��鵯�ɹ��Ƿ�����ҵ���Ұ��Χ��
    //private void CheckIfInSight()
    //{
    //    Vector2 direction = player.transform.position - transform.position;
    //    float angle = Vector2.Angle(playerScript.transform.up, direction);

    //    // �жϵ��ɹ��Ƿ�����ҵ���Ұ�Ƕȣ�45�ȣ���
    //    isInSight = (angle <= 45f);
    //}

    protected override void Patrol() // ��д��Ѳ���߼�����ʵ��  
    {
        // ���ɹֲ�����Ѳ��  
    }

    protected override void ReturnToOrigin() // ��д�ķ���ԭ���߼�����ʵ��  
    {
        // ���ɹֲ�����ԭ��  
    }

    protected override void CheckVision()
    {
        // ��д����ʵ��
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

        // ���Ƶ��ɹ��ƶ��ڵ�ǰ������  
        if (roomCollider != null && roomCollider.bounds.Contains(targetPosition))
        {
            transform.position = targetPosition;
        }

        // ������ɹֽӽ���ң����й���  
        if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)
        {
           // AttackPlayer();
        }
    }

}
