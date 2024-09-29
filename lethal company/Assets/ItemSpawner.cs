using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] chests; // ����Ԥ����  
    public int minChestCount = 2; // ��С��������  
    public int maxChestCount = 5; // ��󱦲�����  

    void Start()
    {
        SpawnChests();
    }

    void SpawnChests()
    {
        // ȷ��������һ������Ԥ�����������  
        if (chests.Length == 0) return;

        // ������ɱ�������  
        int chestCount = Random.Range(minChestCount, maxChestCount);

        // ���λ�����ɱ���  
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