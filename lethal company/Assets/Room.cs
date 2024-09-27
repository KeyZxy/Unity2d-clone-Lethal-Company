using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool playerInRoom = false;

    public Transform cameraPosition;
    public Transform[] doorPosition;
    public Transform[] movePosition;

 
    void Update()
    {
        if (playerInRoom)
        {
            CameraSetting();
        }
    }
    void CameraSetting()
    {
        float cameraMoveSpeed = 0.3f;
        GameControl.instance.myCamera.transform.SetParent(cameraPosition);
        GameControl.instance.myCamera.transform.localPosition = Vector3.MoveTowards(GameControl.instance.myCamera.transform.localPosition, new Vector3(0, 0, 0), cameraMoveSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRoom = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRoom = false;
        }
    }
}
