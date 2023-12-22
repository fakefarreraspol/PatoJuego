using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deserealizer : MonoBehaviour
{
    Queue<string> ReceivedMessages = new Queue<string>();
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
                    break;
                case MessageType.CHECK_CONNECTION:
                    break;
                case MessageType.WAITING_ROOM:
                    break;
                case MessageType.CHAT_MESSAGE:
                    FindObjectOfType<Chat>().EnqueueMessage(msg);
                    break;
                case MessageType.GAME_START:
                    break;
                case MessageType.CHARACTER_INFO:
                    Debug.Log("hhhhhhhhhhhhh"+msg.UserCharacterInfo);
                    FindObjectOfType<RemoteCharacter>().UpdateRemoteCharacterPos(msg.UserCharacterInfo);
                    break;
                case MessageType.GENERATE_PLAYERS:
                    break;
                case MessageType.GAME_END:
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
}


