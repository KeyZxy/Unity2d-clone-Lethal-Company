using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f; // ���˵��ƶ��ٶ�
    public int EnemyHp = 10;
    public float changeTime = 3.0f; // �ı䷽���ʱ����
    private float timer;

    public GameObject player; // ������Ƕ���
    public float patrolRadius = 5f; // Ѳ��·���뾶
    public float visionRadius = 10f; // ���˵���Ұ�뾶
    public Transform[] patrolPoints; // Ѳ��·���ϵĸ���·��

    private int currentPatrolIndex = 0; // ��ǰѲ�ߵ������
    protected  bool chasingPlayer = false; // �Ƿ���׷�����
    private bool hasAttacked = false; // �Ƿ��Ѿ�������
    public Vector2 origin; // ��ʼλ��
    private bool returningToOrigin = false; // �Ƿ����ڷ��س�ʼλ��
    public BoxCollider2D roomCollider; // ��ǰ�������ײ��

    public int hurt;//��ͬ���˵Ĺ����˺�

    protected  void Start()
    {
        timer = changeTime;
        origin = transform.position; // ��¼���˵ĳ�ʼλ��
        //roomCollider = GetComponentInParent<BoxCollider2D>(); // ��ȡ��ǰ�������ײ��

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true; // ����Ϊ�˶�ѧ  
        }
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // ȷ��Ѱ�Ҵ���"Player"��ǩ�Ķ���  
        }
        if (!chasingPlayer && !returningToOrigin) // ���û��׷����ң�Ҳû�ڷ��س�ʼλ�ã�ִ��Ѳ���߼�
        {
            Patrol();
        }

        if (!hasAttacked) // �����û�й������������Ұ��Χ
        {
            CheckVision();
        }

        if (returningToOrigin) // ������ڷ���ԭ��
        {
            ReturnToOrigin();
        }
    }
    public void TakeDamage(int damage)
    {
        Debug.Log("���˵�����ֵ������ 1 ��");
        EnemyHp -= damage; // ��������ֵ  
        if (EnemyHp <= 0)
        {
            
            Die(); // ������������  
        }
    }

    private void Die()
    {
        Debug.Log("����������");
        // ��������������߼������粥�Ŷ�����������Ʒ��  
        Destroy(gameObject); // ���ٵ��˶���  
    }
    protected virtual void Patrol() // ������·��֮��Ѳ��
    {
        if (patrolPoints.Length == 0) return; // û������·��ʱ��ֱ�ӷ���

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector2 newPosition = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // ���Ƶ����ƶ��ڵ�ǰ������
        if (roomCollider != null && roomCollider.bounds.Contains(newPosition))
        {
            transform.position = newPosition;
        }

        // �����˵��ﵱǰ·��ʱ��ת����һ��·��
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // ����ѭ��·��
        }
    }

    protected virtual void CheckVision() // �������Ƿ������Ұ��Χ
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= visionRadius) // �������Ұ��Χ��
        {
            chasingPlayer = true; // ����׷��״̬
            ChasePlayer(); // ִ��׷���߼�
        }
        else
        {
            chasingPlayer = false; // ����뿪��Ұ��Χʱ��ֹͣ׷��
        }
    }

    protected virtual void ChasePlayer() // ׷�����
    {
        if (chasingPlayer && !hasAttacked)
        {
            Vector2 targetPosition = Vector2.MoveTowards(transform.position, player.transform.position, speed * 2 * Time.deltaTime);

            // ���Ƶ����ƶ��ڵ�ǰ������
            if (roomCollider != null && roomCollider.bounds.Contains(targetPosition))
            {
                transform.position = targetPosition;
            }

            
           
        }
    }

    private void OnCollisionEnter2D(Collision2D other) // �������ҵ���ײ
    {
        Player playerScript = other.gameObject.GetComponent<Player>(); // ��ȡ��ҽű�
        if (playerScript != null && !hasAttacked) // �����Ҵ�����û�й�����
        {
            playerScript.ChangeHealth(-hurt); // �����������ֵ
            Debug.Log($"��ҵ�����ֵ������{hurt}��");

            // ������ֹͣ׷��������ԭ��
            hasAttacked = true;
            chasingPlayer = false;
            returningToOrigin = true; // ���Ϊ����ԭ��״̬
        }
    }

    protected virtual void ReturnToOrigin() // ���س�ʼλ��
    {
        transform.position = Vector2.MoveTowards(transform.position, origin, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, origin) < 0.1f) // ���������ԭ��
        {
            returningToOrigin = false; // ֹͣ����
            //hasAttacked = false; // ���ù���״̬�������ٴι���
        }
    }
}
