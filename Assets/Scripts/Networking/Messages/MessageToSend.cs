using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageToSend 
{
    public int ID;
    public string UserName;
    public string Message = null;

    public chInfo UserCharacterInfo = null;
    public MessageType messageType;

    public MessageToSend(int iD, string userName, MessageType messageType, string message = null, chInfo userCharacterInfo = null)
    {
        ID = iD;
        UserName = userName;
        Message = message;
        UserCharacterInfo = userCharacterInfo;
        this.messageType = messageType;
    }
}
