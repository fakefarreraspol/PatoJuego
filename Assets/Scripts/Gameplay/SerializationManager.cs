using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

public class SerializationManager : MonoBehaviour
{
    private RemoteCharacter obj;
    private Queue<PlayerActionData> deserealization = new Queue<PlayerActionData>();
    private void Start()
    {
        obj = FindObjectOfType<RemoteCharacter>();
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

    private void EnqueueMessage(PlayerActionData data)
    {
        deserealization.Enqueue(data);
    }
    private void Update()
    {
        if (deserealization.Count > 0)
        {
            PlayerActionData Char = deserealization.Dequeue();
            FindObjectOfType<RemoteCharacter>().UpdateRemoteCharacterPos(Char);
        }
    }

    public void Deserealize(string Json)
    {
        if (IsValidJson(Json))
        {
            EnqueueMessage(PlayerData(Json));
        }
    }

    public PlayerActionData PlayerData(string JSon)
    {
        PlayerActionData pData = JsonUtility.FromJson<PlayerActionData>(JSon);
        Debug.Log(pData.playerTransform);
        return pData;

    }
}
