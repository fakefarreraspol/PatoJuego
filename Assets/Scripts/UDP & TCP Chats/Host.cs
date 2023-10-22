using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Host : MonoBehaviour
{
    public static Action<string> OnSendServerMessage;
    private void OnEnable()
    {
        OnSendServerMessage += SendServerMessage;
    }
    private void OnDisable()
    {
        OnSendServerMessage -= SendServerMessage;
    }

    //Function called by chat which is overrided by the TCP or UDP script
    protected virtual void SendServerMessage(string msg)
    {
    }

    protected void DisplayOnChat(string text)
    {
        //Deserialize json
        MessageToSend deserializedData = JsonUtility.FromJson<MessageToSend>(text);
        //Call the chat displaying function
        Chat.OnMessageReceived(deserializedData);
    }
}
