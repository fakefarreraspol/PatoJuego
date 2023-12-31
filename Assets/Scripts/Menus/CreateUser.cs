using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class CreateUser : MonoBehaviour
{
    GameObject UserOBJ;

    private Slider colorSlider;
    private TMP_InputField textInput;



    private void Awake()
    {
        textInput = FindObjectOfType<TMP_InputField>();
    }
    // Start is called before the first frame update
    private void Start()
    {        
        User user = SaveAndLoad.LoadUser();
        if (user != null) 
        {
            GenerateUserObject(user);
            DontDestroyOnLoad(UserOBJ);
            FindObjectOfType<GameManager>().ChangeState( GameManager.GameState.Intro);
            SceneManager.LoadScene("Intro");
            
        }
    }

    public void CreateNewUser()
    {
        User newUser =  new User();
        if (textInput.text == "")
        {
            newUser.Username = "user" + Random.Range(001, 100);
        }
        else newUser.Username = textInput.text;

        newUser.maxScore = 0;
        SaveAndLoad.SaveUser(newUser);

        GenerateUserObject(newUser);

        DontDestroyOnLoad(UserOBJ);

        FindObjectOfType<GameManager>().ChangeState(GameManager.GameState.Intro);

        SceneManager.LoadScene("Intro");
        
    }

    private void GenerateUserObject(User userD)
    {
        UserOBJ = new GameObject("UserData");

        UserManager newUserObjectData = UserOBJ.AddComponent<UserManager>();
        newUserObjectData.Username = userD.Username;        
        newUserObjectData.maxScore = userD.maxScore;
    }
}
