using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class fakeLobby : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Update()
    {
        DontDestroyOnLoad(FindObjectOfType<fakefaID>().gameObject);
        if (Input.GetKey(KeyCode.L))
        {
            if (FindObjectOfType<fakeTestServer>() != null)
            { 
                FindObjectOfType<fakeTestServer>().SendMessageToAllClients("112");
                FindObjectOfType<fakeGameManager>().gState = fakeGameManager.GameState.InGame;
            }
        }

        if(FindObjectOfType<fakeGameManager>().gState == fakeGameManager.GameState.InGame)
        {
            
            if (FindObjectOfType<fakeTestServer>() != null) SceneManager.LoadScene("Scene 01");
            else SceneManager.LoadScene("Scene02");
        }
    }
}
