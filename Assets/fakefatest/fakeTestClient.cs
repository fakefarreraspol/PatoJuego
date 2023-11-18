using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class fakeTestClient : MonoBehaviour
{
    private UdpClient udpClient;
    private string serverIP = "127.0.0.1"; // Change this to the IP address of your server
    private int serverPort = 8080;

    void Start()
    {
        InitializeClient();
    }

    private void InitializeClient()
    {
        udpClient = new UdpClient();
    }

    void Update()
    {
        // Example: Sending the position of an object (transform.position) to the server
        Vector3 currentPosition = transform.position;
        string message = currentPosition.x + "," + currentPosition.y + "," + currentPosition.z;

        SendData(message);
    }

    private void SendData(string message)
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
