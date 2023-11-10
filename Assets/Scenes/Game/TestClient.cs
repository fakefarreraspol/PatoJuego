using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TestClient : MonoBehaviour
{
    //public TMP_InputField nameInputField;
    //public TMP_InputField ipInputField;
    //public Button joinGameButton;

    private UdpClient udpClient;
    private int port = 1803;

    //private User user;

    void Start()
    {
        udpClient = new UdpClient();
        //user = FindObjectOfType<UserInfo>().user;
        
        //Go to chat scene
        //Intro.OnServerFinishedLoading();
    }
    void BeginReceive()
    {
        if (udpClient != null)
        {
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
    }

    private void Update()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SendDataToServer(player.position.x.ToString());
        Debug.Log("Data Sent! " + player.position.x.ToString());
    }
    public void SendDataToServer(string message)
    {
        try
        {
            string ip = "192.168.2.23";
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
        //Only deserialize if the message is a json

        

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
