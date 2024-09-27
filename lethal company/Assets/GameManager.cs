using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerTotalCoin;
    public float gamingTime = 180;
    public float gamingTimeNow = 0;
    private Player playerComponent;
    public Text countTime;
    public Text countCoin;
    public Text aim;
    private float TextLivingTime;
    private GameObject play;
    private int rand; // 将随机目标定义为成员变量  

    void Start()
    {
        play = GameObject.FindGameObjectWithTag("Player");
        playerComponent = play.GetComponent<Player>();

        // 在游戏开始时随机生成赚钱目标  
        rand = UnityEngine.Random.Range(100, 250);
    }

    // Update is called once per frame  
    void Update()
    {
        gamingTimeNow += Time.deltaTime;
        TextLivingTime += Time.deltaTime;
        playerTotalCoin = playerComponent.Coin;
        countTime.text = $"计时：{(int)gamingTimeNow}s";
        countCoin.text = $"金钱：{playerTotalCoin}";
        if (TextLivingTime > 0.5f)
        {
            aim.text = $"您本局游戏的赚钱目标是：{rand}";
        }

        if (TextLivingTime > 3f)
        {
            aim.text = ""; 
        }

        // 检查游戏是否结束
        if (playerComponent.Hp > 0)
        {
            if (gamingTimeNow >= gamingTime)
            {
                if (playerTotalCoin >= rand)
                {
                    SceneManager.LoadScene("win"); //游戏场景结束，跳转玩家胜利  
                }
                else
                {
                    SceneManager.LoadScene("lose"); //游戏场景结束，跳转玩家失败  
                }
            }
        }
        else
        {
            SceneManager.LoadScene("lose");
        }
    }
}