using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sell : MonoBehaviour
{
    private bool playerInArea = false;  // �������Ƿ���������
    private Player playerComponent;  // ������

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // ȷ����������������
        {
            playerInArea = true;
            playerComponent = other.GetComponent<Player>();  // ��ȡ������
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // ȷ���뿪����������
        {
            playerInArea = false;
            playerComponent = null;  // ���������
        }
    }

    void Update()
    {
        // �������Ƿ��������ڲ�����E��
        if (playerInArea && playerComponent != null && Input.GetKeyDown(KeyCode.E))
        {
            TrySellChest();
        }
        // �������Ƿ��������ڲ�����R��
        if (playerInArea && playerComponent != null && Input.GetKeyDown(KeyCode.R))
        {
            // ��¼��ǰ��������  
            PlayerPrefs.SetString("CurrentMainScene", SceneManager.GetActiveScene().name);
            // ���Լ����̵곡�������磺  
            SceneManager.LoadScene("Shop"); // �滻Ϊ����̵곡������  
        }
    }

    void TrySellChest()//��һ��e�������壬���ڶ���ʰȡ���岢����
    {
        // ��� playerComponent ������Ƿ��������
        if (playerComponent != null && playerComponent.pickedObject != null)
        {
            // �������ı�ǩ
            GameObject pickedObject = playerComponent.pickedObject;
            if (pickedObject.CompareTag("Normal"))
            {
                SellChest(50, pickedObject);  // ��ͨ����
            }
            else if (pickedObject.CompareTag("Silver"))
            {
                SellChest(100, pickedObject);  // ������
            }
            else if (pickedObject.CompareTag("Gold"))
            {
                SellChest(150, pickedObject);  // ����
            }
            else
            {
                Debug.Log("�޷����ۣ������ǩ��ƥ��");
            }
        }
        else
        {
            Debug.Log("û�б���ɳ���");
        }
    }

    // ���۱�����߼�
    void SellChest(int coinValue, GameObject chest)
    {
        playerComponent.ChangeCoin(coinValue);  // ������ҵĽ������
        Debug.Log($"�����ѳ��ۣ���ý�ң�{coinValue}");
        Destroy(chest);  // ���ٱ���
        playerComponent.pickedObject = null;  // �����ҳ��е�����
        playerComponent.isHoldingObject = false;  // �������״̬
    }
}
