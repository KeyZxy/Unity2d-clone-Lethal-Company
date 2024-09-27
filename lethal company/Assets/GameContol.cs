using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    public int stageLevel; 
    public int stageSize; 
    public int stageMinimunRoom;
    public GameObject playerObject;

    public StageGenerate stageGenerate;
    public RoomGenerate roomGenerate;
    public GameObject myCamera;

    private float curTime;

    void Start()
    {
        SetStage(1); 
        roomGenerate.SetPrefabs(); 

        Invoke("StageStart", 0.3f);
    }
    void Update()
    {
#if TEST_MODE
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("Main2");
        }
#endif
        if (Input.GetKey(KeyCode.R))
        {
            curTime += Time.deltaTime;

            if (curTime >= 2f) 
                SceneManager.LoadScene("Main2"); 
        }
        if (Input.GetKeyUp(KeyCode.R) && curTime <= 2.4f)
        {
            curTime = 0;
        }
    }
    public void StageStart()
    {
        myCamera.transform.SetParent(null);
        while (true)
        {
            if (stageGenerate.CreateStage(stageSize, stageMinimunRoom))
            {
                roomGenerate.ClearRoom(); 
          
                roomGenerate.CreateRoom(stageLevel, stageSize); 
                myCamera.transform.position = playerObject.transform.position;
                break;
            }
        }
    }
    public void SetStage(int stage)
    {
        stageLevel = stage;
        stageSize = 5;
        stageMinimunRoom = 8;
    }

 }
