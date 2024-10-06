using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //玩家是否在房间
    public bool PlayerInRoom = false;
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
        ConfigureDoor(doorLeft, isLeft);
        ConfigureDoor(doorRight, isRight);
        ConfigureDoor(doorTop, isTop);
        ConfigureDoor(doorBottom, isBottom);

        // 更新门的数量
        UpdateRoomState();
    }

    private void ConfigureDoor(GameObject door, bool isActive)
    {
        if (door != null)
        {
            // 递归设置door及其子物体的SpriteRenderer可见性
            SetSpriteVisibility(door.transform, isActive);

            // 保持 Collider2D 活动和设为 Trigger
            Collider2D collider = door.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = isActive; // 只有当门启用时，isTrigger 是 true
            }
        }
    }

    // 递归设置door及其所有子物体的SpriteRenderer可见性
    private void SetSpriteVisibility(Transform doorTransform, bool isActive)
    {
        // 设置当前对象的SpriteRenderer
        SpriteRenderer spriteRenderer = doorTransform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = isActive;
        }

        // 遍历所有子物体并递归设置它们的SpriteRenderer
        foreach (Transform child in doorTransform)
        {
            SetSpriteVisibility(child, isActive);
        }
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
