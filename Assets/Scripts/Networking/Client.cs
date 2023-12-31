using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{
    private UdpClient udpClient;
    private string serverIP = "25.63.64.104";
    //private string serverIP = "192.168.104.18";
    //private string serverIP = "192.168.204.18";
    private int serverPort = 8080;

    Deserealizer Deserealizer;            /////////////////////////////////////////

    private void Awake()
    {
        Client[] checkClient = FindObjectsOfType<Client>();

        if (checkClient.Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        if (FindObjectOfType<ServerData>().ServerIP != "") serverIP = FindObjectOfType<ServerData>().ServerIP;
        Deserealizer = FindObjectOfType<Deserealizer>(); ///////////////////////////////////
        ConnectToServer();
        InitializeClient();

    }

    private void ConnectToServer()
    {
        //serverIP = FindObjectOfType<fakeDatos>().ip;      /////////////////////////////////
        if (serverIP == null)
        {
            Debug.Log("IP is null ");
        }
        else
        {
            Debug.Log("IP is: " + serverIP);
        }
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
        Deserealizer.AddToDeserealizeQueue(message);    /////////////////////////////////////////

        BeginReceive();

    }

    private void InitializeClient()
    {
        udpClient = new UdpClient();
        udpClient.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        SendData("0");
    }


    public void SendData(string message)
    {
        try
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            if (serverIP != null)
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
