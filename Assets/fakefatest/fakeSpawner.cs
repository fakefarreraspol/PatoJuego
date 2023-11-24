using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject mainCharacter;
    [SerializeField] private GameObject remoteCharacter;

    public static Action<int> onNewUser;
    Queue<int> newUsers = new Queue<int>();
    private void OnEnable()
    {
        onNewUser += PlsQueueSpawnPlayer;
    }
    private void OnDisable()
    {
        onNewUser -= PlsQueueSpawnPlayer;
    }
    public void Start()
    {
        //Debug.Log(FindObjectOfType<fakeTestServer>().GetConnectedClientIPs().Count);
        //if (FindObjectOfType<fakeGameManager>().gState == fakeGameManager.GameState.InGame)
        //{
        //    fakeTestServer srvr = FindObjectOfType<fakeTestServer>();
        //    for(int i = 0; i < srvr.GetConnectedClientIPs().Count;  i++)
        //    {
        //        if(srvr.GetConnectedClientIPs()[i] != FindObjectOfType<fakefaID>().IDDDDDDDD) PlsSpawnPlayer(srvr.GetConnectedClientIPs()[i]);
        //    }
        //}
    }
    public void Update()
    {
        
        //if (newUsers.Count > 0)
        //{
        //    int ipNewUser = newUsers.Dequeue();
        //    PlsSpawnPlayer(ipNewUser);
        //}
    }
    public void SpawnPlayers(List<int> Listip)
    {
        //Instantiate(mainCharacter, spawnPoints[0]);
        for (int i = 0; i < Listip.Count; i++)
        {
            if (i != FindObjectOfType<fakefaID>().IDDDDDDDD)
            {
                Debug.Log(Listip[i]);
                
                GameObject newUser = Instantiate(remoteCharacter, spawnPoints[i].position, Quaternion.identity);
                FindObjectOfType<GameObjectManager>().AddGameObject(Listip[i], newUser);
            }
        }
    }
    public void PlsSpawnPlayer(int id)
    {
        if (!FindObjectOfType<GameObjectManager>().CheckIfItsAlreadyListed(id))
        {
            if (id != FindObjectOfType<fakefaID>().IDDDDDDDD)
            {

                if (spawnPoints is { Length: > 0 })
                {
                    GameObject newuser = Instantiate(remoteCharacter, spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
                    FindObjectOfType<GameObjectManager>().AddGameObject(id, newuser);
                }
                else
                {
                    GameObject newuser = Instantiate(remoteCharacter, Vector2.zero, Quaternion.identity);
                    FindObjectOfType<GameObjectManager>().AddGameObject(id, newuser);
                }
            }
        }
        
    }
    public void PlsQueueSpawnPlayer(int id)
    {
        newUsers.Enqueue(id);
    }
    
}
