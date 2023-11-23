using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeDeserealizer : MonoBehaviour
{
    private fakeCharacterRemote obj;
    private Queue<fakePlayerData> deserealize = new Queue<fakePlayerData>();
    private void Start()
    {
        obj = FindObjectOfType<fakeCharacterRemote>();
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
    private void EnqueueMessage(fakePlayerData fData)
    {
        deserealize.Enqueue(fData);
    }
    private void Update()
    {
        

        if (deserealize.Count > 0)
        {
            fakePlayerData fChar = deserealize.Dequeue();
            FindObjectOfType<fakeCharacterRemote>().UpdateRemoteCharacterPos(fChar);
        }
        
        
    }

    public void Deserealize(string Json)
    {
        if(IsValidJson(Json))
        {
            //obj.GetComponent<fakeCharacterRemote>().UpdateRemoteCharacterPos(PlayerData(Json));
            //Debug.Log(FindObjectOfType<fakeCharacterRemote>().transform.ToString());
            EnqueueMessage(PlayerData(Json));
        }
    }

    public fakePlayerData PlayerData(string JSon)
    {
        fakePlayerData pData = JsonUtility.FromJson<fakePlayerData>(JSon);
        Debug.Log(JSon);
        return pData;

    }
}
