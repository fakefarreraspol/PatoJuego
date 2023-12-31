using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerLobby : MonoBehaviour
{
    public TMP_Dropdown dropdownMap;
    private int MapSelected;
    public void StartGame()
    {
        MapSelected = dropdownMap.value;

        if (MapSelected == 0 )
        {
            MessageToSend StartGameMsg = new MessageToSend(FindObjectOfType<UserManager>().userID, FindObjectOfType<UserManager>().Username, MessageType.GAME_START, MapSelected.ToString());
            FindObjectOfType<GameManager>().ChangeState(GameManager.GameState.Spawn);
            string msg = JsonUtility.ToJson(StartGameMsg);
            FindObjectOfType<Server>().SendMessageToAllClients(msg);
            SceneManager.LoadScene("Scene01");
        }

    }
  
}
