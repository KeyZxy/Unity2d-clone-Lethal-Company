using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // �ĸ��ŵ�����
    public GameObject doorLeft, doorRight, doorTop, doorBottom;

    // �Ƿ�����ĳ����
    public bool isLeft;
    public bool isRight;
    public bool isTop;
    public bool isBottom;

    // �ŵ�����
    public int doorNumber;

    // Start is called before the first frame update
    void Start()
    {
        // ���ݲ���ֵ���û���ö�Ӧ����
        if (doorLeft != null) doorLeft.SetActive(isLeft);
        if (doorRight != null) doorRight.SetActive(isRight);
        if (doorBottom != null) doorBottom.SetActive(isBottom);
        if (doorTop != null) doorTop.SetActive(isTop);

        // �����ŵ�����
        UpdateRoomState();
    }

    public void UpdateRoomState()
    {
        doorNumber = 0; // ���ü�����

        if (isLeft) doorNumber++;
        if (isRight) doorNumber++;
        if (isTop) doorNumber++;
        if (isBottom) doorNumber++;
    }
}
