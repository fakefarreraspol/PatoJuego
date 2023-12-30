using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        CreateUser,
        Intro,
        Lobby,
        Gameplay
    }

    //public GameState gState = GameState.CreateUser;
    public GameState gState = GameState.Gameplay;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(gState == GameState.Gameplay && Input.GetKeyDown(KeyCode.M))
        {

            List<int> connectedClients = FindObjectOfType<Server>().GetConnectedClientIPs();
            for(int i = 0; i < connectedClients.Count; i++)
            {
                FindObjectOfType<Spawner>().SpawnRemoteCharacter(i,connectedClients[i]);
            }
        }
    }
}
