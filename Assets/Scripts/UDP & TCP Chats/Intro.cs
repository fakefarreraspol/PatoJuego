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

    //Join button is clicked
    public void Join()
    {
        //Create a new user using the name the user choose or creating one with a random name if the user didn't input any name
        if (nameTextInput.text != string.Empty) FindObjectOfType<UserInfo>().user = new User(nameTextInput.text);
        else
        {
            FindObjectOfType<UserInfo>().user = new User();
        }
        //Activating client gameObject and making sure host gameObject is not active
        client.SetActive(true);
        host.SetActive(false);

        //Activating the type of client depending on the dropdown of the UI
        if (dropdown.value == 0)
        {
            client.GetComponent<ClientTCP>().enabled = true;
        }
        else client.GetComponent<ClientUDP>().enabled = true;

        Destroy(host);
        DontDestroyOnLoad(userInfo);
        DontDestroyOnLoad(client);
    }

    //Host button pressed
    public void Host()
    {
        //Activating host gameObject and making sure client gameObject is not active
        host.SetActive(true);
        client.SetActive(false);

        //Create a new user using the name the user choose or creating one with a random name if the user didn't input any name
        if (nameTextInput.text != string.Empty) FindObjectOfType<UserInfo>().user = new User(nameTextInput.text);
        else
        {
            FindObjectOfType<UserInfo>().user = new User();
        }

        //Activating the type of host depending on the dropdown of the UI
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
        OnServerFinishedLoading += ChangeScene;
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("Chat");
        OnServerFinishedLoading -= ChangeScene;
    }
}
