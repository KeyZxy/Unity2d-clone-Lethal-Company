using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sell : MonoBehaviour
{
    private bool playerInArea = false;  // 检查玩家是否在区域内
    private Player playerComponent;  // 玩家组件
    public Canvas shop;
    private bool isShopActive = false; // 输入框是否激活 

    private void Start()
    {
        shop.gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // 确保进入区域的是玩家
        {
            playerInArea = true;
            playerComponent = other.GetComponent<Player>();  // 获取玩家组件
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // 确保离开区域的是玩家
        {
            playerInArea = false;
            playerComponent = null;  // 清除玩家组件
        }
    }

    void Update()
    {
        if (shop == null)
        {
            GameObject shopob = GameObject.FindWithTag("Shop");
            shop=shopob.GetComponent<Canvas>();
        }
        // 检查玩家是否在区域内并按下E键
        if (playerInArea && playerComponent != null && Input.GetKeyDown(KeyCode.E))
        {
            TrySellChest();
        }
        // 检查玩家是否在区域内并按下R键
        if (playerInArea && playerComponent != null && Input.GetKeyDown(KeyCode.R))
        {
            //// 记录当前场景名称  
            //PlayerPrefs.SetString("CurrentMainScene", SceneManager.GetActiveScene().name);
            //// 可以加载商店场景，例如：  
            //SceneManager.LoadScene("Shop"); // 替换为你的商店场景名称  
            //shop.gameObject.SetActive(true);
            ToggleShop();
            if (playerComponent != null)
            {
                playerComponent.SetMouseLookEnabled(!isShopActive); // 关闭商店时启用 MouseLook  
            }
        }
    }
    public void ToggleShop()
    {
        isShopActive = !isShopActive;
        shop.gameObject.SetActive(isShopActive);
    }

    void TrySellChest()//按一下e放下物体，按第二下拾取物体并出售
    {
        // 检查 playerComponent 和玩家是否持有物体
        if (playerComponent != null && playerComponent.pickedObject != null)
        {
            // 检查物体的标签
            GameObject pickedObject = playerComponent.pickedObject;
            if (pickedObject.CompareTag("Normal"))
            {
                SellChest(50, pickedObject);  // 普通宝箱
            }
            else if (pickedObject.CompareTag("Silver"))
            {
                SellChest(100, pickedObject);  // 银宝箱
            }
            else if (pickedObject.CompareTag("Gold"))
            {
                SellChest(150, pickedObject);  // 金宝箱
            }
            else
            {
                Debug.Log("无法出售，物体标签不匹配");
            }
        }
        else
        {
            Debug.Log("没有宝箱可出售");
        }
    }

    // 出售宝箱的逻辑
    void SellChest(int coinValue, GameObject chest)
    {
        playerComponent.ChangeCoin(coinValue);  // 增加玩家的金币数量
        Debug.Log($"宝箱已出售，获得金币：{coinValue}");
        Destroy(chest);  // 销毁宝箱
        playerComponent.pickedObject = null;  // 清除玩家持有的物体
        playerComponent.isHoldingObject = false;  // 更新玩家状态
    }
}
