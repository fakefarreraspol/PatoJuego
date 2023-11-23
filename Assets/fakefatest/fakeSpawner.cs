using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private GameObject remoteCharacter;


    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.F))
        {
            fakeTestServer srvr = FindObjectOfType<fakeTestServer>();
            SpawnPlayers(srvr.GetConnectedClientIPs()[0], srvr.GetConnectedClientsCount());
        }
    }
    public void SpawnPlayers(int pCount, int ip)
    {
        //Instantiate(mainCharacter, spawnPoints[0]);
        for (int i = 1; i <= pCount; i++)
        {
            GameObject newUser = Instantiate(remoteCharacter, spawnPoints[i].position, Quaternion.identity);
            FindObjectOfType<GameObjectManager>().AddGameObject(ip, newUser);
        }
    }

}
