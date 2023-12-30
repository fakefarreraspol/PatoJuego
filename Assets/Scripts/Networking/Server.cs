using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using UnityEngine;
using System.Linq;

public class Server : MonoBehaviour
{
    private UdpClient udpServer;
    private int port = 8080;
    private Dictionary<IPEndPoint, ClientInfo> connectedClients = new Dictionary<IPEndPoint, ClientInfo>();
    private Dictionary<IPEndPoint, DateTime> lastReceivedTime = new Dictionary<IPEndPoint, DateTime>();
    private Dictionary<IPEndPoint, bool> isDisconnected = new Dictionary<IPEndPoint, bool>();
    private TimeSpan timeoutThreshold = TimeSpan.FromSeconds(50); // Adjust as needed

    

    //public fakeDeserealizer fakeDeserealizer;    ///////////////////////////////////////////////////////


    private int nextClientId = 1;
    private Deserealizer deserealizer;
    private class ClientInfo
    {
        public int Id { get; set; }
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
        deserealizer = FindObjectOfType<Deserealizer>();
    }
    void Start()
    {
        if (FindObjectOfType<ServerData>().ServerPort != "") port = int.Parse(FindObjectOfType<ServerData>().ServerPort);
        InitializeServer();
        //fakeDeserealizer = FindObjectOfType<fakeDeserealizer>(); ///////////////////////////////////////////////////////
    }

    private void InitializeServer()
    {
        udpServer = new UdpClient(new IPEndPoint(IPAddress.Any, 8080));
        Debug.Log("Server is listening on port " + 8080);

        // Start receiving data asynchronously
        udpServer.BeginReceive(ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult result)
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            byte[] data = udpServer.EndReceive(result, ref remoteEndPoint);

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
                string message = Encoding.ASCII.GetString(data);

                Debug.Log("received: " + message);
                deserealizer.AddToDeserealizeQueue(message);        ///////////////////////////////////////////////////////

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
        connectedClients.Add(clientEndPoint, new ClientInfo { Id = clientId });
        lastReceivedTime.Add(clientEndPoint, DateTime.Now);
        isDisconnected.Add(clientEndPoint, false);

        MessageToSend welcomeMessage = new MessageToSend(clientId, "server", MessageType.CONNECTION, "This is your ID");
        string welcomeMessageToSend = JsonUtility.ToJson(welcomeMessage);
        SendServerMessage(welcomeMessageToSend, clientEndPoint);


        
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
        try
        {
            byte[] messageData = Encoding.ASCII.GetBytes(message);
            udpServer.Send(messageData, messageData.Length, IPpoint);
        }
        catch (Exception e)
        {
            Debug.LogError("Error broadcasting message to client " + IPpoint + ": " + e.Message);
        }
    }

    public void SendMessageToAllClients(string message)
    {
        Debug.Log("sending: " + message + " to all clients");
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
    public List<int> GetConnectedClientIPs()
    {
        List<int> clientIPs = new List<int>();

        foreach (var endPoint in connectedClients)
        {
            clientIPs.Add(endPoint.Value.Id);
        }

        return clientIPs;
    }
    public int GetConnectedClientsCount()
    {
        return connectedClients.Count;
    }
    void OnApplicationQuit()
    {
        if (udpServer != null)
        {
            udpServer.Close();
        }
    }
}
