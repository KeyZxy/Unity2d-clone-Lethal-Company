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
    public Text countTime; // 计时器文本
    public Text countCoin; // 金钱文本
    public Text aim; // 目标文本
    private float TextLivingTime;
    private GameObject play;
    private int rand; // 随机目标
    private int startHour = 7; // 游戏开始时的小时
   // private int startMinute = 0; // 游戏开始时的分钟

    void Start()
    {
        play = GameObject.FindGameObjectWithTag("Player");
        playerComponent = play.GetComponent<Player>();
        rand = UnityEngine.Random.Range(100, 250);
    }

    void Update()
    {
        gamingTimeNow += Time.deltaTime;
        TextLivingTime += Time.deltaTime;
        playerTotalCoin = playerComponent.Coin;
        countCoin.text = $"金钱：{playerTotalCoin}";

        // 计算当前时间
        int currentHour = startHour + (int)(gamingTimeNow / 60);
        int currentMinute = (int)(gamingTimeNow % 60);

        // 处理小时数超过24的情况
        if (currentHour >= 24)
        {
            currentHour -= 24;
        }

        // 更新计时文本
        countTime.text = $"时间：{currentHour:D2}:{currentMinute:D2}";

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
                    SceneManager.LoadScene("win");
                }
                else
                {
                    SceneManager.LoadScene("lose");
                }
            }
        }
        else
        {
            SceneManager.LoadScene("lose");
        }
    }
}
