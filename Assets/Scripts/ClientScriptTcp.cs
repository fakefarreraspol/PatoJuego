using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ClientScriptTcp : MonoBehaviour
{
    public InputField ipInputField;
    public InputField nameInputField;
    public Button joinGameButton;   

    TcpClient client;

    void Start()
    {
        joinGameButton.onClick.AddListener(ConnectToServer);
    }

    void ConnectToServer()
    {
        string serverIP = ipInputField.text;
        string playerName = nameInputField.text;

        Debug.Log("Attempting to connect to Goozy server at " + serverIP);

        client = new TcpClient();

        try
        {
            client.Connect(serverIP, 1803); 

            UnityEngine.Debug.Log("Connected to server at " + serverIP);

            // After successfully connecting start sending and receiving messages.

            SendData(playerName);

        }
        catch (SocketException socketException)
        {
            UnityEngine.Debug.LogError("Socket exception: " + socketException);
        }
    }

    void SendData(string message)
    {
        if (client == null || !client.Connected)
        {
            Debug.LogError("Not connected to server");
            return;
        }

        NetworkStream stream = client.GetStream();
        if (stream.CanWrite)
        {
            byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(message);
            stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            Debug.Log("Player name sent to server");
        }
        else
        {
            Debug.LogError("Cannot write to the stream.");
        }
    }
}