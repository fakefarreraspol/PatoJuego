using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deserealizer : MonoBehaviour
{
    public void Deserealize(string Message)
    {
        Debug.Log("Deserealizing message:    " +Message);
        if(!IsValidJson(Message)) 
        {
            return;
        }
        else
        {
            MessageToSend msg = JsonUtility.FromJson<MessageToSend>(Message);

            switch (msg.messageType)
            {
                case MessageType.CONNECTION:
                    break;
                case MessageType.CHECK_CONNECTION:
                    break;
                case MessageType.WAITING_ROOM:
                    break;
                case MessageType.CHAT_MESSAGE:
                    break;
                case MessageType.GAME_START:
                    break;
                case MessageType.CHARACTER_INFO:
                    FindObjectOfType<RemoteCharacter>().UpdateRemoteCharacterPos(msg.UserCharacterInfo);
                    break;
                case MessageType.GENERATE_PLAYERS:
                    break;
                case MessageType.GAME_END:
                    break;

            }
        }
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
}


