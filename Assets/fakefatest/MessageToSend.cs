using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageToSend 
{
    public string username;
    public string message;
    public Color color;

    public MessageToSend(User user, string message)
    {
        this.username = user.userName;
        this.message = message;
        this.color = user.usernameColor;
    }

    
}
