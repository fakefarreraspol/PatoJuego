using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void GoBackToUserCreationScene()
    {
        SaveAndLoad.DeleteFile();
        Destroy(FindObjectOfType<UserManager>().gameObject);
        SceneManager.LoadScene("CreateUser");
    }
}
