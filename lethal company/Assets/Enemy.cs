using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f; // ���˵��ƶ��ٶ�
    public float changeTime = 3.0f; // �ı䷽���ʱ����
    private float timer;


    public GameObject player ; // ������Ƕ���
    public float patrolRadius = 5f; // Ѳ��·���뾶
    public float visionRadius = 10f; // ���˵���Ұ�뾶
    public Transform[] patrolPoints; // Ѳ��·���ϵĸ���·��

    private int currentPatrolIndex = 0; // ��ǰѲ�ߵ������
    private bool chasingPlayer = false; // �Ƿ���׷�����
    private bool hasAttacked = false; // �Ƿ��Ѿ�������
    public Vector2 origin; // ��ʼλ��
    private bool returningToOrigin = false; // �Ƿ����ڷ��س�ʼλ��

    void Start()
    {
        timer = changeTime;
        origin = transform.position; // ��¼���˵ĳ�ʼλ��
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

    private void Patrol() // ������·��֮��Ѳ��
    {
        if (patrolPoints.Length == 0) return; // û������·��ʱ��ֱ�ӷ���

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // �����˵��ﵱǰ·��ʱ��ת����һ��·��
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // ����ѭ��·��
        }
    }

    private void CheckVision() // �������Ƿ������Ұ��Χ
    {
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

    private void ChasePlayer() // ׷�����
    {
        if (chasingPlayer && !hasAttacked)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * 2 * Time.deltaTime);

            // ������˽ӽ���ң����Դ�������
            if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)
            {
                // �� OnCollisionEnter2D �д���ʵ�ʵ���ײ�����˺�
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) // �������ҵ���ײ
    {
        Player playerScript = other.gameObject.GetComponent<Player>(); // ��ȡ��ҽű�
        if (playerScript != null && !hasAttacked) // �����Ҵ�����û�й�����
        {
            playerScript.ChangeHealth(-1); // �����������ֵ
            Debug.Log("��ҵ�����ֵ������ 1 ��");

            // ������ֹͣ׷��������ԭ��
            hasAttacked = true;
            chasingPlayer = false;
            returningToOrigin = true; // ���Ϊ����ԭ��״̬
        }
    }

    private void ReturnToOrigin() // ���س�ʼλ��
    {
        transform.position = Vector2.MoveTowards(transform.position, origin, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, origin) < 0.1f) // ���������ԭ��
        {
            returningToOrigin = false; // ֹͣ����
            //hasAttacked = false; // ���ù���״̬�������ٴι���
        }
    }

   
}
