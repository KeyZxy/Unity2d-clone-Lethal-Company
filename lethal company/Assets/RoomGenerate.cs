using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    /// <summary>
    /// ����
    /// </summary>
    public enum Direction
    {
        TOP = 0,
        RIGHT = 1,
        LEFT = 3,
        BOTTOM = 2
    }

    //��������Ԥ����
    public GameObject baseRoom;

    //�´����ɷ���ķ���
    private Direction direction;

    //����������
    public int maxCreateNum;

    //����㼶
    public LayerMask roomLayer;

    //��ǰ�����б�
    private List<Room> roomList = new List<Room>();

    public GameObject startRoom;
    public GameObject endRoom;

    //���ɵ�
    public Transform spawnPoint;
    //���ɵ�ƫ����
    public float xOffset, yOffset;
    //���뾶����ǰλ���Ƿ���һ�����䣩
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
    /// ����������䷽��
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
    /// ���������ͼ
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
    /// ��������
    /// </summary>
    /// <param name="room">����Ԥ�������</param>
    /// <param name="pos">λ��</param>
    void CreateRoomObj(GameObject room, Vector3 pos)
    {
        GameObject obj = Instantiate(room, pos, Quaternion.identity);
        // obj.transform.position = pos;
        roomList.Add(obj.GetComponent<Room>());
    }

    /// <summary>
    /// ��⵱ǰ������Χ�ķ�������
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

