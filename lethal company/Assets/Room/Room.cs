using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //����Ƿ��ڷ���
    public bool PlayerInRoom = false;
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
        ConfigureDoor(doorLeft, isLeft);
        ConfigureDoor(doorRight, isRight);
        ConfigureDoor(doorTop, isTop);
        ConfigureDoor(doorBottom, isBottom);

        // �����ŵ�����
        UpdateRoomState();
    }

    private void ConfigureDoor(GameObject door, bool isActive)
    {
        if (door != null)
        {
            // �ݹ�����door�����������SpriteRenderer�ɼ���
            SetSpriteVisibility(door.transform, isActive);

            // ���� Collider2D �����Ϊ Trigger
            Collider2D collider = door.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = isActive; // ֻ�е�������ʱ��isTrigger �� true
            }
        }
    }

    // �ݹ�����door���������������SpriteRenderer�ɼ���
    private void SetSpriteVisibility(Transform doorTransform, bool isActive)
    {
        // ���õ�ǰ�����SpriteRenderer
        SpriteRenderer spriteRenderer = doorTransform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = isActive;
        }

        // �������������岢�ݹ��������ǵ�SpriteRenderer
        foreach (Transform child in doorTransform)
        {
            SetSpriteVisibility(child, isActive);
        }
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
