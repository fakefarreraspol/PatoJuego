using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private GameObject content;
    [SerializeField] private TMP_InputField inputField;
    private User userr;

    public static Action<string, string, Image, Color> OnMessageSend;

    private void Awake()
    {
        userr = FindObjectOfType<manager>().user;
    }

    public void SendMessage()
    {
        DisplayMessage(inputField.text);
        inputField.text = string.Empty;
    }


    private void DisplayMessage(string msg)
    {
        if(msg == string.Empty) { return; }
        

        //messagePrefab.GetComponentInChildren<TextMeshProUGUI>().text = msg;
        messagePrefab.transform.Find("MsgText").GetComponent<TextMeshProUGUI>().text = msg;
        messagePrefab.transform.Find("MsgText").GetComponent<TextMeshProUGUI>().color = Color.black;

        messagePrefab.transform.Find("MsgUserName").GetComponent<TextMeshProUGUI>().text = userr.userName;
        messagePrefab.transform.Find("MsgUserName").gameObject.GetComponent<TextMeshProUGUI>().color = userr.usernameColor;
        Instantiate(messagePrefab, Vector3.zero, Quaternion.identity, content.transform);
    }    
}
