using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        CreateUser,
        Intro,
        Lobby,
        Spawn,
        Gameplay
    }

    //public GameState gState = GameState.CreateUser;
    public GameState gState = GameState.CreateUser;
    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GameManager[] checkManagers = FindObjectsOfType<GameManager>();

        if (checkManagers.Length > 1) Destroy(gameObject);
    }

    private void Start()
    {
        gState = GameState.CreateUser;
    }

    // Update is called once per frame
    private void Update()
    {
        if(FindObjectOfType<Server>())
        {
            if (gState == GameState.Spawn)
            {

                MessageToSend msg = new MessageToSend(0, "SigmaMale", MessageType.GENERATE_PLAYERS, "0");
                string msgToClient = JsonUtility.ToJson(msg);
                FindObjectOfType<Server>().SendMessageToAllClients(msgToClient);

                FindObjectOfType<Spawner>().SpawnControllableCharacter(0);

                List<int> connectedClients = FindObjectOfType<Server>().GetConnectedClientIPs();
                for (int i = 0; i < connectedClients.Count; i++)
                {
                    FindObjectOfType<Spawner>().SpawnRemoteCharacter(i + 1, connectedClients[i]);

                    MessageToSend newMsg = new MessageToSend(connectedClients[i], "SigmaMale", MessageType.GENERATE_PLAYERS, (i + 1).ToString());
                    string newmsgToClient = JsonUtility.ToJson(newMsg);
                    FindObjectOfType<Server>().SendMessageToAllClients(newmsgToClient);
                }

                gState = GameState.Gameplay;



                
            }
        }
        

        if (gState == GameState.Gameplay && Input.GetKeyDown(KeyCode.M))
        {
            MessageToSend othermsg = new MessageToSend(0, "LladosFitness", MessageType.GAME_END);
            string othermsgToClient = JsonUtility.ToJson(othermsg);
            FindObjectOfType<Server>().SendMessageToAllClients(othermsgToClient);

            gState = GameState.Lobby;

            SceneManager.LoadScene("Lobby");
        }

    }


    public void ChangeState(GameState state)
    {
        gState = state;
    }
}
