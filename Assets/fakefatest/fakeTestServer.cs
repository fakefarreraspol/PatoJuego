using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System.Linq;

public class fakeTestServer : MonoBehaviour
{
    private UdpClient udpServer;
    private int port = 8080;
    private Dictionary<IPEndPoint, string> connectedClients = new Dictionary<IPEndPoint, string>();
    private Dictionary<IPEndPoint, DateTime> lastReceivedTime = new Dictionary<IPEndPoint, DateTime>();
    private Dictionary<IPEndPoint, bool> isDisconnected = new Dictionary<IPEndPoint, bool>();
    private TimeSpan timeoutThreshold = TimeSpan.FromSeconds(5); // Adjust as needed

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
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            byte[] data = udpServer.EndReceive(result, ref remoteEndPoint);

            // Check if data is null, which may indicate a disconnection
            if (data == null)
            {
                HandleClientDisconnect(remoteEndPoint);
                return;
            }

            // If the client is disconnected, ignore the received data
            if (isDisconnected.ContainsKey(remoteEndPoint) && isDisconnected[remoteEndPoint])
            {
                return;
            }

            // If the client is not in the connectedClients dictionary, it's a new client
            if (!connectedClients.ContainsKey(remoteEndPoint))
            {
                if (!isDisconnected.ContainsKey(remoteEndPoint) || !isDisconnected[remoteEndPoint])
                {
                    HandleNewClient(remoteEndPoint);
                }
            }

            if (data.Length > 0)
            {
                string message = Encoding.UTF8.GetString(data);
                Debug.Log("Received from " + remoteEndPoint + " (" + GetClientName(remoteEndPoint) + "): " + message);

                // Update last received time for the client
                lastReceivedTime[remoteEndPoint] = DateTime.Now;
            }

            // Continue listening for more data
            udpServer.BeginReceive(ReceiveCallback, null);
        }
        catch (ObjectDisposedException)
        {
            // The UdpClient is disposed, indicating that the server is closing
            Debug.Log("Server is shutting down.");
        }
        catch (Exception e)
        {
            if (e is SocketException && ((SocketException)e).SocketErrorCode == SocketError.ConnectionReset)
            {
                // Connection reset by the client, indicating disconnection
                HandleClientDisconnect(remoteEndPoint);
            }
            else
            {
                Debug.LogError("Error receiving data: " + e.Message);
            }
        }
    }

    private void HandleNewClient(IPEndPoint clientEndPoint)
    {
        Debug.Log("New client joined: " + clientEndPoint);
        // Add the new client to the dictionaries
        connectedClients.Add(clientEndPoint, "Client" + connectedClients.Count);
        lastReceivedTime.Add(clientEndPoint, DateTime.Now);
        isDisconnected.Add(clientEndPoint, false);
    }

    private void HandleClientDisconnect(IPEndPoint clientEndPoint)
    {
        Debug.Log("Client " + GetClientName(clientEndPoint) + " has disconnected.");
        connectedClients.Remove(clientEndPoint);
        lastReceivedTime.Remove(clientEndPoint);
        isDisconnected[clientEndPoint] = true;
    }

    private string GetClientName(IPEndPoint clientEndPoint)
    {
        if (connectedClients.ContainsKey(clientEndPoint))
        {
            return connectedClients[clientEndPoint];
        }
        else
        {
            return "Unknown Client";
        }
    }

    void Update()
    {
        // Check for client timeouts
        CheckClientTimeouts();

        SendServerMessage("sussy baka");
    }

    private void CheckClientTimeouts()
    {
        DateTime currentTime = DateTime.Now;

        foreach (var kvp in lastReceivedTime.ToList())
        {
            if (!isDisconnected[kvp.Key] && currentTime - kvp.Value > timeoutThreshold)
            {
                // The client has not sent data for a while, consider it disconnected
                HandleClientDisconnect(kvp.Key);
            }
        }
    }


    protected void SendServerMessage(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);

        // Broadcast to all connected clients.
        foreach (var client in connectedClients.Keys)
        {
            

            try
            {
                byte[] messageData = Encoding.UTF8.GetBytes(message);
                udpServer.Send(messageData, messageData.Length, client);
            }
            catch (Exception e)
            {
                Debug.LogError("Error broadcasting message to client " + client + ": " + e.Message);
            }
        }
    }



    void OnApplicationQuit()
    {
        if (udpServer != null)
        {
            udpServer.Close();
        }
    }
}