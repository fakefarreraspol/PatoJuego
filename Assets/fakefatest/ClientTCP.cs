using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System;


public class ClientTCP : Client
{
    public TMP_InputField ipInputField;
    public TMP_InputField nameInputField;
    private User uSeR;
    byte[] buffer = new byte[1024];

    TcpClient client;

    void Start()
    {
        uSeR = FindObjectOfType<UserInfo>().user;
        ConnectToServer();
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

        BeginReceive();
    }

    void BeginReceive()
    {
        NetworkStream stream = client.GetStream();
        stream.BeginRead(buffer, 0, buffer.Length, ReceiveCallback, stream);
    }

    void ReceiveCallback(IAsyncResult AR)
    {
        NetworkStream stream = (NetworkStream)AR.AsyncState;
        int bytesRead = stream.EndRead(AR);

        if (bytesRead > 0)
        {
            string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log("Received from server: " + receivedMessage);

            if (IsValidJson(receivedMessage))
            {
                MessageToSend deserializedData = JsonUtility.FromJson<MessageToSend>(receivedMessage);
                Chat.OnMessageReceived(deserializedData);
            }

            // After processing the received message, begin receiving again.
            BeginReceive();
        }
        else
        {
            Debug.LogError("Connection closed.");
        }
    }

    public override void SendChatMessage(string info)
    {
        if (client == null || !client.Connected)
        {
            Debug.LogError("Not connected to server");
            return;
        }

        Debug.Log("info   " + info);
        NetworkStream stream = client.GetStream();

        if (stream.CanWrite)
        {
            MessageToSend tst = JsonUtility.FromJson<MessageToSend>(info);

            Debug.Log(tst.message);

            byte[] jsonData = Encoding.ASCII.GetBytes(info);
            stream.Write(jsonData, 0, jsonData.Length);

            Debug.Log("Message sent!" + tst.message);

        }
        else
        {
            Debug.LogError("Cannot write to the stream.");
        }
    }

    void OnDestroy()
    {
        CloseConnection();
    }

    void CloseConnection()
    {
        if (client != null && client.Connected)
        {
            client.Close();
            client = null;
        }
    }
}
