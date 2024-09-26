using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public float generateLength;  // 道具生成的范围长度（可选）  
    public float generateWidth;   // 道具生成的范围宽度（可选）  
    private int objectCount;      // 每次生成的道具数量  
    public List<GameObject> skills = new List<GameObject>();  // 可生成的道具列表  
    public List<GameObject> generatedObjects = new List<GameObject>();  // 已生成的道具列表  
    private BoxCollider2D roomCollider;
    private bool hasGenerated = false; // 标志位，表示是否已经生成过道具  

    // Start is called before the first frame update  
    void Start()
    {
        // 获取房间的 BoxCollider2D 组件  
        roomCollider = GetComponent<BoxCollider2D>();
        if (roomCollider == null)
        {
            Debug.LogError("Missing BoxCollider2D component on the room.");
            return;
        }
    }

    // 当玩家进入触发区域时，生成道具  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 确保是玩家进入  
        {
            Debug.Log("玩家进入道具生成区域");
            if (!hasGenerated)  // 检查是否还未生成过道具  
            {
                GenerateProps();  // 玩家进入时生成道具  
                hasGenerated = true; // 设置标志位为已生成  
            }
        }
    }

    void GenerateProps()
    {
        objectCount = Random.Range(4, 7);  // 生成 2 到 4 个道具  
        Debug.Log($"开始生成道具");

        Vector2 roomSize = roomCollider.size;         // 获取房间的尺寸  
        Vector2 roomOffset = roomCollider.offset;     // 获取房间的偏移  

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
            generatedObjects.Add(skill);  // 将生成的道具添加到列表中  
        }
    }

    // 当玩家离开触发区域时，如果有需要可以加额外逻辑  
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // 确保是玩家离开  
        {
            Debug.Log("玩家离开道具生成区域");
        }
    }
}