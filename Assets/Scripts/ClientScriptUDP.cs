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
    public InputField messageInputField;
    public Button sendButton;

    private UdpClient udpClient;
    private int port = 1803;

    void Start()
    {
        udpClient = new UdpClient();
        sendButton.onClick.AddListener(SendMessageToServer);
    }

    void SendMessageToServer()
    {
        string message = messageInputField.text;
        byte[] data = Encoding.ASCII.GetBytes(message);
        udpClient.Send(data, data.Length, "127.0.0.1", port);

        // Also listen for a response
        udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, port);
        byte[] data = udpClient.EndReceive(ar, ref serverEndPoint);
        string message = Encoding.ASCII.GetString(data);
        Debug.Log("Received response from server: " + message);
    }

    void OnDestroy()
    {
        udpClient.Close();
    }
}
