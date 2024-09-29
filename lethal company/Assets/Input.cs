using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    public InputField inputField;   // 输入框  
    public Text outputText;         // 输出框  

    void Start()
    {
        inputField= GameObject.Find("InputField").GetComponent<InputField>();
    }

    
    public void testclick()
    {
        Debug.Log("点击了");
    }
    public void testChange()
    {
        Debug.Log("Change:"+inputField.text);
    }
    public void testEnd()
    {
        Debug.Log("End:" + inputField.text);
    }
}