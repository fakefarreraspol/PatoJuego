using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakePlayerData 
{
    public int userId;
    public Vector3 playerTransform;
    public Vector2 playerDirection;
    public int Health;
    public bool bulletShot = false;

    public bool newUser = false;
    public fakePlayerData() 
    {
    }
    public fakePlayerData(Vector3 pTrans, Vector2 pDir, bool isbShot, int health, int id, bool newUser = false)
    {
        playerTransform = pTrans;
        playerDirection = pDir;
        bulletShot = isbShot;
        Health = health;
        userId = id;
        this.newUser = newUser;
    }

}
