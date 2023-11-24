using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class fakeNewIntro : MonoBehaviour
{
    public TMP_InputField nameTextInput;
    public TMP_InputField IPInput;
    public TMP_InputField PortInput;

    public void onIntroButtonPressed()
    {
        fakeDatos fake = FindObjectOfType<fakeDatos>();
        if (PortInput.text == "") fake.port = 8080;
        else fake.port = int.Parse(PortInput.text);

        fake.ip = IPInput.text;

        DontDestroyOnLoad(FindAnyObjectByType<fakeDatos>().gameObject);
        GameObject empty = new GameObject("Empty01");
        empty.AddComponent<fakeTestClient>();
        DontDestroyOnLoad(empty);
        FindObjectOfType<fakeGameManager>().gState = fakeGameManager.GameState.Lobby;
        DontDestroyOnLoad(FindObjectOfType<fakeGameManager>().gameObject);
        DontDestroyOnLoad(FindObjectOfType<fakeDeserealizer>().gameObject);
        SceneManager.LoadScene("Lobby");
    }
    public void onIntroButtonPressed2()
    {
        fakeDatos fake = FindObjectOfType<fakeDatos>();
        if (PortInput.text == "") fake.port = 8080;
        else fake.port = int.Parse(PortInput.text);

        fake.ip = IPInput.text;

        DontDestroyOnLoad(FindAnyObjectByType<fakeDatos>().gameObject);
        GameObject empty = new GameObject("Empty02");
        empty.AddComponent<fakeTestServer>();
        DontDestroyOnLoad(empty);
        FindObjectOfType<fakeGameManager>().gState = fakeGameManager.GameState.Lobby;
        DontDestroyOnLoad(FindObjectOfType<fakeGameManager>().gameObject);
        DontDestroyOnLoad(FindObjectOfType<fakeDeserealizer>().gameObject);
        SceneManager.LoadScene("Lobby");
    }
}
