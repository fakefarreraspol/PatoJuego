using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class fakeDeserealizer : MonoBehaviour
{
    private fakeCharacterRemote obj;
    private Queue<fakePlayerData> deserealize = new Queue<fakePlayerData>();
 
    private Queue<string> messages = new Queue<string>();

    private GameObjectManager goManager;
    int numID;
    private void Start()
    {
        obj = FindObjectOfType<fakeCharacterRemote>();
        goManager = FindObjectOfType<GameObjectManager>();
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
    private void EnqueuePlayerDataMessage(fakePlayerData fData)
    {
        deserealize.Enqueue(fData);
    }
    
    private void Update()
    {
        
        if (messages.Count > 0)
        {
            Debug.Log("msg count: " + messages.Count);
            string msg = messages.Dequeue();
            if (IsValidJson(msg))
            {
                if (FindObjectOfType<fakeTestServer>() != null)
                {
                    FindObjectOfType<fakeTestServer>().SendMessageToAllClients(msg);
                }
                
                EnqueuePlayerDataMessage(PlayerData(msg));

            }
            else
            {
                int newNum = int.Parse(msg);
                if (newNum == 112) { FindObjectOfType<fakeGameManager>().gState = fakeGameManager.GameState.InGame; }
                else numID = newNum;
            }
        }
        

        if (deserealize.Count > 0 && FindObjectOfType<fakeGameManager>().gState == fakeGameManager.GameState.InGame)
        {
            fakePlayerData fChar = deserealize.Dequeue();
            
            if ((fChar.userId != FindObjectOfType<fakefaID>().IDDDDDDDD) && (fChar.newUser == true)) FindObjectOfType<fakeSpawner>().PlsSpawnPlayer(fChar.userId);

            if (fChar.userId != FindObjectOfType<fakefaID>().IDDDDDDDD)
            {
                if (!FindObjectOfType<GameObjectManager>().CheckIfItsAlreadyListed(fChar.userId))
                {
                    FindObjectOfType<fakeSpawner>().PlsSpawnPlayer(fChar.userId);
                }
                FindObjectOfType<GameObjectManager>().GetGameObject(fChar.userId).GetComponent<fakeCharacterRemote>().UpdateRemoteCharacterPos(fChar);
            }

        }
        if (FindObjectOfType<fakefaID>() != null) FindObjectOfType<fakefaID>().changeID(numID);
    }

    public void Deserealize(string Json)
    {
        Debug.Log(Json);
        messages.Enqueue(Json);
    }

    public fakePlayerData PlayerData(string JSon)
    {
        fakePlayerData pData = JsonUtility.FromJson<fakePlayerData>(JSon);
        Debug.Log(JSon);
        return pData;

    }

}
