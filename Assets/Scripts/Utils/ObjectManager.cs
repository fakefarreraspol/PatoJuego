using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    // Dictionary to store the mapping of IDs to GameObjects
    private Dictionary<int, GameObject> objectDictionary = new Dictionary<int, GameObject>();

    // Function to add a GameObject to the dictionary with a specified ID
    public void AddGameObject(int id, GameObject gameObject)
    {
        if (!objectDictionary.ContainsKey(id))
        {
            objectDictionary.Add(id, gameObject);
        }
        else
        {
            Debug.LogWarning("An object with ID " + id + " already exists in the dictionary.");
        }
    }

    // Function to get a GameObject based on its ID
    public GameObject GetGameObject(int id)
    {
        if (objectDictionary.ContainsKey(id))
        {
            return objectDictionary[id];
        }
        else
        {
            Debug.LogWarning("No object found with ID " + id + ".");
            return null;
        }
    }

    // Function to remove a GameObject from the dictionary based on its ID
    public void RemoveGameObject(int id)
    {
        if (objectDictionary.ContainsKey(id))
        {
            objectDictionary.Remove(id);
        }
        else
        {
            Debug.LogWarning("No object found with ID " + id + ".");
        }
    }

    // Example of how to use the GameObjectManager
    //private void ExampleUsage()
    //{
    //    // Assuming you have GameObjects with unique IDs
    //    int objectId1 = 1;
    //    int objectId2 = 2;

    //    // Adding GameObjects to the manager
    //    GameObject object1 = /* Your GameObject */;
    //    GameObject object2 = /* Your GameObject */;

    //    AddGameObject(objectId1, object1);
    //    AddGameObject(objectId2, object2);

    //    // Retrieving GameObjects from the manager
    //    GameObject retrievedObject1 = GetGameObject(objectId1);
    //    GameObject retrievedObject2 = GetGameObject(objectId2);

    //    // Removing GameObjects from the manager
    //    RemoveGameObject(objectId1);
    //    RemoveGameObject(objectId2);
    //}
}
