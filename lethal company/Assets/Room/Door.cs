using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  

public class Door : MonoBehaviour  
{  
    public int doorDir = -1; // 0=上，1=右，2=下，3=左  

    public GameObject closeDoor;  
    public GameObject openDoor;  

    public void OpenDoor()  
    {  
        closeDoor.SetActive(false);  
        openDoor.SetActive(true);  
    }  

    public void CloseDoor()  
    {  
        closeDoor.SetActive(true);  
        openDoor.SetActive(false);  
    }  

    private void OnTriggerStay2D(Collider2D collision)  
    {  
        if (collision.gameObject.CompareTag("Player"))  
        {  
            // 确保只有门的 isTrigger 为 true 时才能打开  
            if (gameObject.GetComponent<Collider2D>().isTrigger)  
            {  
                if (Input.GetKey(KeyCode.W) && doorDir == 0)  
                {  
                    OpenDoor();  
                }  
                else if (Input.GetKey(KeyCode.D) && doorDir == 1)  
                {  
                    OpenDoor();  
                }  
                else if (Input.GetKey(KeyCode.S) && doorDir == 2)  
                {  
                    OpenDoor();  
                }  
                else if (Input.GetKey(KeyCode.A) && doorDir == 3)  
                {  
                    OpenDoor();  
                }  
            }  
        }  
    }  

    private void OnTriggerExit2D(Collider2D collision)  
    {  
        if (collision.gameObject.CompareTag("Player"))  
        {  
            CloseDoor(); // 玩家离开时关闭门  
        }  
    }  
}