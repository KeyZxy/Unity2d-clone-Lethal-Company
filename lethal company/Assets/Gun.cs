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
        player.Coin=0;
        tex.text = "���Ѿ������ǹ";
        player.skillName = "Gun";

    }

   
}
