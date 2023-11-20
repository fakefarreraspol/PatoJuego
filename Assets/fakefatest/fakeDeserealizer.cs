using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fakeDeserealizer : MonoBehaviour
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
        if(IsValidJson(Json))
        {
            PlayerData(Json);
        }
    }

    public fakePlayerData PlayerData(string JSon)
    {
        fakePlayerData pData = JsonUtility.FromJson<fakePlayerData>(JSon);
        Debug.Log(pData.playerTransform);
        return pData;

    }
}
