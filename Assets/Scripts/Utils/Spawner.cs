using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawners;

    public GameObject characterRemotePrefab;
    public GameObject characterLocalPrefab;


    public void SpawnRemoteCharacter(int spawnPos, int ID)
    {
        if (spawnPos >= 4) spawnPos = 3;
        if(!FindObjectOfType<ObjectManager>().GetGameObject(ID))
        {
            GameObject NewRemoteCharacter = Instantiate(characterRemotePrefab, spawners[spawnPos].position, Quaternion.identity);
            FindObjectOfType<ObjectManager>().AddGameObject(ID, NewRemoteCharacter);
        }
        
    }
    public void SpawnControllableCharacter(int spawnPos)
    { 
        Instantiate(characterLocalPrefab, spawners[spawnPos].position, Quaternion.identity);
        
    }
}
