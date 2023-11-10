using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using TMPro;
using UnityEngine;

public class TestServer : MonoBehaviour
{
    private UdpClient udpServer;
    private int port = 1803;
    private List<IPEndPoint> clients = new List<IPEndPoint>();

    //[SerializeField] private TMP_InputField portField;

    void Start()
    {
        //if (portField.text == string.Empty) port = 1803;
        //else port = int.Parse(portField.text);

        StartServer();
    }

    void StartServer()
    {
        udpServer = new UdpClient(port);
        BeginReceive();
        //Debug.Log("Goozy server started and waiting for client...");
    }

    void BeginReceive()
    {
        udpServer.BeginReceive(ReceiveCallback, null);
    }

    void ReceiveCallback(IAsyncResult ar)
    {
        IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, port);
        byte[] data = udpServer.EndReceive(ar, ref clientEndPoint);
        string nameRecieved = Encoding.ASCII.GetString(data);

        if (!clients.Contains(clientEndPoint))
        {
            clients.Add(clientEndPoint);
        }

        Debug.Log(nameRecieved);


        Transform pibeMalo = GameObject.FindGameObjectWithTag("pibemalo").GetComponent<Transform>();
        pibeMalo.position = new Vector3(float.Parse(nameRecieved), 0, 0);
        BeginReceive();
        // Continue listening for messages
        udpServer.BeginReceive(new AsyncCallback(ReceiveCallback), null);
    }

    

    void OnDestroy()
    {
        if (udpServer != null)
        {
            udpServer.Close();
            udpServer = null;
        }
    }
}
