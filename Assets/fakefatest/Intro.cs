using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    public TMP_InputField nameTextInput;
    public GameObject userInfo;

    public GameObject host;
    public GameObject client;
    private TMP_Dropdown dropdown;

    public static Action OnServerFinishedLoading;
    // Start is called before the first frame update
    private void Start()
    {
        dropdown = FindObjectOfType<TMP_Dropdown>();
        host.SetActive(false);
        client.SetActive(false);
    }
    public void Join()
    {
        
        if (nameTextInput.text != string.Empty) FindObjectOfType<UserInfo>().user = new User(nameTextInput.text);
        else
        {
            FindObjectOfType<UserInfo>().user = new User();
        }

        client.SetActive(true);
        host.SetActive(false);


        if (dropdown.value == 0)
        {
            client.GetComponent<ClientTCP>().enabled = true;
        }
        else client.GetComponent<ClientUDP>().enabled = true;



        Destroy(host);
        DontDestroyOnLoad(userInfo);
        DontDestroyOnLoad(client);
       // SceneManager.LoadScene("Chat");
    }
    public void Host()
    {
        host.SetActive(true);
        client.SetActive(false);

        if (nameTextInput.text != string.Empty) FindObjectOfType<UserInfo>().user = new User(nameTextInput.text);
        else
        {
            FindObjectOfType<UserInfo>().user = new User();
        }


        if (dropdown.value == 0)
        {
            host.GetComponent<HostTCP>().enabled = true;
        }
        else host.GetComponent<ServerUDP>().enabled = true;


        Destroy(client);
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
        OnServerFinishedLoading -= changeScene;
    }
}
