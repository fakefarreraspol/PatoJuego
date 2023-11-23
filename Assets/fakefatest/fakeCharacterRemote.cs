using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeCharacterRemote : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject bullet;
    public void UpdateRemoteCharacterPos(fakePlayerData playerData)
    {
        transform.position = playerData.playerTransform;
        if (playerData.bulletShot)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
}
