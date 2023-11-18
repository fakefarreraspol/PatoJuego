using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;
using System.Linq;

public class TestServer : MonoBehaviour
{
    private UdpClient udpServer;
    private int port = 8080;
    private Dictionary<IPEndPoint, string> connectedClients = new Dictionary<IPEndPoint, string>();
    private Dictionary<IPEndPoint, DateTime> lastReceivedTime = new Dictionary<IPEndPoint, DateTime>();
    private TimeSpan timeoutThreshold = TimeSpan.FromSeconds(5); // Adjust as needed

    private void Start()
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

            if (data.Length > 0)
            {
                // Deserialize the received data
                PlayerActionData playerActionData = SerializationManager.Instance.DeserializeObject<PlayerActionData>(data);

                // Schedule the processing on the main thread
                UnityMainThreadDispatcher.Instance().Enqueue(() => UpdateGameState(playerActionData));
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

    private void UpdateGameState(PlayerActionData playerActionData)
    {
        // Example: Update the game state based on player action data
        // Update player position, handle other actions, etc.
    }

    private void HandleClientDisconnect(IPEndPoint clientEndPoint)
    {
        Debug.Log("Client " + GetClientName(clientEndPoint) + " has disconnected.");
        connectedClients.Remove(clientEndPoint);
        lastReceivedTime.Remove(clientEndPoint);
    }

    private string GetClientName(IPEndPoint clientEndPoint)
    {
        if (connectedClients.ContainsKey(clientEndPoint))
        {
            return connectedClients[clientEndPoint];
        }
        else
        {
            string clientName = "Client" + connectedClients.Count;
            connectedClients.Add(clientEndPoint, clientName);
            lastReceivedTime.Add(clientEndPoint, DateTime.Now);
            return clientName;
        }
    }

    void Update()
    {
        // Check for client timeouts
        CheckClientTimeouts();
    }

    private void CheckClientTimeouts()
    {
        DateTime currentTime = DateTime.Now;

        foreach (var kvp in lastReceivedTime.ToList())
        {
            if (currentTime - kvp.Value > timeoutThreshold)
            {
                // The client has not sent data for a while, consider it disconnected
                HandleClientDisconnect(kvp.Key);
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