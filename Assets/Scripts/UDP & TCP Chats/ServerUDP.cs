using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using TMPro;

public class ServerUDP : Host
{
    private UdpClient udpServer;
    private int port = 1803;
    private List<IPEndPoint> clients = new List<IPEndPoint>();

    [SerializeField] private TMP_InputField portField;

    void Start()
    {
        if (portField.text == string.Empty) port = 1803;
        else port = int.Parse(portField.text);

        StartServer();
    }

    void StartServer()
    {
        udpServer = new UdpClient(port);
        BeginReceive();
        //Debug.Log("Goozy server started and waiting for client...");
    }

    void BeginReceive()
    {
        udpServer.BeginReceive(ReceiveCallback, null);
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, port);
        byte[] data = udpServer.EndReceive(ar, ref clientEndPoint);
        string nameRecieved = Encoding.ASCII.GetString(data);

        if (!clients.Contains(clientEndPoint))
        {
            clients.Add(clientEndPoint);
        }

        string message = "Welcome to Goozy server!";
        //Debug.Log("Goozy server received message: " + nameRecieved);

        // Send a response back to the client
        byte[] response = Encoding.ASCII.GetBytes(message);
        udpServer.Send(response, response.Length, clientEndPoint);

        string jsonData = Encoding.ASCII.GetString(data);

       //Debug.Log("json:   " + jsonData);

        // Send the received message to all the clients
        SendServerMessage(jsonData);
        // Process received message...
        DisplayOnChat(jsonData);

        // Continue listening for messages
        udpServer.BeginReceive(new AsyncCallback(ReceiveCallback), null);
    }

    protected override void SendServerMessage(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);

        // Broadcast to all connected clients.
        foreach (var client in clients)
        {
            udpServer.Send(data, data.Length, client);
        }
    }

    void OnDestroy()
    {
        if (udpServer != null)
        {
            udpServer.Close();
            udpServer = null;
        }
    }
}