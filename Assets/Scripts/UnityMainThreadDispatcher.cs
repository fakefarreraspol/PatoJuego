using UnityEngine;
using System;
using System.Collections.Generic;


public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private readonly Queue<Action> actionQueue = new Queue<Action>();

    // Ensure there's only one instance of UnityMainThreadDispatcher
    public static UnityMainThreadDispatcher Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("UnityMainThreadDispatcher").AddComponent<UnityMainThreadDispatcher>();
            }
            return instance;
        }
    }

    // Enqueue an action to be executed on the main thread
    public void Enqueue(Action action)
    {
        lock (actionQueue)
        {
            actionQueue.Enqueue(action);
        }
    }

    private void Update()
    {
        lock (actionQueue)
        {
            while (actionQueue.Count > 0)
            {
                actionQueue.Dequeue().Invoke();
            }
        }
    }

    //public void Enqueue(Action action)
    //{
    //    instance?.RunOnMainThread(action);
    //}

    //private void RunOnMainThread(Action action)
    //{
    //    if (this != null)
    //    {
    //        action?.Invoke();
    //    }
    //}
}