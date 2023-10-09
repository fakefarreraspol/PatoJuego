using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ClientScriptUDP : MonoBehaviour
{
    public InputField nameInputField;
    public InputField ipInputField;
    public Button joinGameButton;

    private UdpClient udpClient;
    private int port = 1803;

    void Start()
    {
        udpClient = new UdpClient();
        joinGameButton.onClick.AddListener(SendMessageToServer);
    }

    void SendMessageToServer()
    {
        string ip = ipInputField.text;
        string name = nameInputField.text;
        byte[] data = Encoding.ASCII.GetBytes(name);
        udpClient.Send(data, data.Length, ip, port);

        // Also listen for a response
        udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, port);
        byte[] data = udpClient.EndReceive(ar, ref serverEndPoint);
        string message = Encoding.ASCII.GetString(data);
        Debug.Log("Received response from Goozy server: " + message);
    }

    void OnDestroy()
    {
        udpClient.Close();
    }
}
