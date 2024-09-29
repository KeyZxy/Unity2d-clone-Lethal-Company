using UnityEngine;
using UnityEngine.SceneManagement;

public class Shopexit : MonoBehaviour
{
    void Update()
    {
        // 检测是否按下 R 键  
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 从 PlayerPrefs 中获取当前所处的主游戏场景  
            string mainScene = PlayerPrefs.GetString("CurrentMainScene", "Main1"); // 默认值为 Main1  
            SceneManager.LoadScene(mainScene);
        }
    }
}