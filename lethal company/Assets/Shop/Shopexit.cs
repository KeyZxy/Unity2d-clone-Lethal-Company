using UnityEngine;
using UnityEngine.SceneManagement;

public class Shopexit : MonoBehaviour
{
    void Update()
    {
        // ����Ƿ��� R ��  
        if (Input.GetKeyDown(KeyCode.R))
        {
            // �� PlayerPrefs �л�ȡ��ǰ����������Ϸ����  
            string mainScene = PlayerPrefs.GetString("CurrentMainScene", "Main1"); // Ĭ��ֵΪ Main1  
            SceneManager.LoadScene(mainScene);
        }
    }
}