using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    /// <summary>
    /// 方向
    /// </summary>
    public enum Direction
    {
        TOP = 0,
        RIGHT = 1,
        LEFT = 3,
        BOTTOM = 2
    }

    //基础房间预制体
    public GameObject baseRoom;

    //下次生成房间的方向
    private Direction direction;

    //房间最大个数
    public int maxCreateNum;

    //房间层级
    public LayerMask roomLayer;

    //当前房间列表
    private List<Room> roomList = new List<Room>();

    public GameObject startRoom;
    public GameObject endRoom;

    //生成点
    public Transform spawnPoint;
    //生成点偏移量
    public float xOffset, yOffset;
    //检测半径（当前位置是否有一个房间）
    public float roomColliderRadius;

   

    // Start is called before the first frame update
    void Start()
    {
        CreateRoom();

        foreach (var room in roomList)
        {
            CheckRoomDoor(room, room.transform.position);
        }

        startRoom = roomList[0].gameObject;
        endRoom = roomList[maxCreateNum - 1].gameObject;

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 随机创建房间方向
    /// </summary>
    void RandomDirection()
    {
        do
        {
            direction = (Direction)Random.Range(0, 4);

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

        } while (Physics2D.OverlapCircle(spawnPoint.position, roomColliderRadius, roomLayer));


    }

    /// <summary>
    /// 创建随机地图
    /// </summary>
    void CreateRoom()
    {
        for (int i = 0; i < maxCreateNum; i++)
        {
            CreateRoomObj(baseRoom, spawnPoint.position);
            RandomDirection();
        }
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    /// <param name="room">房间预制体对象</param>
    /// <param name="pos">位置</param>
    void CreateRoomObj(GameObject room, Vector3 pos)
    {
        GameObject obj = Instantiate(room, pos, Quaternion.identity);
        // obj.transform.position = pos;
        roomList.Add(obj.GetComponent<Room>());
    }

    /// <summary>
    /// 检测当前房间周围的房间数量
    /// </summary>
    /// <param name="room"></param>
    /// <param name="pos"></param>
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


}

