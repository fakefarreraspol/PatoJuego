using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

public class SerializationManager : MonoBehaviour
{
    // Singleton pattern for easy access throughout the project
    private static SerializationManager instance;

    public static SerializationManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SerializationManager>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("SerializationManager");
                    instance = managerObject.AddComponent<SerializationManager>();
                }
            }
            return instance;
        }
    }

    // Serialize an object to a byte array
    public byte[] SerializeObject(object obj)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream())
        {
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }
    }

    // Deserialize a byte array to an object
    public T DeserializeObject<T>(byte[] data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (MemoryStream stream = new MemoryStream(data))
        {
            return (T)formatter.Deserialize(stream);
        }
    }

    // Example: Serialize a UserInfo object
    public byte[] SaveUserInfo(UserInfo userInfo)
    {
        byte[] serializedData = SerializeObject(userInfo);
        // Save or send the serializedData as needed
        return serializedData;
    }

    // Example: Deserialize a UserInfo object
    public UserInfo LoadUserInfo(byte[] savedData)
    {
        return DeserializeObject<UserInfo>(savedData);
    }
}
