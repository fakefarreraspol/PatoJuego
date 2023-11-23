using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class fakeTestClient : MonoBehaviour
{
    private UdpClient udpClient;
    private string serverIP = "127.0.0.1"; // Change this to the IP address of your server
    private int serverPort = 8080;

    fakeDeserealizer fakeDeserealizer;
    
    [SerializeField] private Character chRef;

    private void Awake()
    {
        chRef = FindObjectOfType<Character>();
    }
    void Start()
    {
        fakeDeserealizer = FindObjectOfType<fakeDeserealizer>();
        InitializeClient();
    }

    void BeginReceive()
    {
        if (udpClient != null)
        {
            udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        }
    }
    void ReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, serverPort);
        byte[] data = udpClient.EndReceive(ar, ref serverEndPoint);
        string message = Encoding.ASCII.GetString(data);

        Debug.Log("Received response from Goozy server: " + message);
        //Only deserialize if the message is a json
        fakeDeserealizer.Deserealize(message);

        BeginReceive();

    }

    private void InitializeClient()
    {
        udpClient = new UdpClient();
        udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
    }


    public void SendData(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, serverIP, serverPort);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending data: " + e.Message);
        }
    }

    void OnApplicationQuit()
    {
        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
