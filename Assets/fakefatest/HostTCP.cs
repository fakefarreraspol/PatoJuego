using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Collections.Generic;
using TMPro;
using System.IO;

public class HostTCP : Host
{ 
    Socket socketListener;
    List<Socket> clients = new List<Socket>();
    public int port = 1803;
    public string serverName = "Goosy Server";
    [SerializeField] private TMP_InputField portField;

    void Start()
    {
        StartServer();
        if (portField.text == string.Empty) port = 1803;
        else port = int.Parse(portField.text);
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
            Debug.Log("Host recieving callbacks");
            string receivedMessage = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);
            Debug.Log("Received from client: " + receivedMessage);

            byte[] data = new byte[1024];
            
            string jsonData = Encoding.ASCII.GetString(receiveBuffer, 0, bytesRead);

            Debug.Log("json:   "+ jsonData);

            // Process received message...
            MessageToSend deserializedData = JsonUtility.FromJson<MessageToSend>(receivedMessage);
            Chat.OnMessageReceived(deserializedData);

            // Send a response back to the client
            string responseMessage = "Message received!";
            byte[] responseData = Encoding.ASCII.GetBytes(responseMessage);
            client.Send(responseData);

            // Start listening again
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

    public override void SendServerMessage(string message)
    {
        foreach (Socket client in clients)
        {
            if (client != null && client.Connected)
            {
                byte[] messageData = Encoding.ASCII.GetBytes(message);
                client.Send(messageData);
            }
        }
    }
}
