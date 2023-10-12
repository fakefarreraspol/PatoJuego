using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public TMP_InputField inputfield;
    public GameObject userInfo;

    public GameObject host;
    public GameObject client;


    public static Action OnServerFinishedLoading;
    // Start is called before the first frame update
    private void Start()
    {
        host.SetActive(false);
        client.SetActive(false);
    }
    public void Join()
    {
        client.SetActive(true);
        if (inputfield.text != string.Empty) FindObjectOfType<UserInfo>().user = new User(inputfield.text);
        else
        {
            FindObjectOfType<UserInfo>().user = new User();
        }


        DontDestroyOnLoad(userInfo);
        DontDestroyOnLoad(client);
       // SceneManager.LoadScene("Chat");
    }
    public void Host()
    {
        host.SetActive(true);

        if (inputfield.text != string.Empty) FindObjectOfType<UserInfo>().user = new User(inputfield.text);
        else
        {
            FindObjectOfType<UserInfo>().user = new User();
        }
        
        

        DontDestroyOnLoad(userInfo);
        DontDestroyOnLoad(host);
        SceneManager.LoadScene("Chat");
    }


    private void OnEnable()
    {
        OnServerFinishedLoading += changeScene;
    }

    private void changeScene()
    {
        SceneManager.LoadScene("Chat");
    }
}
