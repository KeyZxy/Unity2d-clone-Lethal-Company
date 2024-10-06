using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Torch : MonoBehaviour
{
    public Text tex;
    private GameObject play;
    public Player player; // 存放主角对象
    public Image image;
    public void Change()
    {
        if (player.Coin >= 100)
        {
            player.Coin -= 100;
            tex.text = "你已经获得手电筒";
            image.gameObject.SetActive(false);
            //player.Torch.SetActive(true);
        }
        else
        {
            tex.text = "金币不足，无法购买";
        }
    }
}
