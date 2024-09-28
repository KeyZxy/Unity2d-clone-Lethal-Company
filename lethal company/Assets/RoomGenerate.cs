using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    // ����ö��  
    public enum Direction
    {
        TOP = 0,
        RIGHT = 1,
        LEFT = 3,
        BOTTOM = 2
    }

    // ��������Ԥ����  
    public GameObject startRoomPrefab;
    public GameObject indoorRoomPrefab;
    public GameObject outdoorRoomPrefab;

    // ����Ԥ���壬��Ϊ���ں�����  
    public GameObject[] indoorEnemyPrefabs; // ���ڵ���Ԥ����  
    public GameObject[] outdoorEnemyPrefabs; // �������Ԥ����  

    //����Ԥ����
    public GameObject[] chests;
    // ����������  
    public int maxCreateNum;

    // ����㼶  
    public LayerMask roomLayer;

    // ��ǰ�����б�  
    public List<Room> roomList = new List<Room>();

    // ���ɵ�  
    public Transform spawnPoint;

    // ���ɵ�ƫ����  
    public float xOffset, yOffset;

    // ���뾶  
    public float roomColliderRadius;

    // ���ɵ���ʼ����ͽ�������  
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

        // ������ʼ�ͽ�������  
        startRoom = roomList[0].gameObject;
        endRoom = roomList[maxCreateNum - 1].gameObject;

        // ���ɵ���  
        SpawnEnemies();
       SpawnChest();
    }

    /// <summary>  
    /// ���������ͼ  
    /// </summary>  
    void CreateRoom()
    {
        for (int i = 0; i < maxCreateNum; i++)
        {
            // ��һ����������ʼ����  
            if (i == 0)
            {
                CreateRoomObj(startRoomPrefab, spawnPoint.position);
            }
            else
            {
                // ����������ڻ����ⷿ��  
                GameObject roomPrefab = Random.Range(0, 2) == 0 ? indoorRoomPrefab : outdoorRoomPrefab;
                CreateRoomObj(roomPrefab, spawnPoint.position);
            }

            // ���ɷ����������ɵ�λ��  
            RandomDirection();
        }
    }




    /// <summary>  
    /// ����������䷽��  
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

        // ȷ����ǰλ��û�з���  
        while (Physics2D.OverlapCircle(spawnPoint.position, roomColliderRadius, roomLayer))
        {
            RandomDirection();
        }
    }

    /// <summary>  
    /// ��������  
    /// </summary>  
    void CreateRoomObj(GameObject roomPrefab, Vector3 pos)
    {
        GameObject obj = Instantiate(roomPrefab, pos, Quaternion.identity);
        roomList.Add(obj.GetComponent<Room>());
    }

    /// <summary>  
    /// ��⵱ǰ������Χ�ķ�������  
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
        /// ���ɵ��ˣ����ݷ������;�������  
        /// </summary>  
        void SpawnEnemies()
        {
            foreach (Room room in roomList)
            {
                BoxCollider2D roomCollider = room.GetComponent<BoxCollider2D>();
                //Vector3 spawnPos = room.transform.position + new Vector3(Random.Range(-xOffset / 2, xOffset / 2), Random.Range(-yOffset / 2, yOffset / 2), 0);

                // ��鷿������  
                if (room.CompareTag("InSide")) // ���ڷ���  
                {
                    int indoorEnemyCount = Random.Range(1, 4); // ������� 1 �� 3 ֮��ĵ�������  
                    for (int i = 0; i < indoorEnemyCount; i++)
                    {
                        GameObject enemyToSpawn = indoorEnemyPrefabs[Random.Range(0, indoorEnemyPrefabs.Length)];
                        Vector2 spawnPosition = new Vector2(
                        Random.Range(roomCollider.bounds.min.x, roomCollider.bounds.max.x),
                        Random.Range(roomCollider.bounds.min.y, roomCollider.bounds.max.y)
                    ); // Ϊÿ�������������λ��  
                        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
                    }
                }
                else if (room.CompareTag("OutSide")) // ���ⷿ��  
                {
                    int outdoorEnemyCount = Random.Range(1, 4); // ������� 1 �� 3 ֮��ĵ�������  
                    for (int i = 0; i < outdoorEnemyCount; i++)
                    {
                        GameObject enemyToSpawn = outdoorEnemyPrefabs[Random.Range(0, outdoorEnemyPrefabs.Length)];
                        Vector2 spawnPosition = new Vector2(
                        Random.Range(roomCollider.bounds.min.x, roomCollider.bounds.max.x),
                        Random.Range(roomCollider.bounds.min.y, roomCollider.bounds.max.y)); // Ϊÿ�������������λ��  
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
            if (room.CompareTag("InSide")) // ���ڷ���  
            {
                int chestCount = Random.Range(2, 5);
                for (int i = 0; i < chestCount; i++)
                {
                    GameObject chestToSpawn = chests[Random.Range(0, chests.Length)];
                    Vector2 spawnPosition = new Vector2(
                    Random.Range(roomCollider.bounds.min.x, roomCollider.bounds.max.x),
                    Random.Range(roomCollider.bounds.min.y, roomCollider.bounds.max.y)
                ); // Ϊÿ�������������λ��  
                    Instantiate(chestToSpawn, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    }
