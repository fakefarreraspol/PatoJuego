using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winlose : MonoBehaviour
{
    public int lifes = 3;
    public GameObject loseScreen;
    public void playerDied()
    {
        lifes--;
       
        if (lifes == 0)
        {
            Debug.LogWarning("YOUDIED");
            loseScreen.SetActive(true);
        }
    }
}
