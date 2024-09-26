using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public float generateLength;  // �������ɵķ�Χ���ȣ���ѡ��  
    public float generateWidth;   // �������ɵķ�Χ��ȣ���ѡ��  
    private int objectCount;      // ÿ�����ɵĵ�������  
    public List<GameObject> skills = new List<GameObject>();  // �����ɵĵ����б�  
    public List<GameObject> generatedObjects = new List<GameObject>();  // �����ɵĵ����б�  
    private BoxCollider2D roomCollider;
    private bool hasGenerated = false; // ��־λ����ʾ�Ƿ��Ѿ����ɹ�����  

    // Start is called before the first frame update  
    void Start()
    {
        // ��ȡ����� BoxCollider2D ���  
        roomCollider = GetComponent<BoxCollider2D>();
        if (roomCollider == null)
        {
            Debug.LogError("Missing BoxCollider2D component on the room.");
            return;
        }
    }

    // ����ҽ��봥������ʱ�����ɵ���  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ȷ������ҽ���  
        {
            Debug.Log("��ҽ��������������");
            if (!hasGenerated)  // ����Ƿ�δ���ɹ�����  
            {
                GenerateProps();  // ��ҽ���ʱ���ɵ���  
                hasGenerated = true; // ���ñ�־λΪ������  
            }
        }
    }

    void GenerateProps()
    {
        objectCount = Random.Range(4, 7);  // ���� 2 �� 4 ������  
        Debug.Log($"��ʼ���ɵ���");

        Vector2 roomSize = roomCollider.size;         // ��ȡ����ĳߴ�  
        Vector2 roomOffset = roomCollider.offset;     // ��ȡ�����ƫ��  

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
            generatedObjects.Add(skill);  // �����ɵĵ�����ӵ��б���  
        }
    }

    // ������뿪��������ʱ���������Ҫ���ԼӶ����߼�  
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ȷ��������뿪  
        {
            Debug.Log("����뿪������������");
        }
    }
}