using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Collections.Generic;

public class ServerScriptTcp : MonoBehaviour
{

    //TcpListener listener;
    //List<TcpClient> clients = new List<TcpClient>();
    //public int port = 1803;
    //public string serverName = "Goosy Server";

    //void Start()
    //{
    //    StartServer();
    //}

    //void StartServer()
    //{
    //    listener = new TcpListener(IPAddress.Any, port);
    //    listener.Start();
    //    while (true)
    //    {
    //        TcpClient client = await listener.AcceptTcpClientAsync();
    //        // Handle client here or spawn a new task to handle the client
    //    }

    //    Debug.Log("Goozy server started and listening on port " + port);
    //}

    //void AcceptCallback(IAsyncResult AR)
    //{
    //    TcpClient client = listener.EndAcceptTcpClient(AR);
    //    clients.Add(client);

    //    // Start reading messages from this client in a new thread or async task.

    //    // Respond with server name.
    //    byte[] buffer = Encoding.ASCII.GetBytes(serverName);
    //    client.GetStream().Write(buffer, 0, buffer.Length);

    //    // Continue listening for more clients:
    //    listener.BeginAcceptTcpClient(AcceptCallback, null);
    //    Debug.Log("Client connected from: " + client.Client.RemoteEndPoint); 

    //}

    //void OnApplicationQuit()
    //{
    //    if (listener != null)
    //    {
    //        listener.Stop();
    //    }
    //}

    Socket socketListener;
    List<Socket> clients = new List<Socket>();
    public int port = 1803;
    public string serverName = "Goosy Server";

    void Start()
    {
        StartServer();
    }

    void StartServer()
    {
        // Create an IPV4 TCP socket
        socketListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint (all IP addresses and the specified port)
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);
        socketListener.Bind(localEndPoint);

        // Start listening with a backlog of pending connections
        socketListener.Listen(10);

        Debug.Log("Goozy server started and listening on port " + port);

        BeginAccept();
    }

    void BeginAccept()
    {
        socketListener.BeginAccept(AcceptCallback, null);
    }

    void AcceptCallback(IAsyncResult AR)
    {
        Socket client = socketListener.EndAccept(AR);
        clients.Add(client);

        // Start reading from the client socket
        BeginReceive(client);

        // Respond with server name.
        byte[] buffer = Encoding.ASCII.GetBytes(serverName);
        client.Send(buffer);

        Debug.Log("Client connected from: " + client.RemoteEndPoint);

        // Continue listening for more clients
        BeginAccept();
    }

    byte[] receiveBuffer = new byte[1024];

    void BeginReceive(Socket client)
    {
        client.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallback, client);
    }

    void ReceiveCallback(IAsyncResult AR)
    {
        Socket client = (Socket)AR.AsyncState;
        int bytesRead = client.EndReceive(AR);

        if (bytesRead > 0)
        {
            string receivedData = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);
            Debug.Log("Received from client: " + receivedData);

            // If you want to keep reading data continuously, start another asynchronous read operation
            BeginReceive(client);
        }
    }

    void OnApplicationQuit()
    {
        foreach (var client in clients)
        {
            if (client != null && client.Connected)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }

        if (socketListener != null)
        {
            socketListener.Close();
        }
    }
}