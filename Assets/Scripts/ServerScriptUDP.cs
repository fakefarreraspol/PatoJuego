using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;

public class ServerScriptUDP : MonoBehaviour
{
    private UdpClient udpServer;
    private int port = 1803;

    void Start()
    {
        udpServer = new UdpClient(port);
        udpServer.BeginReceive(new AsyncCallback(ReceiveCallback), null);
        Debug.Log("Goozy server started, waiting for client...");
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, port);
        byte[] data = udpServer.EndReceive(ar, ref clientEndPoint);
        string nameRecieved = Encoding.ASCII.GetString(data);
        string message = "Welcome to Goozy server!";
        Debug.Log("Goozy server received message: " + nameRecieved);

        // Send a response back to the client
        byte[] response = Encoding.ASCII.GetBytes(message);
        udpServer.Send(response, response.Length, clientEndPoint);

        // Continue listening for messages
        udpServer.BeginReceive(new AsyncCallback(ReceiveCallback), null);
    }

    void OnDestroy()
    {
        udpServer.Close();
    }
}