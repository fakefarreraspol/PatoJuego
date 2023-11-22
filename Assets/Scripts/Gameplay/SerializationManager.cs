using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

public class SerializationManager : MonoBehaviour
{
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


    public void Deserealize(string Json)
    {
        if (IsValidJson(Json))
        {
            PlayerData(Json);
        }
    }

    public PlayerActionData PlayerData(string JSon)
    {
        PlayerActionData pData = JsonUtility.FromJson<PlayerActionData>(JSon);
        Debug.Log(pData.playerTransform);
        return pData;

    }
}
