using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakePlayerData 
{
    public int userId;
    public Vector3 playerTransform;
    public Vector2 playerDirection;
   
    public bool bulletShot = false;


    public fakePlayerData() 
    {
    }
    public fakePlayerData(Vector3 pTrans, Vector2 pDir, bool isbShot)
    {
        playerTransform = pTrans;
        playerDirection = pDir;
        bulletShot = isbShot;

    }

}
