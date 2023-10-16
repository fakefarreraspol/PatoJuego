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
        //.onClick.AddListener(SendMessageToServer());
        user = FindObjectOfType<UserInfo>().user;
        MessageToSend jguse = new MessageToSend(user, "soygay");
        string serializedDatajosegay = JsonUtility.ToJson(jguse);
        SendChatMessage(serializedDatajosegay);
        Intro.OnServerFinishedLoading();
    }

    public override void SendChatMessage(string josemaricon) 
    {
        string ip = ipInputField.text;
        string name = nameInputField.text;
        byte[] data = Encoding.ASCII.GetBytes(josemaricon);
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
