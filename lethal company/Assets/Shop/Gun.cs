using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gun : MonoBehaviour
{
    public Text tex;
    private GameObject play;
    public Player player; // ������Ƕ���
    // Start is called before the first frame update
    public void Change()
    {
        if (player.Coin >= 200)
        {
            player.Coin -= 200;
            tex.text = "���Ѿ������ǹ";
            player.skillName = "Gun";
        }
        else
        {
            tex.text = "��Ҳ��㣬�޷�����";
        }
    }

   
}
