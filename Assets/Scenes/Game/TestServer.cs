using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;

public class TestServer : MonoBehaviour
{
    private UdpClient udpServer;
    private int port = 8080;


    private Vector3 rValue;
    [SerializeField] private GameObject malomalisimo;

    void Start()
    {
        InitializeServer();
    }

    private void InitializeServer()
    {
        udpServer = new UdpClient(port);
        Debug.Log("Server is listening on port " + port);

        // Start receiving data asynchronously
        udpServer.BeginReceive(ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        try
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpServer.EndReceive(result, ref remoteEndPoint);

            if (data.Length > 0)
            {
                string message = Encoding.UTF8.GetString(data);
                Debug.Log("Received from " + remoteEndPoint + ": " + message);

                float w= float.Parse(message);
                rValue = new Vector3(w, 0, 0);

                Debug.Log("rrr, " + rValue.ToString());
            }

            // Continue listening for more data
            udpServer.BeginReceive(ReceiveCallback, null);
        }
        catch (Exception e)
        {
            Debug.LogError("Error receiving data: " + e.Message);
        }
    }

    private void FixedUpdate()
    {

        malomalisimo.transform.position = new Vector3(rValue.x, malomalisimo.transform.position.y, malomalisimo.transform.position.z);
    }

    void OnApplicationQuit()
    {
        if (udpServer != null)
        {
            udpServer.Close();
        }
    }
}
