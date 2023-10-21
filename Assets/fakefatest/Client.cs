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
