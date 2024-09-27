using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerate : MonoBehaviour
{
    int[] dy = new int[4] { -1, 0, 1, 0 };
    int[] dx = new int[4] { 0, 1, 0, -1 };

    List<GameObject> doors;

    public GameObject[,] roomList;

    public GameObject[,] roomPrefabs;

    [Header("Unity Setup")]
    public GameObject[] rooms; // Assume [0] = Start Room, [1] = Indoor, [2] = Outdoor
    public GameObject doorPrefab; // Only one type of door


    void Start()
    {

    }

    public void SetPrefabs()
    {
        roomPrefabs = new GameObject[1, 3];

        // Assign rooms to the array (0: Start Room, 1: Indoor, 2: Outdoor)
        roomPrefabs[0, 0] = rooms[0]; // Start Room
        roomPrefabs[0, 1] = rooms[1]; // Indoor
        roomPrefabs[0, 2] = rooms[2]; // Outdoor
    }

    public void ClearRoom()
    {
        // Destroy existing rooms
        if (roomList != null)
        {
            for (int i = 0; i < roomList.GetLength(0); i++)
            {
                for (int j = 0; j < roomList.GetLength(1); j++)
                {
                    if (roomList[i, j] != null)
                    {
                        Destroy(roomList[i, j]);
                    }
                }
            }
        }

        // Destroy all doors
        if (doors != null)
        {
            foreach (GameObject door in doors)
            {
                Destroy(door);
            }
            doors.Clear();
        }
    }

    public void CreateRoom(int stage, int size)
    {
        roomList = new GameObject[size, size];
        doors = new List<GameObject>();

        Vector3 roomPos = new Vector3(0, 0, 0);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int roomNum = GameControl.instance.stageGenerate.stageArr[i, j];
                if (roomNum == 0)
                {
                    roomPos += new Vector3(15, 0, 0);
                    continue;
                }

                GameObject room = Instantiate(roomPrefabs[0, roomNum - 1], roomPos, Quaternion.identity);
                roomList[i, j] = room;

                roomPos += new Vector3(15, 0, 0);
            }

            roomPos = new Vector3(0, roomPos.y, 0);
            roomPos += new Vector3(0, -10, 0);
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int roomNum = GameControl.instance.stageGenerate.stageArr[i, j];
                if (roomNum == 0)
                {
                    continue;
                }

                CreateDoor(i, j);
            }
        }
    }

    public void CreateDoor(int y, int x)
    {
        for (int i = 0; i < 4; i++)
        {
            int ny = y + dy[i];
            int nx = x + dx[i];
            if (ny < 0 || nx < 0 || ny >= 5 || nx >= 5)
                continue;
            if (GameControl.instance.stageGenerate.stageArr[ny, nx] == 0)
                continue;

            GameObject door = Instantiate(doorPrefab);
            door.transform.SetParent(roomList[y, x].GetComponent<Room>().doorPosition[i]);
            door.transform.localPosition = Vector3.zero;
            door.transform.localRotation = Quaternion.identity;

            door.GetComponent<Door>().doorDir = i;
            

            doors.Add(door);
        }
    }

    private void Update()
    {
        // No need to check for room clearance or door states.
    }
}
