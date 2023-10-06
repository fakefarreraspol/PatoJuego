using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    private TcpClient client;
    void OnConnected(IAsyncResult ar)
    {
        client.EndConnect(ar);

        // Send an initial message or just prepare to read the server's response
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        stream.BeginRead(buffer, 0, buffer.Length, OnServerResponse, new Tuple<NetworkStream, byte[]>(stream, buffer));
    }

    void OnServerResponse(IAsyncResult ar)
    {
        var state = (Tuple<NetworkStream, byte[]>)ar.AsyncState;
        NetworkStream stream = state.Item1;
        byte[] buffer = state.Item2;

        int bytesRead = stream.EndRead(ar);
        if (bytesRead > 0)
        {
            string serverMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Debug.Log("Received from server: " + serverMessage);  // This logs the server's message to Unity's console.
        }
    }
}
