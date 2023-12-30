using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private TMP_InputField inputField;
    private UserManager ThisUser;
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
            if (incomingMessage.ID != ThisUser.userID)
            {
                
                DisplayMessage(incomingMessage);
            }
            Debug.Log("this is my own message");
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
        ThisUser = FindObjectOfType<UserManager>();
    }

    //Function called when the send button is clicked
    public void SendMessage()
    {
        MessageToSend msg = new MessageToSend(ThisUser.userID, ThisUser.Username, MessageType.CHAT_MESSAGE, inputField.text);

        DisplayMessage(msg);
        SendChatMessage(msg);

        //set the text field empty again
        inputField.text = string.Empty;
    }

    //Displays a message using a prefab and the MessageToSend class
    private void DisplayMessage(MessageToSend msg)
    {
        if(msg.Message == null || msg.Message == string.Empty) { return; }
        
       // Debug.Log("message printing: " + msg.message);
       // Debug.Log(msg.username);

        messagePrefab.transform.Find("MsgText").GetComponent<TextMeshProUGUI>().text = msg.Message;
        messagePrefab.transform.Find("MsgText").GetComponent<TextMeshProUGUI>().color = Color.black;

        messagePrefab.transform.Find("MsgUserName").GetComponent<TextMeshProUGUI>().text = msg.UserName;
        messagePrefab.transform.Find("MsgUserName").gameObject.GetComponent<TextMeshProUGUI>().color = Color.red;

        GameObject NewChatPrefabMessage = Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, content.transform);
        NewChatPrefabMessage.transform.SetAsFirstSibling();

    }

    private void SendChatMessage(MessageToSend msg)
    {
        string serializedData = JsonUtility.ToJson(msg);

        if (FindObjectOfType<Server>())
        {
            FindObjectOfType<Server>().SendMessageToAllClients(serializedData);
        }
        if (FindObjectOfType<Client>())
        {
            FindObjectOfType<Client>().SendData(serializedData);
        }
        //OnSendMessage(serializedData);
    }

}
