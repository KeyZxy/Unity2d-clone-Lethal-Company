using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : MonoBehaviour
{
    public InputField inputField;   // �����  
    public Text outputText;         // �����  

    void Start()
    {
        inputField= GameObject.Find("InputField").GetComponent<InputField>();
    }

    
    public void testclick()
    {
        Debug.Log("�����");
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