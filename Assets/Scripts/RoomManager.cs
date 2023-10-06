using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Text;
using System.Threading;

public class RoomManager : MonoBehaviour
{
    public int port = 1803;
    //Using TCP:
    TcpListener listener;
    List<TcpClient> connectedClients = new List<TcpClient>();

    public void StartServer()
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        BeginAcceptClients();
    }

    void BeginAcceptClients()
    {
        listener.BeginAcceptTcpClient(OnClientConnected, null);
        Debug.Log("client connected");
    }

    void OnClientConnected(IAsyncResult ar)
    {
        TcpClient client = listener.EndAcceptTcpClient(ar);
        connectedClients.Add(client);

        // Read the client's message 
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024]; // buffer size of 1024 as an example
        stream.BeginRead(buffer, 0, buffer.Length, OnDataRead, new Tuple<NetworkStream, byte[]>(stream, buffer));
    }

    void OnDataRead(IAsyncResult ar)
    {
        var state = (Tuple<NetworkStream, byte[]>)ar.AsyncState;
        NetworkStream stream = state.Item1;
        byte[] buffer = state.Item2;

        int bytesRead = stream.EndRead(ar);
        if (bytesRead > 0)
        {
            string clientMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            // Handle the client's message as needed, for example, print it
            Debug.Log("Client Message: " + clientMessage);

            // Respond to client
            string serverResponse = "Hello from server!";
            byte[] responseData = Encoding.ASCII.GetBytes(serverResponse);
            stream.Write(responseData, 0, responseData.Length);
        }
    }


    //Using UDP:

    //UdpClient udpServer = new UdpClient(port);

    //void StartServer()
    //{
    //    BeginReceive();
    //}

    //void BeginReceive()
    //{
    //    udpServer.BeginReceive(OnDataReceived, null);
    //}

    //void OnDataReceived(IAsyncResult ar)
    //{
    //    IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, port);
    //    byte[] data = udpServer.EndReceive(ar, ref clientEndPoint);
    //    // Handle data and send a response
    //}

}
