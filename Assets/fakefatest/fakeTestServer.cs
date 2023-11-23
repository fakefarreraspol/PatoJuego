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
    private Dictionary<IPEndPoint, ClientInfo> connectedClients = new Dictionary<IPEndPoint, ClientInfo>();
    private Dictionary<IPEndPoint, DateTime> lastReceivedTime = new Dictionary<IPEndPoint, DateTime>();
    private Dictionary<IPEndPoint, bool> isDisconnected = new Dictionary<IPEndPoint, bool>();
    private TimeSpan timeoutThreshold = TimeSpan.FromSeconds(50); // Adjust as needed

    string serverIP = "25.63.64.104";

    fakeDeserealizer fakeDeserealizer;
    [SerializeField] private Character chRef;

    private int nextClientId = 1;

    private class ClientInfo
    {
        public int Id { get; set; }
    }
    void Start()
    {
        InitializeServer();
        chRef = FindObjectOfType<Character>();
        fakeDeserealizer = FindObjectOfType<fakeDeserealizer>();
    }

    private void InitializeServer()
    {
        udpServer = new UdpClient(new IPEndPoint(IPAddress.Any, port));
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
                //Debug.Log("Received from " + remoteEndPoint + " (" + GetClientName(remoteEndPoint) + "): " + message);
                fakeDeserealizer.Deserealize(message);
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

        int clientId = nextClientId++;
        connectedClients.Add(clientEndPoint, new ClientInfo { Id = clientId});
        lastReceivedTime.Add(clientEndPoint, DateTime.Now);
        isDisconnected.Add(clientEndPoint, false);

        string welcomeMessage = $"Welcome! Your ID is {clientId}";
        SendServerMessage(welcomeMessage, clientEndPoint);
    }

    private void HandleClientDisconnect(IPEndPoint clientEndPoint)
    {
        Debug.Log("Client (ID: " + connectedClients[clientEndPoint].Id + " has disconnected.");
        connectedClients.Remove(clientEndPoint);
        lastReceivedTime.Remove(clientEndPoint);
        isDisconnected[clientEndPoint] = true;
    }

    

    void Update()
    {
        // Check for client timeouts
        CheckClientTimeouts();

        //SendServerMessage("Hola guapo", GetEndPointById(1));

        

        

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


    protected void SendServerMessage(string message, IPEndPoint IPpoint)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);

        // Broadcast to all connected clients.
        //foreach (var client in connectedClients.Keys)
        //{


        //    try
        //    {
        //        byte[] messageData = Encoding.UTF8.GetBytes(message);
        //        udpServer.Send(messageData, messageData.Length, client);
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError("Error broadcasting message to client " + client + ": " + e.Message);
        //    }
        //}
        try
        {
            byte[] messageData = Encoding.UTF8.GetBytes(message);
            udpServer.Send(messageData, messageData.Length, IPpoint);
        }
        catch (Exception e)
        {
            Debug.LogError("Error broadcasting message to client " + IPpoint + ": " + e.Message);
        }
    }

    public void SendMessageToAllClients(string message)
    {
        foreach (var client in connectedClients.Keys)
        {
            SendServerMessage(message, client);
        }
    }
    private IPEndPoint GetEndPointById(int clientId)
    {
        foreach (var kvp in connectedClients)
        {
            if (kvp.Value.Id == clientId)
            {
                return kvp.Key;
            }
        }
        return null; // Client not found
    }

    void OnApplicationQuit()
    {
        if (udpServer != null)
        {
            udpServer.Close();
        }
    }
}
