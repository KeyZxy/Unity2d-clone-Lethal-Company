using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    // 方向枚举  
    public enum Direction
    {
        TOP = 0,
        RIGHT = 1,
        LEFT = 3,
        BOTTOM = 2
    }

    // 基础房间预制体  
    public GameObject startRoomPrefab;
    public GameObject indoorRoomPrefab;
    public GameObject outdoorRoomPrefab;

    // 敌人预制体，分为室内和室外  
    public GameObject[] indoorEnemyPrefabs; // 室内敌人预制体  
    public GameObject[] outdoorEnemyPrefabs; // 室外敌人预制体  

    //宝藏预制体
    public GameObject[] chests;
    // 房间最大个数  
    public int maxCreateNum;

    // 房间层级  
    public LayerMask roomLayer;

    // 当前房间列表  
    public List<Room> roomList = new List<Room>();

    // 生成点  
    public Transform spawnPoint;

    // 生成点偏移量  
    public float xOffset, yOffset;

    // 检测半径  
    public float roomColliderRadius;

    // 生成的起始房间和结束房间  
    public GameObject startRoom;
    public GameObject endRoom;

    // Start is called before the first frame update  
    void Start()
    {
        CreateRoom();

        foreach (var room in roomList)
        {
            CheckRoomDoor(room, room.transform.position);
        }

        // 设置起始和结束房间  
        startRoom = roomList[0].gameObject;
        endRoom = roomList[maxCreateNum - 1].gameObject;

        // 生成敌人  
        SpawnEnemies();
       SpawnChest();
    }

    /// <summary>  
    /// 创建随机地图  
    /// </summary>  
    void CreateRoom()
    {
        for (int i = 0; i < maxCreateNum; i++)
        {
            // 第一个房间是起始房间  
            if (i == 0)
            {
                CreateRoomObj(startRoomPrefab, spawnPoint.position);
            }
            else
            {
                // 随机生成室内或室外房间  
                GameObject roomPrefab = Random.Range(0, 2) == 0 ? indoorRoomPrefab : outdoorRoomPrefab;
                CreateRoomObj(roomPrefab, spawnPoint.position);
            }

            // 生成房间后更新生成点位置  
            RandomDirection();
        }
    }




    /// <summary>  
    /// 随机创建房间方向  
    /// </summary>  
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

        // 确保当前位置没有房间  
        while (Physics2D.OverlapCircle(spawnPoint.position, roomColliderRadius, roomLayer))
        {
            RandomDirection();
        }
    }

    /// <summary>  
    /// 创建房间  
    /// </summary>  
    void CreateRoomObj(GameObject roomPrefab, Vector3 pos)
    {
        GameObject obj = Instantiate(roomPrefab, pos, Quaternion.identity);
        roomList.Add(obj.GetComponent<Room>());
    }

    /// <summary>  
    /// 检测当前房间周围的房间数量  
    /// </summary>  
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
    
        /// <summary>  
        /// 生成敌人，根据房间类型决定生成  
        /// </summary>  
        void SpawnEnemies()
        {
            foreach (Room room in roomList)
            {
                BoxCollider2D roomCollider = room.GetComponent<BoxCollider2D>();
                //Vector3 spawnPos = room.transform.position + new Vector3(Random.Range(-xOffset / 2, xOffset / 2), Random.Range(-yOffset / 2, yOffset / 2), 0);

                // 检查房间类型  
                if (room.CompareTag("InSide")) // 室内房间  
                {
                    int indoorEnemyCount = Random.Range(1, 4); // 随机生成 1 到 3 之间的敌人数量  
                    for (int i = 0; i < indoorEnemyCount; i++)
                    {
                        GameObject enemyToSpawn = indoorEnemyPrefabs[Random.Range(0, indoorEnemyPrefabs.Length)];
                        Vector2 spawnPosition = new Vector2(
                        Random.Range(roomCollider.bounds.min.x, roomCollider.bounds.max.x),
                        Random.Range(roomCollider.bounds.min.y, roomCollider.bounds.max.y)
                    ); // 为每个敌人随机生成位置  
                        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                    }
                }
                else if (room.CompareTag("OutSide")) // 室外房间  
                {
                    int outdoorEnemyCount = Random.Range(1, 4); // 随机生成 1 到 3 之间的敌人数量  
                    for (int i = 0; i < outdoorEnemyCount; i++)
                    {
                        GameObject enemyToSpawn = outdoorEnemyPrefabs[Random.Range(0, outdoorEnemyPrefabs.Length)];
                        Vector2 spawnPosition = new Vector2(
                        Random.Range(roomCollider.bounds.min.x, roomCollider.bounds.max.x),
                        Random.Range(roomCollider.bounds.min.y, roomCollider.bounds.max.y)); // 为每个敌人随机生成位置  
                        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                    }
                }
            }
        }

    void SpawnChest()
    {
        foreach (Room room in roomList)
        {
            BoxCollider2D roomCollider = room.GetComponent<BoxCollider2D>();
            if (room.CompareTag("InSide")) // 室内房间  
            {
                int chestCount = Random.Range(2, 5);
                for (int i = 0; i < chestCount; i++)
                {
                    GameObject chestToSpawn = chests[Random.Range(0, chests.Length)];
                    Vector2 spawnPosition = new Vector2(
                    Random.Range(roomCollider.bounds.min.x, roomCollider.bounds.max.x),
                    Random.Range(roomCollider.bounds.min.y, roomCollider.bounds.max.y)
                ); // 为每个宝藏随机生成位置  
                    Instantiate(chestToSpawn, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    }
