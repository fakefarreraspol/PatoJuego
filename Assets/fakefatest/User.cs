using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User 
{
    public string userName;
    public Sprite userAvatar;
    public Color usernameColor;
    
    public User()
    {
        int num = Random.Range(0,100);
        this.userName = "unknown"+ num;
        this.userAvatar = null;
        this.usernameColor = Random.ColorHSV();
    }

    public User(string userName)
    {
        this.userName = userName;
        this.userAvatar = null;
        this.usernameColor = Random.ColorHSV();
    }

    public User(string userName, Sprite userAvatar)
    {
        this.userName = userName;
        this.userAvatar = userAvatar;
        this.usernameColor = Random.ColorHSV();
    }

    public void ChangeUser(string userName, Sprite userAvatar)
    {
        this.userName = userName;
        this.userAvatar = userAvatar;

    }
}
