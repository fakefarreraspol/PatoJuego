using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System;
//using JetBrains.Annotations;
//using Mono.Cecil;

public class ClientTCP : Client
{
    public TMP_InputField ipInputField;
    public TMP_InputField nameInputField;
    private User uSeR;


    TcpClient client;


    void Start()
    {
        uSeR = FindObjectOfType<UserInfo>().user;
        ConnectToServer();
        
    }

    //private void Update()
    //{
    //    NetworkStream stream = client.GetStream();

    //    byte[] data = new byte[1024];
    //    int bytesRead = stream.Read(data, 0, data.Length);

    //    if (bytesRead > 0)
    //    {
    //        string message = Encoding.UTF8.GetString(data, 0, bytesRead);
    //        Console.WriteLine("Received message from the server: " + message);
    //    }
    //}

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
            
            string serializedData = JsonUtility.ToJson(uSeR);
            byte[] jsonData = Encoding.ASCII.GetBytes(serializedData);
            //stream.Write(jsonData, 0, jsonData.Length);

            //stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
            User test = JsonUtility.FromJson<User>(serializedData);

            Debug.Log("Player name sent to server" + test.userName);

            
            Intro.OnServerFinishedLoading();
        }
        else
        {
            Debug.LogError("Cannot write to the stream.");
        }

        
    }
    public override void SendChatMessage(string info)
    {
        if (client == null || !client.Connected)
        {
            Debug.LogError("Not connected to server");
            return;
        }
        Debug.Log("info   "+info);
        NetworkStream stream = client.GetStream();
        if (stream.CanWrite)
        {
            MessageToSend tst = JsonUtility.FromJson<MessageToSend>(info);

            Debug.Log(tst.message);

            byte[] jsonData = Encoding.ASCII.GetBytes(info);
            stream.Write(jsonData, 0, jsonData.Length);


            Debug.Log("Message sent!"+ tst.message);

            //Intro.OnServerFinishedLoading();
        }
        else
        {
            Debug.LogError("Cannot write to the stream.");
        }
    }
}
