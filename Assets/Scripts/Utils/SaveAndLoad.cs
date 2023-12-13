using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveAndLoad
{
    public static void SaveUser(User user)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.sigma";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, user);
        stream.Close();
    }

    public static User LoadUser()
    {
        string path = Application.persistentDataPath + "/player.sigma";
        if(File.Exists(path)) 
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            User uData = formatter.Deserialize(stream) as User;

            stream.Close();

            return uData;
        }
        else
        {
            Debug.LogError("save file not found in " + path);
            return null;
        }
    }
}
