using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Deserealizer : MonoBehaviour
{
    Queue<string> ReceivedMessages = new Queue<string>();

    //Necessari objects to update
    private ObjectManager remotePlayersManager;
    private RemoteCharacter character;
    private UserManager myUser;

    private void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        character = FindObjectOfType<RemoteCharacter>();
        myUser = FindObjectOfType<UserManager>();
    }
    private void Deserealize(string Message)
    {
        Debug.Log("Deserealizing message:    " +Message);
        if(!IsValidJson(Message)) 
        {
            return;
        }
        else
        {
            MessageToSend msg = JsonUtility.FromJson<MessageToSend>(Message);
            Debug.Log(msg.UserName);
            switch (msg.messageType)
            {
                case MessageType.CONNECTION:
                    HandleConnectionMessage(msg);
                    break;
                case MessageType.CHECK_CONNECTION:
                    HandleConnectionCheck(msg);
                    break;
                case MessageType.WAITING_ROOM:
                    HandleWaitingRoomMessage(msg);
                    break;
                case MessageType.CHAT_MESSAGE:
                    HandleChatMessage(msg);
                    break;
                case MessageType.GAME_START:
                    HandleGameStart(msg);
                    break;
                case MessageType.CHARACTER_INFO:
                    HandleCharacterInfo(msg);
                    break;
                case MessageType.GENERATE_PLAYERS:
                    HandleGeneratePlayers(msg);
                    break;
                case MessageType.GAME_END:
                    HandleGameEnd(msg);
                    break;

            }
        }
    }
    private void Update()
    {
        if(ReceivedMessages.Count > 0) 
        {
            for(int i = 0; i < ReceivedMessages.Count; i++)
            {
                Deserealize(ReceivedMessages.Dequeue());
            }
            
        }
    }
    public void AddToDeserealizeQueue(string receivedMSG)
    {
        ReceivedMessages.Enqueue(receivedMSG);
    }
    private bool IsValidJson(string jsonString)
    {
        try
        {
            JsonUtility.FromJsonOverwrite(jsonString, new object());
            return true;
        }
        catch (System.Exception)
        {
            return false;
        }
    }


    private void HandleConnectionMessage(MessageToSend msg)
    {
        Debug.Log("Server changed your ID to: " + msg.ID);
        myUser.userID = msg.ID;
    }
    private void HandleConnectionCheck(MessageToSend msg)
    {

    }
    private void HandleWaitingRoomMessage(MessageToSend msg)
    {

    }
    private void HandleChatMessage(MessageToSend msg)
    {
        FindObjectOfType<Chat>().EnqueueMessage(msg);
    }
    private void HandleGameStart(MessageToSend msg)
    {
        SceneManager.LoadScene("Scene02");
    }
    private void HandleCharacterInfo(MessageToSend msg)
    {
        if(character == null) character = FindObjectOfType<RemoteCharacter>();
        character.UpdateRemoteCharacter(msg.UserCharacterInfo);
    }
    private void HandleGeneratePlayers(MessageToSend msg)
    {

    }
    private void HandleGameEnd(MessageToSend msg)
    {

    }
}


