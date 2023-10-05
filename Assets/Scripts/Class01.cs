using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class Class01 : MonoBehaviour
{
    // Start is called before the first frame update
    private string fileURL = "https://fakefarreraspol.github.io/Images/PolFarrerasCV.pdf";

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, 1);
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GetFakefaCurriculum());
        }
    }
    IEnumerator GetFakefaCurriculum()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(fileURL);

        yield return webRequest.SendWebRequest();

        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + webRequest.error);
        }
        else
        {
            // Save the downloaded file
            System.IO.File.WriteAllBytes(Application.dataPath, webRequest.downloadHandler.data);
            Debug.Log("File downloaded successfully to: " + Application.dataPath);
        }
    }
}
