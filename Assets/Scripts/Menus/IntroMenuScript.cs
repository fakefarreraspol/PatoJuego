using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField port;
    public TMP_InputField ip;

    private string portNum;
    private string ipNum;

    // Update is called once per frame
    public void GoBackToUserCreationScene()
    {
        SaveAndLoad.DeleteFile();
        Destroy(FindObjectOfType<UserManager>().gameObject);
        SceneManager.LoadScene("CreateUser");
    }


    public void JoinGame()
    {
        GetInputFields();
        GameObject ServerData;
        ServerData = new GameObject("ServerData");
        ServerData.AddComponent<ServerData>();
        ServerData.GetComponent<ServerData>().ServerIP = ipNum;
        ServerData.GetComponent<ServerData>().ServerPort = portNum;

        DontDestroyOnLoad(ServerData);

        SceneManager.LoadScene("LobbyClient");
    }
    public void HostGame()
    {
        GetInputFields();
        GameObject ServerData;
        ServerData = new GameObject("ServerData");
        ServerData.AddComponent<ServerData>();
        ServerData.GetComponent<ServerData>().ServerIP = ipNum;
        ServerData.GetComponent<ServerData>().ServerPort = portNum;

        DontDestroyOnLoad(ServerData);

        SceneManager.LoadScene("Lobby");
    }

    private void GetInputFields()
    {
        portNum = port.text;
        ipNum = ip.text;
    }
}
