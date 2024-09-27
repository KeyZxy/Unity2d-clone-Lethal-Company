using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public List<GameObject> skills = new List<GameObject>();  // 可生成的道具列表
    private BoxCollider2D roomCollider;
    private bool hasGenerated = false; // 标志位，表示是否已经生成过道具

    void Start()
    {
        // 获取房间的 BoxCollider2D 组件
        roomCollider = GetComponent<BoxCollider2D>();
        if (roomCollider == null)
        {
            Debug.LogError("房间缺乏 BoxCollider2D 组件。");
            return;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasGenerated)
        {
            GenerateProps();
            hasGenerated = true; // 防止重复生成
        }
    }

    void GenerateProps()
    {
        int objectCount = Random.Range(2, 5);  // 生成 2 到 4 个道具
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
                // 计算生成的坐标，确保在房间内
                x = Random.Range(roomLeft, roomRight);
                y = Random.Range(roomBottom, roomTop);

                int profIndex = Random.Range(0, skills.Count);
                profPrefab = skills[profIndex];
                Vector2 generatorPosition = new Vector2(x, y);
                // 检查生成位置是否重叠
                colliders = Physics2D.OverlapBoxAll(generatorPosition, new Vector2(profPrefab.transform.localScale.x, profPrefab.transform.localScale.y), 0);
            }
            while (colliders.Length != 0);  // 避免道具重叠生成

            // 实例化道具
            GameObject skill = Instantiate(profPrefab, new Vector3(x, y, 0), Quaternion.identity);
            skill.transform.SetParent(transform); // 将道具设置为房间的子对象
        }
    }
}
