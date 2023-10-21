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
        InvokeRepeating(nameof(Messaging), 2, 1);
        notificationSource = GetComponent<AudioSource>();
    }

    public void Messaging()
    {
        if (messageQueue.Count > 0) 
        {            
            DisplayMessage(messageQueue.Dequeue());
            notificationSource.Play();
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
    public void EnqueueMessage(MessageToSend message)
    {
        messageQueue.Enqueue(message);
    }

    private void Awake()
    {
        userr = FindObjectOfType<UserInfo>().user;
    }

    public void SendMessage()
    {
        if(userr == null) { userr = new User(); }
        MessageToSend msg = new MessageToSend(userr, inputField.text);
        //SendMessageTCP(msg);
        DisplayMessage(msg);
        
        if(GameObject.Find("Server") != null) SendServerMessage(msg);
        else SendChatMessage(msg);

        inputField.text = string.Empty;
    }


    private void DisplayMessage(MessageToSend msg)
    {
        if(msg.message == string.Empty) { return; }
        
        Debug.Log("message printing: "+
            msg.message);

        Debug.Log(msg.username);

        //messagePrefab.GetComponentInChildren<TextMeshProUGUI>().text = msg;
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
