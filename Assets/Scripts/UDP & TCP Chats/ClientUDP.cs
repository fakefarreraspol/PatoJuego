using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClientUDP : Client
{
    public TMP_InputField nameInputField;
    public TMP_InputField ipInputField;
    public Button joinGameButton;

    private UdpClient udpClient;
    private int port = 1803;

    private User user;

    void Start()
    {
        udpClient = new UdpClient();
        user = FindObjectOfType<UserInfo>().user;

        //MessageToSend jguse = new MessageToSend(user, "Welcome to Goozy Server");
        //string serializedData = JsonUtility.ToJson(jguse);
        //SendChatMessage(serializedData);
        Intro.OnServerFinishedLoading();
    }

    public override void SendChatMessage(string message) 
    {
        try
        {
            string ip = ipInputField.text;
            byte[] data = Encoding.ASCII.GetBytes(message);
            udpClient.Send(data, data.Length, ip, port);
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error sending message: {e.Message}");
        }
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, port);
        byte[] data = udpClient.EndReceive(ar, ref serverEndPoint);
        string message = Encoding.ASCII.GetString(data);

        Debug.Log("Received response from Goozy server: " + message);
        if (IsValidJson(message))
        {
            MessageToSend deserializedData = JsonUtility.FromJson<MessageToSend>(message);
            Chat.OnMessageReceived(deserializedData);
        }

        BeginReceive();

    }

    void BeginReceive()
    {
        if (udpClient != null)
        {
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
    }

    void OnDestroy()
    {
        if (udpClient != null)
        {
            udpClient.Close();
            udpClient = null;
        }
    }
}
