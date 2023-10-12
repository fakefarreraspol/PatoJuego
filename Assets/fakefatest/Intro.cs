using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public TMP_InputField inputfield;
    public GameObject managerr;
    // Start is called before the first frame update

    public void buttonClicked()
    {
        if(inputfield.text != string.Empty) FindObjectOfType<manager>().user = new User(inputfield.text);
        else
        {
            FindObjectOfType<manager>().user = new User();
        }
        DontDestroyOnLoad(managerr);
        SceneManager.LoadScene("Chat");
    }
}
