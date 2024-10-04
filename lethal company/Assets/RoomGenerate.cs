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

    public GameObject[] indoorEnemyPrefabs; // ���ڵ���Ԥ����  
    public GameObject[] outdoorEnemyPrefabs; // �������Ԥ����  
   
    public GameObject[] chests; //����Ԥ����
    public int maxCreateNum; // ����������  
    public LayerMask roomLayer;

    public List<Room> roomList = new List<Room>();
    public Transform spawnPoint;

    public float xOffset, yOffset; // ���ɵ�ƫ����  
    public float roomColliderRadius; // ���뾶

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

        // ���ɵ���
        SpawnEnemies();
        SpawnChest();
    }
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // ȷ��Ѱ�Ҵ���"Player"��ǩ�Ķ���  
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

    // �������ɺ�Ѳ�ߵ������߼�
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

                    // �����ɵ�λ�ô��ݸ� Enemy �ű�
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    enemyScript.origin = spawnPosition; // ���õ��˵ĳ�ʼλ��
                    enemyScript.patrolPoints = GeneratePatrolPoints(roomCollider); // ����Ѳ�ߵ�
                    enemyScript.roomCollider = roomCollider; // ���õ�ǰ�������ײ��

                    // ����� Slime��ֱ�Ӵ��� roomCollider  
                    Slime slimeScript = enemy.GetComponent<Slime>();
                    if (slimeScript != null)
                    {
                        slimeScript.roomCollider = roomCollider; // ���������ײ�崫�ݸ� Slime  
                    }

                    // ����� Huge��ֱ�Ӵ��� roomCollider  
                    Huge hugeScript = enemy.GetComponent<Huge>();
                    if (hugeScript != null)
                    {
                        hugeScript.roomCollider = roomCollider; // ���������ײ�崫�ݸ� Slime  
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

                    // �����ɵ�λ�ô��ݸ� Enemy �ű�
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    enemyScript.origin = spawnPosition; // ���õ��˵ĳ�ʼλ��
                    enemyScript.patrolPoints = GeneratePatrolPoints(roomCollider); // ����Ѳ�ߵ�
                    enemyScript.roomCollider = roomCollider; // ���õ�ǰ�������ײ��

                    // ����� Slime��ֱ�Ӵ��� roomCollider  
                    Slime slimeScript = enemy.GetComponent<Slime>();
                    if (slimeScript != null)
                    {
                        slimeScript.roomCollider = roomCollider; // ���������ײ�崫�ݸ� Slime  
                    }

                    // ����� Huge��ֱ�Ӵ��� roomCollider  
                    Huge hugeScript = enemy.GetComponent<Huge>();
                    if (hugeScript != null)
                    {
                        hugeScript.roomCollider = roomCollider; // ���������ײ�崫�ݸ� Slime  
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

    // ����ö��
    public enum Direction
    {
        TOP = 0,
        RIGHT = 1,
        LEFT = 3,
        BOTTOM = 2
    }
    //���ɱ���
    void SpawnChest()
    {
        foreach (Room room in roomList)
        {
            if (room.CompareTag("InSide")) // ���ڷ���  
            {
                // Ϊÿ�����ڷ������ ItemSpawner ���  
                ItemSpawner itemSpawner = room.gameObject.AddComponent<ItemSpawner>();
                itemSpawner.chests = chests; // ���ñ���Ԥ����  
                itemSpawner.minChestCount = 2; // ������С��������  
                itemSpawner.maxChestCount = 5; // ������󱦲�����  

                // Start �������� ItemSpawner ���Զ����ã������������߼��������Ƴ�  
            }
        }
    }
    //�ж�����Ƿ��ڷ�����
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
