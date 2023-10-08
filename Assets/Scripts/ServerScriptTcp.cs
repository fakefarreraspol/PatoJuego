using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Collections.Generic;

public class ServerScriptTcp : MonoBehaviour
{

    TcpListener listener;
    List<TcpClient> clients = new List<TcpClient>();
    public int port = 1803;
    public string serverName = "MyServerName";

    void Start()
    {
        StartServer();
    }

    void StartServer()
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        listener.BeginAcceptTcpClient(AcceptCallback, null);
        Debug.Log("Server started and listening on port " + port);

    }

    void AcceptCallback(IAsyncResult AR)
    {
        TcpClient client = listener.EndAcceptTcpClient(AR);
        clients.Add(client);

        // Start reading messages from this client in a new thread or async task.

        // Respond with server name.
        byte[] buffer = Encoding.ASCII.GetBytes(serverName);
        client.GetStream().Write(buffer, 0, buffer.Length);

        // Continue listening for more clients:
        listener.BeginAcceptTcpClient(AcceptCallback, null);
        Debug.Log("Client connected from: " + client.Client.RemoteEndPoint); 

    }

    void OnApplicationQuit()
    {
        if (listener != null)
        {
            listener.Stop();
        }
    }
}