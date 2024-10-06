using UnityEngine;

public class Tanhuang : Enemy
{
    public bool playerInside = false; // �ж�����Ƿ��ڷ�����  
    private Player playerScript; // �洢��ҵ� Player �ű�����  
    public bool canAttack = true; // ���Ƶ��ɹ��Ƿ���Թ���
    private bool isInSight = false; // �Ƿ��������Ұ��

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

        // �������ڷ����ڣ����Ҳ��������Ұ�ڣ��Ҿ����������������ɹֿ��Խ���׷��
        if (playerInside && distance >= playerScript.vision && canAttack && !isInSight)
        {
            ChasePlayer();
        }

        // ���� isInSight Ϊ false��ÿ֡����Ҫ���¼��
        isInSight = false;
    }

    public void SetInPlayerSight(bool inSight)
    {
        isInSight = inSight; // �����Ƿ��������Ұ��
    }

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
    }
}
