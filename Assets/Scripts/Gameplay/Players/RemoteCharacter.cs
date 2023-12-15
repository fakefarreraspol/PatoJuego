using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    public void UpdateRemoteCharacterPos(chInfo playerData)
    {
        transform.position = playerData.playerTransform;
    }
}
