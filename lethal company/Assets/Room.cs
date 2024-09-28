using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // 四个门的引用
    public GameObject doorLeft, doorRight, doorTop, doorBottom;

    // 是否启用某个门
    public bool isLeft;
    public bool isRight;
    public bool isTop;
    public bool isBottom;

    // 门的数量
    public int doorNumber;

    // Start is called before the first frame update
    void Start()
    {
        // 根据布尔值启用或禁用对应的门
        if (doorLeft != null) doorLeft.SetActive(isLeft);
        if (doorRight != null) doorRight.SetActive(isRight);
        if (doorBottom != null) doorBottom.SetActive(isBottom);
        if (doorTop != null) doorTop.SetActive(isTop);

        // 更新门的数量
        UpdateRoomState();
    }

    public void UpdateRoomState()
    {
        doorNumber = 0; // 重置计数器

        if (isLeft) doorNumber++;
        if (isRight) doorNumber++;
        if (isTop) doorNumber++;
        if (isBottom) doorNumber++;
    }
}
