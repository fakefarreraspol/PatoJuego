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
    // Start is called before the first frame update
    private void SendServerMessage(string msg)
    {
        Debug.Log("Server Message Sent!!!!!!");
    }
}
