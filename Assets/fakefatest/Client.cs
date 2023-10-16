using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Action<string> OnSendMessage;
    private void OnEnable()
    {
        OnSendMessage += SendChatMessage;
    }
    private void DisEnable()
    {
        OnSendMessage -= SendChatMessage;
    }


    public virtual void SendChatMessage(string message) { }
}
