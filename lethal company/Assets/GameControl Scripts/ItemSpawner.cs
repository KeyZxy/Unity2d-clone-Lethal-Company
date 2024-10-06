using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] chests; // 宝藏预制体  
    public int minChestCount = 2; // 最小宝藏数量  
    public int maxChestCount = 5; // 最大宝藏数量  

    void Start()
    {
        SpawnChests();
    }

    void SpawnChests()
    {
        // 确保至少有一件宝藏预制体可以生成  
        if (chests.Length == 0) return;

        // 随机生成宝藏数量  
        int chestCount = Random.Range(minChestCount, maxChestCount);

        // 随机位置生成宝藏  
        BoxCollider2D roomCollider = GetComponent<BoxCollider2D>();
        for (int i = 0; i < chestCount; i++)
        {
            GameObject chestToSpawn = chests[Random.Range(0, chests.Length)];
            Vector2 spawnPosition = new Vector2(
                Random.Range(roomCollider.bounds.min.x, roomCollider.bounds.max.x),
                Random.Range(roomCollider.bounds.min.y, roomCollider.bounds.max.y)
            );
            Instantiate(chestToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}