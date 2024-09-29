using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    private Vector2 currentCoordinate;
    public RoomGenerator roomGenerator; // ȷ����Inspector���ѷ���  
    public GameObject miniRoomPrefab;   // ���㷿���Ԥ����  
    public Transform miniRoomNode;      // ���㷿��ĸ��ڵ�  
    private GameObject[,] miniRoomArray; // ��ά������������㷿��  
    private Color white = new Color(1f, 1f, 1f, 1f);
    private Color gray = new Color(0.6f, 0.6f, 0.6f, 1f);
    private Color black = new Color(0.3f, 0.3f, 0.3f, 1f);

    private void Start()
    {
        // ȷ��RoomGenerator�ѷ��䲢�ҷ���������  
        if (roomGenerator != null)
        {
            CreateMiniMap();
        }
        else
        {
            Debug.LogError("RoomGeneratorδ���䣬����Inspector�е�����!");
        }
    }

    public void CreateMiniMap()
    {
        int width = roomGenerator.maxCreateNum;  // �������з�����һά������  
        miniRoomArray = new GameObject[width, 1];

        for (int i = 0; i < roomGenerator.roomList.Count; i++)
        {
            if (roomGenerator.roomList[i] != null)
            {
                var cell = Instantiate(miniRoomPrefab, miniRoomNode);
                miniRoomArray[i, 0] = cell;

                // ������������λ��  
                cell.GetComponent<RectTransform>().localPosition = new Vector2(i * 30, 0); // �������λ��  

                // ���ó�ʼ��ɫΪ��ɫ  
                cell.GetComponent<Image>().color = black;

                Debug.Log($"�������㷿�䵥Ԫ: {i}"); // �������  
            }
        }

        currentCoordinate = new Vector2(0, 0);
        UpdateCurrentRoomColor(white);
    }

    public void UpdateMiniMap(Vector2 moveDirection)
    {
        // ���µ�ǰ������ɫΪ��ɫ  
        UpdateCurrentRoomColor(gray);

        // ���µ�ǰ����  
        currentCoordinate.x += moveDirection.x;

        // ȷ����������Ч��Χ��  
        currentCoordinate.x = Mathf.Clamp(currentCoordinate.x, 0, miniRoomArray.GetLength(0) - 1);

        // ���µ�ǰ������ɫΪ��ɫ  
        UpdateCurrentRoomColor(white);

        // ���������ͼ��ͼλ��  
        miniRoomNode.localPosition += new Vector3(-moveDirection.x * 30, 0, 0); // �����귴������Ӧ�Ӿ�Ч��  
    }

    private void UpdateCurrentRoomColor(Color color)
    {
        // ���õ�ǰ�������ɫ  
        var currentRoom = miniRoomArray[(int)currentCoordinate.x, (int)currentCoordinate.y];
        if (currentRoom != null)
        {
            currentRoom.GetComponent<Image>().color = color;
        }
    }

    public void ShowAllMiniMap()
    {
        // ���������㷿����ɫ����Ϊ��ɫ  
        for (int i = 0; i < miniRoomArray.GetLength(0); i++)
        {
            var cell = miniRoomArray[i, 0];
            if (cell != null)
            {
                cell.GetComponent<Image>().color = black; // �����з�����Ϊ��ɫ  
            }
        }

        UpdateMiniMap(Vector2.zero); // ���õ�ǰ����Ϊ��ʼ״̬  
    }
}