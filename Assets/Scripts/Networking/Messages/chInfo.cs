using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class chInfo 
{
    public Vector3 playerTransform;
    public Vector2 playerDirection;

    public chActions characterActions;

    public int healthPoints;
    public chInfo(Vector3 pos, Vector2 dir, int HP, chActions actions) 
    { 
        playerTransform = pos;
        playerDirection = dir;
        characterActions = actions;
        healthPoints = HP;
    }
}
