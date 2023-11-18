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

        // Instantiate PlayerActionData with the currentPosition
        PlayerActionData playerActionData = new PlayerActionData(currentPosition);

        // Serialize PlayerActionData to a byte array
        byte[] serializedData = SerializationManager.Instance.SerializeObject(playerActionData);

        // Send the serialized data
        SendData(serializedData);
    }

    private void SendData(byte[] data)
    {
        try
        {
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