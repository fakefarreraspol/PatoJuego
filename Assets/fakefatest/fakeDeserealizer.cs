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
            string msg = messages.Dequeue();
            if (IsValidJson(msg))
            {
                //obj.GetComponent<fakeCharacterRemote>().UpdateRemoteCharacterPos(PlayerData(Json));
                //Debug.Log(FindObjectOfType<fakeCharacterRemote>().transform.ToString());
                if (FindObjectOfType<fakeTestServer>() != null)
                {
                    FindObjectOfType<fakeTestServer>().SendMessageToAllClients(msg);
                }
                
                if (TryDeserialize<fakePlayerData>(msg)) EnqueuePlayerDataMessage(PlayerData(msg));
                
                //PlayerData(Json);
            }
            else
            {
                numID = int.Parse(msg);
            }
        }
        

        if (deserealize.Count > 0)
        {
            fakePlayerData fChar = deserealize.Dequeue();
            
            if (fChar.userId != FindObjectOfType<fakefaID>().IDDDDDDDD && fChar.newUser) FindObjectOfType<fakeSpawner>().PlsSpawnPlayer(fChar.userId);

            if (fChar.userId != FindObjectOfType<fakefaID>().IDDDDDDDD)
                goManager.GetGameObject(fChar.userId).GetComponent<fakeCharacterRemote>().UpdateRemoteCharacterPos(fChar);
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
    

    bool TryDeserialize<T>(string json)
    {
        try
        {
            JsonUtility.FromJson<T>(json);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
