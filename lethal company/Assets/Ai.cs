using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject player; // ��Ҷ���
    public float attackRange = 1.0f; // ������Χ
    public float moveSpeed = 3.0f;   // �ƶ��ٶ�

    // Update is called once per frame
    void Update()
    {
        // ������˺����֮��ľ���
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // ���������ڹ�����Χ�������������ƶ�
        if (distanceToPlayer > attackRange)
        {
            // ��ȡ������ҵķ�������
            Vector2 direction = (player.transform.position - transform.position).normalized;

            // �����ƶ�λ��
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

            // ���µ��˳���ȷ��ʼ���������
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
