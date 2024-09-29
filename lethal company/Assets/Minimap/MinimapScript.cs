using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    private Vector2 currentCoordinate;
    public RoomGenerator roomGenerator; // 确保在Inspector中已分配  
    public GameObject miniRoomPrefab;   // 迷你房间的预制体  
    public Transform miniRoomNode;      // 迷你房间的父节点  
    private GameObject[,] miniRoomArray; // 二维数组来存放迷你房间  
    private Color white = new Color(1f, 1f, 1f, 1f);
    private Color gray = new Color(0.6f, 0.6f, 0.6f, 1f);
    private Color black = new Color(0.3f, 0.3f, 0.3f, 1f);

    private void Start()
    {
        // 确保RoomGenerator已分配并且房间已生成  
        if (roomGenerator != null)
        {
            CreateMiniMap();
        }
        else
        {
            Debug.LogError("RoomGenerator未分配，请检查Inspector中的设置!");
        }
    }

    public void CreateMiniMap()
    {
        int width = roomGenerator.maxCreateNum;  // 假设所有房间在一维数组中  
        miniRoomArray = new GameObject[width, 1];

        for (int i = 0; i < roomGenerator.roomList.Count; i++)
        {
            if (roomGenerator.roomList[i] != null)
            {
                var cell = Instantiate(miniRoomPrefab, miniRoomNode);
                miniRoomArray[i, 0] = cell;

                // 根据索引设置位置  
                cell.GetComponent<RectTransform>().localPosition = new Vector2(i * 30, 0); // 按需调整位置  

                // 设置初始颜色为黑色  
                cell.GetComponent<Image>().color = black;

                Debug.Log($"生成迷你房间单元: {i}"); // 调试输出  
            }
        }

        currentCoordinate = new Vector2(0, 0);
        UpdateCurrentRoomColor(white);
    }

    public void UpdateMiniMap(Vector2 moveDirection)
    {
        // 更新当前房间颜色为灰色  
        UpdateCurrentRoomColor(gray);

        // 更新当前坐标  
        currentCoordinate.x += moveDirection.x;

        // 确保坐标在有效范围内  
        currentCoordinate.x = Mathf.Clamp(currentCoordinate.x, 0, miniRoomArray.GetLength(0) - 1);

        // 更新当前房间颜色为白色  
        UpdateCurrentRoomColor(white);

        // 调整迷你地图视图位置  
        miniRoomNode.localPosition += new Vector3(-moveDirection.x * 30, 0, 0); // 将坐标反向以适应视觉效果  
    }

    private void UpdateCurrentRoomColor(Color color)
    {
        // 设置当前房间的颜色  
        var currentRoom = miniRoomArray[(int)currentCoordinate.x, (int)currentCoordinate.y];
        if (currentRoom != null)
        {
            currentRoom.GetComponent<Image>().color = color;
        }
    }

    public void ShowAllMiniMap()
    {
        // 将所有迷你房间颜色设置为黑色  
        for (int i = 0; i < miniRoomArray.GetLength(0); i++)
        {
            var cell = miniRoomArray[i, 0];
            if (cell != null)
            {
                cell.GetComponent<Image>().color = black; // 将所有房间标记为黑色  
            }
        }

        UpdateMiniMap(Vector2.zero); // 重置当前房间为初始状态  
    }
}