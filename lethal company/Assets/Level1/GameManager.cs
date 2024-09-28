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
    public Text countTime; // ��ʱ���ı�
    public Text countCoin; // ��Ǯ�ı�
    public Text aim; // Ŀ���ı�
    private float TextLivingTime;
    private GameObject play;
    private int rand; // ���Ŀ��
    private int startHour = 7; // ��Ϸ��ʼʱ��Сʱ
   // private int startMinute = 0; // ��Ϸ��ʼʱ�ķ���

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
        countCoin.text = $"��Ǯ��{playerTotalCoin}";

        // ���㵱ǰʱ��
        int currentHour = startHour + (int)(gamingTimeNow / 60);
        int currentMinute = (int)(gamingTimeNow % 60);

        // ����Сʱ������24�����
        if (currentHour >= 24)
        {
            currentHour -= 24;
        }

        // ���¼�ʱ�ı�
        countTime.text = $"ʱ�䣺{currentHour:D2}:{currentMinute:D2}";

        if (TextLivingTime > 0.5f)
        {
            aim.text = $"��������Ϸ��׬ǮĿ���ǣ�{rand}";
        }

        if (TextLivingTime > 3f)
        {
            aim.text = "";
        }

        // �����Ϸ�Ƿ����
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
