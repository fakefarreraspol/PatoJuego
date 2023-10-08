using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class test : MonoBehaviour
{

    private Thread thread;
    private float rot;
    private void Start()
    {
        // Create and start a new thread
        thread = new Thread(DoWork);
        thread.Start();
    }

    private void DoWork()
    {
        // This code will run on a separate thread
        Debug.Log("Hello from another thread!");

        // You can perform more complex tasks here
        for (int i = 0; i < 10; i++)
        {
            Debug.Log($"Thread working... {i}");
            rot += i;
            Thread.Sleep(1000); // Sleep for 1 second
        }
        thread.Abort();
    }

    
}
