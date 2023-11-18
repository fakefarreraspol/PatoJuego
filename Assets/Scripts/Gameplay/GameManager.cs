using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject managerObject = new GameObject("GameManager");
                    instance = managerObject.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //GameManager functionality

    void SaveGame()
    {
        // Assuming UserInfo is a component attached to a GameObject in the scene
        UserInfo userInfo = FindObjectOfType<UserInfo>();

        if (userInfo != null)
        {
            // Assuming you have a reference to the SerializationManager
            byte[] serializedUserInfo = SerializationManager.Instance.SaveUserInfo(userInfo);
            // Save or send the serializedUserInfo as needed
        }
        else
        {
            Debug.LogError("UserInfo not found in the scene.");
        }
    }

    void LoadGame(byte[] savedData)
    {
        // Assuming you have a reference to the SerializationManager
        UserInfo userInfo = FindObjectOfType<UserInfo>();

        if (userInfo != null)
        {
            userInfo = SerializationManager.Instance.LoadUserInfo(savedData);
            // Use the loadedUserInfo as needed
        }
        else
        {
            Debug.LogError("UserInfo not found in the scene.");
        }
    }
}