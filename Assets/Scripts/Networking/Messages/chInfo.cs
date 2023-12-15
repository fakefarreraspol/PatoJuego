using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class chInfo 
{
    public Vector3 playerTransform;
    public Vector2 playerDirection;

    public chActions characterActions;


    public chInfo(Vector3 pos, Vector2 dir, chActions actions) 
    { 
        playerTransform = pos;
        playerDirection = dir;
        characterActions = actions;
    }
}
