using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private TMP_InputField inputField;
    private User userr;
    private AudioSource notificationSource;

    public static Action<string, string, Image, Color> OnMessageSend;
    public static Action<MessageToSend> OnMessageReceived;


    private Queue<MessageToSend> messageQueue = new Queue<MessageToSend>();


    private void Start()
    {
        InvokeRepeating(nameof(Messaging), 2, 0.2f);
        notificationSource = GetComponent<AudioSource>();
    }

    //Check if there are messages inside de queue and display them onscreen
    public void Messaging()
    {
        if (messageQueue.Count > 0)
        {
            MessageToSend incomingMessage = messageQueue.Dequeue();
            if (incomingMessage.username != userr.userName)
            {
                notificationSource.Play();
                DisplayMessage(incomingMessage);
            }
        }
    }

    private void OnEnable()
    {
        OnMessageReceived += EnqueueMessage;
    }
    private void OnDisable()
    {
        OnMessageReceived -= EnqueueMessage;
    }

    //Take a received message and enqueue it
    public void EnqueueMessage(MessageToSend message)
    {
        messageQueue.Enqueue(message);
    }

    private void Awake()
    {
        userr = FindObjectOfType<UserInfo>().user;
    }

    //Function called when the send button is clicked
    public void SendMessage()
    {
        //Check if the user is null and create a new one if it is
        if(userr == null) { userr = new User(); }
        //Create a message with the text the user introduced in the text field
        MessageToSend msg = new MessageToSend(userr, inputField.text);

        DisplayMessage(msg);
        //Call different sending function depending if the user is a host or a client
        if (GameObject.Find("Server") != null)
        {
            SendServerMessage(msg);
        }
        else SendChatMessage(msg);

        //set the text field empty again
        inputField.text = string.Empty;
    }

    //Displays a message using a prefab and the MessageToSend class
    private void DisplayMessage(MessageToSend msg)
    {
        if(msg.message == string.Empty) { return; }
        
       // Debug.Log("message printing: " + msg.message);
       // Debug.Log(msg.username);

        messagePrefab.transform.Find("MsgText").GetComponent<TextMeshProUGUI>().text = msg.message;
        messagePrefab.transform.Find("MsgText").GetComponent<TextMeshProUGUI>().color = Color.black;

        messagePrefab.transform.Find("MsgUserName").GetComponent<TextMeshProUGUI>().text = msg.username;
        messagePrefab.transform.Find("MsgUserName").gameObject.GetComponent<TextMeshProUGUI>().color = msg.color;
        Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, content.transform);
    }

    private void SendChatMessage(MessageToSend msg)
    {
        string serializedData = JsonUtility.ToJson(msg);
        Client.OnSendMessage(serializedData);
    }

    private void SendServerMessage(MessageToSend msg)
    {
        string serializedData = JsonUtility.ToJson(msg);
        Host.OnSendServerMessage(serializedData);
    }
}
