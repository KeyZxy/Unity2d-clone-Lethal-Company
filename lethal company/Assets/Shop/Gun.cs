using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gun : MonoBehaviour
{
    public Text tex;
    private GameObject play;
    public Player player; // 存放主角对象
    // Start is called before the first frame update
    public void Change()
    {
        if (player.Coin >= 200)
        {
            player.Coin -= 200;
            tex.text = "你已经获得手枪";
            player.skillName = "Gun";
        }
        else
        {
            tex.text = "金币不足，无法购买";
        }
    }

   
}
