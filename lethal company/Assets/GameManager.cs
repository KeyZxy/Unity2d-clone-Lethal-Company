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
    private int rand; // �����Ŀ�궨��Ϊ��Ա����  

    void Start()
    {
        play = GameObject.FindGameObjectWithTag("Player");
        playerComponent = play.GetComponent<Player>();

        // ����Ϸ��ʼʱ�������׬ǮĿ��  
        rand = UnityEngine.Random.Range(100, 250);
    }

    // Update is called once per frame  
    void Update()
    {
        gamingTimeNow += Time.deltaTime;
        TextLivingTime += Time.deltaTime;
        playerTotalCoin = playerComponent.Coin;
        countTime.text = $"��ʱ��{(int)gamingTimeNow}s";
        countCoin.text = $"��Ǯ��{playerTotalCoin}";
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
                    SceneManager.LoadScene("win"); //��Ϸ������������ת���ʤ��  
                }
                else
                {
                    SceneManager.LoadScene("lose"); //��Ϸ������������ת���ʧ��  
                }
            }
        }
        else
        {
            SceneManager.LoadScene("lose");
        }
    }
}