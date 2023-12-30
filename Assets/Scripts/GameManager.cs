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
            MessageToSend msg = new MessageToSend(0, "SigmaMale", MessageType.GENERATE_PLAYERS, "0");
            string msgToClient = JsonUtility.ToJson(msg);
            FindObjectOfType<Server>().SendMessageToAllClients(msgToClient);
            List<int> connectedClients = FindObjectOfType<Server>().GetConnectedClientIPs();
            for(int i = 0; i < connectedClients.Count; i++)
            {
                FindObjectOfType<Spawner>().SpawnRemoteCharacter(i+1,connectedClients[i]);
            }
        }
    }
}
