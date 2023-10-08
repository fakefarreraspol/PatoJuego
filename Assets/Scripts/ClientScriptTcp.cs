using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ClientScriptTcp : MonoBehaviour
{
    public InputField ipInputField; 
    public Button connectButton;   

    TcpClient client;

    void Start()
    {
        connectButton.onClick.AddListener(ConnectToServer);
    }

    void ConnectToServer()
    {
        string serverIP = ipInputField.text;

        Debug.Log("Attempting to connect to server at " + serverIP);

        client = new TcpClient();

        try
        {
            client.Connect(serverIP, 7777); 

            UnityEngine.Debug.Log("Connected to server at " + serverIP);

            // After successfully connecting, you can start sending and receiving messages.

        }
        catch (SocketException socketException)
        {
            UnityEngine.Debug.LogError("Socket exception: " + socketException);
        }
    }
}
