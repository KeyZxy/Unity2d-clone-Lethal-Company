using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Torch : MonoBehaviour
{
    public Text tex;
    private GameObject play;
    public Player player; // ������Ƕ���
    public Image image;
    public void Change()
    {
        if (player.Coin >= 100)
        {
            player.Coin -= 100;
            tex.text = "���Ѿ�����ֵ�Ͳ";
            image.gameObject.SetActive(false);
            //player.Torch.SetActive(true);
        }
        else
        {
            tex.text = "��Ҳ��㣬�޷�����";
        }
    }
}
