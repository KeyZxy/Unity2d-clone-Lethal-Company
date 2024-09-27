using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public List<GameObject> skills = new List<GameObject>();  // �����ɵĵ����б�
    private BoxCollider2D roomCollider;
    private bool hasGenerated = false; // ��־λ����ʾ�Ƿ��Ѿ����ɹ�����

    void Start()
    {
        // ��ȡ����� BoxCollider2D ���
        roomCollider = GetComponent<BoxCollider2D>();
        if (roomCollider == null)
        {
            Debug.LogError("����ȱ�� BoxCollider2D �����");
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasGenerated)
        {
            GenerateProps();
            hasGenerated = true; // ��ֹ�ظ�����
        }
    }

    void GenerateProps()
    {
        int objectCount = Random.Range(2, 5);  // ���� 2 �� 4 ������
        Vector2 roomSize = roomCollider.size;
        Vector2 roomOffset = roomCollider.offset;

        float roomLeft = transform.position.x - roomSize.x / 2 + roomOffset.x;
        float roomRight = transform.position.x + roomSize.x / 2 + roomOffset.x;
        float roomBottom = transform.position.y - roomSize.y / 2 + roomOffset.y;
        float roomTop = transform.position.y + roomSize.y / 2 + roomOffset.y;

        for (int i = 0; i < objectCount; i++)
        {
            GameObject profPrefab;
            Collider2D[] colliders;
            float x;
            float y;
            do
            {
                // �������ɵ����꣬ȷ���ڷ�����
                x = Random.Range(roomLeft, roomRight);
                y = Random.Range(roomBottom, roomTop);

                int profIndex = Random.Range(0, skills.Count);
                profPrefab = skills[profIndex];
                Vector2 generatorPosition = new Vector2(x, y);
                // �������λ���Ƿ��ص�
                colliders = Physics2D.OverlapBoxAll(generatorPosition, new Vector2(profPrefab.transform.localScale.x, profPrefab.transform.localScale.y), 0);
            }
            while (colliders.Length != 0);  // ��������ص�����

            // ʵ��������
            GameObject skill = Instantiate(profPrefab, new Vector3(x, y, 0), Quaternion.identity);
            skill.transform.SetParent(transform); // ����������Ϊ������Ӷ���
        }
    }
}
