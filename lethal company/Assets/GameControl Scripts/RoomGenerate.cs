using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject player;
    
    public GameObject startRoomPrefab;
    public GameObject indoorRoomPrefab;
    public GameObject outdoorRoomPrefab;

    public GameObject[] indoorEnemyPrefabs; // 室内敌人预制体  
    public GameObject[] outdoorEnemyPrefabs; // 室外敌人预制体  
   
    public GameObject[] chests; //宝藏预制体
    public int maxCreateNum; // 房间最大个数  
    public LayerMask roomLayer;

    public List<Room> roomList = new List<Room>();
    public Transform spawnPoint;

    public float xOffset, yOffset; // 生成点偏移量  
    public float roomColliderRadius; // 检测半径

    public GameObject startRoom;
    public GameObject endRoom;

    void Start()
    {
        CreateRoom();

        foreach (var room in roomList)
        {
            CheckRoomDoor(room, room.transform.position);
        }

        startRoom = roomList[0].gameObject;
        endRoom = roomList[maxCreateNum - 1].gameObject;

        // 生成敌人
        SpawnEnemies();
        SpawnChest();
    }
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // 确保寻找带有"Player"标签的对象  
        }
        PlayerInside();
    }
    void CreateRoom()
    {
        for (int i = 0; i < maxCreateNum; i++)
        {
            if (i == 0)
            {
                CreateRoomObj(startRoomPrefab, spawnPoint.position);
            }
            else
            {
                GameObject roomPrefab = Random.Range(0, 2) == 0 ? indoorRoomPrefab : outdoorRoomPrefab;
                CreateRoomObj(roomPrefab, spawnPoint.position);
            }

            RandomDirection();
        }
    }

    void RandomDirection()
    {
        Direction direction = (Direction)Random.Range(0, 4);

        switch (direction)
        {
            case Direction.LEFT:
                spawnPoint.position += new Vector3(-xOffset, 0, 0);
                break;
            case Direction.RIGHT:
                spawnPoint.position += new Vector3(xOffset, 0, 0);
                break;
            case Direction.BOTTOM:
                spawnPoint.position += new Vector3(0, -yOffset, 0);
                break;
            case Direction.TOP:
                spawnPoint.position += new Vector3(0, yOffset, 0);
                break;
        }

        while (Physics2D.OverlapCircle(spawnPoint.position, roomColliderRadius, roomLayer))
        {
            RandomDirection();
        }
    }

    void CreateRoomObj(GameObject roomPrefab, Vector3 pos)
    {
        GameObject obj = Instantiate(roomPrefab, pos, Quaternion.identity);
        roomList.Add(obj.GetComponent<Room>());
    }

    void CheckRoomDoor(Room room, Vector3 pos)
    {
        Collider2D left = Physics2D.OverlapCircle(pos + new Vector3(-xOffset, 0, 0), roomColliderRadius, roomLayer);
        Collider2D right = Physics2D.OverlapCircle(pos + new Vector3(xOffset, 0, 0), roomColliderRadius, roomLayer);
        Collider2D top = Physics2D.OverlapCircle(pos + new Vector3(0, yOffset, 0), roomColliderRadius, roomLayer);
        Collider2D bottom = Physics2D.OverlapCircle(pos + new Vector3(0, -yOffset, 0), roomColliderRadius, roomLayer);

        room.isLeft = left;
        room.isRight = right;
        room.isTop = top;
        room.isBottom = bottom;

        room.UpdateRoomState();
    }

    // 敌人生成和巡逻点生成逻辑
    void SpawnEnemies()
    {
        foreach (Room room in roomList)
        {
            BoxCollider2D roomCollider = room.GetComponent<BoxCollider2D>();

            if (room.CompareTag("InSide"))
            {
                int indoorEnemyCount = Random.Range(2, 4);
                for (int i = 0; i < indoorEnemyCount; i++)
                {
                    GameObject enemyToSpawn = indoorEnemyPrefabs[Random.Range(0, indoorEnemyPrefabs.Length)];
                    Vector2 spawnPosition = new Vector2(
                        Random.Range(roomCollider.bounds.min.x, roomCollider.bounds.max.x),
                        Random.Range(roomCollider.bounds.min.y, roomCollider.bounds.max.y)
                    );
                    GameObject enemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

                    // 将生成的位置传递给 Enemy 脚本
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    enemyScript.origin = spawnPosition; // 设置敌人的初始位置
                    enemyScript.patrolPoints = GeneratePatrolPoints(roomCollider); // 设置巡逻点
                    enemyScript.roomCollider = roomCollider; // 设置当前房间的碰撞体

                    // 如果是 Slime，直接传递 roomCollider  
                    Slime slimeScript = enemy.GetComponent<Slime>();
                    if (slimeScript != null)
                    {
                        slimeScript.roomCollider = roomCollider; // 将房间的碰撞体传递给 Slime  
                    }

                    // 如果是 Huge，直接传递 roomCollider  
                    Huge hugeScript = enemy.GetComponent<Huge>();
                    if (hugeScript != null)
                    {
                        hugeScript.roomCollider = roomCollider; // 将房间的碰撞体传递给 Slime  
                    }

                }
            }
            else if (room.CompareTag("OutSide"))
            {
                int outdoorEnemyCount = Random.Range(1, 2);
                for (int i = 0; i < outdoorEnemyCount; i++)
                {
                    GameObject enemyToSpawn = outdoorEnemyPrefabs[Random.Range(0, outdoorEnemyPrefabs.Length)];
                    Vector2 spawnPosition = new Vector2(
                        Random.Range(roomCollider.bounds.min.x, roomCollider.bounds.max.x),
                        Random.Range(roomCollider.bounds.min.y, roomCollider.bounds.max.y)
                    );
                    GameObject enemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);

                    // 将生成的位置传递给 Enemy 脚本
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    enemyScript.origin = spawnPosition; // 设置敌人的初始位置
                    enemyScript.patrolPoints = GeneratePatrolPoints(roomCollider); // 设置巡逻点
                    enemyScript.roomCollider = roomCollider; // 设置当前房间的碰撞体

                    // 如果是 Slime，直接传递 roomCollider  
                    Slime slimeScript = enemy.GetComponent<Slime>();
                    if (slimeScript != null)
                    {
                        slimeScript.roomCollider = roomCollider; // 将房间的碰撞体传递给 Slime  
                    }

                    // 如果是 Huge，直接传递 roomCollider  
                    Huge hugeScript = enemy.GetComponent<Huge>();
                    if (hugeScript != null)
                    {
                        hugeScript.roomCollider = roomCollider; // 将房间的碰撞体传递给 Slime  
                    }

                }
            }
        }
    }


    Transform[] GeneratePatrolPoints(BoxCollider2D roomCollider)
    {
        int numPatrolPoints = Random.Range(2, 5);
        Transform[] patrolPoints = new Transform[numPatrolPoints];

        for (int i = 0; i < numPatrolPoints; i++)
        {
            GameObject point = new GameObject("PatrolPoint");
            point.transform.position = new Vector2(
                Random.Range(roomCollider.bounds.min.x, roomCollider.bounds.max.x),
                Random.Range(roomCollider.bounds.min.y, roomCollider.bounds.max.y)
            );
            patrolPoints[i] = point.transform;
        }

        return patrolPoints;
    }

    // 方向枚举
    public enum Direction
    {
        TOP = 0,
        RIGHT = 1,
        LEFT = 3,
        BOTTOM = 2
    }
    //生成宝藏
    void SpawnChest()
    {
        foreach (Room room in roomList)
        {
            if (room.CompareTag("InSide")) // 室内房间  
            {
                // 为每个室内房间添加 ItemSpawner 组件  
                ItemSpawner itemSpawner = room.gameObject.AddComponent<ItemSpawner>();
                itemSpawner.chests = chests; // 设置宝藏预制体  
                itemSpawner.minChestCount = 2; // 设置最小宝藏数量  
                itemSpawner.maxChestCount = 5; // 设置最大宝藏数量  

                // Start 方法将在 ItemSpawner 内自动调用，将宝藏生成逻辑从这里移除  
            }
        }
    }
    //判断玩家是否在房间内
    public void PlayerInside()
    {
        foreach (Room room in roomList)
        {
            BoxCollider2D roomCollider = room.GetComponent<BoxCollider2D>();
            if (roomCollider.bounds.Contains(player.transform.position))
            {
                room.PlayerInRoom = true;
                CameraControl.instance.ChangeTarget(room.transform);
            }
            else
            {
                room.PlayerInRoom = false;
            }
        }
    }
}
