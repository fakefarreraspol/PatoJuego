using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Text;
using System.Threading;

public class RoomManager : MonoBehaviour
{
    public int port = 1803;
    //Using TCP:
    TcpListener listener;
    private string serverName = "MyServerName";
    List<TcpClient> connectedClients = new List<TcpClient>();

    public void onButtonPressed()
    {
        StartServer();
    }
    private void StartServer()
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Debug.Log("Server started and waiting for clients...");
        listener.BeginAcceptTcpClient(OnClientConnected, null);
    }

    private void OnClientConnected(IAsyncResult ar)
    {
        TcpClient client = listener.EndAcceptTcpClient(ar);

        // Wait for a message from this client
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        stream.BeginRead(buffer, 0, buffer.Length, OnDataRead, new Tuple<NetworkStream, byte[]>(stream, buffer));
    }

    private void OnDataRead(IAsyncResult ar)
    {
        var state = (Tuple<NetworkStream, byte[]>)ar.AsyncState;
        NetworkStream stream = state.Item1;
        byte[] buffer = state.Item2;

        int bytesRead = stream.EndRead(ar);
        if (bytesRead > 0)
        {
            // We're ignoring the actual message from the client here and just sending the server's name
            byte[] responseData = Encoding.ASCII.GetBytes(serverName);
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
