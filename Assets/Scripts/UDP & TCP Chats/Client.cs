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
    private void OnDisable()
    {
        OnSendMessage -= SendChatMessage;
    }

    //Function called by chat which is overrided by the TCP or UDP script
    public virtual void SendChatMessage(string message) { }

    //Function that checks if a string is a json and returns true or false
    protected bool IsValidJson(string jsonString)
    {
        try
        {
            JsonUtility.FromJsonOverwrite(jsonString, new object());
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }
}
