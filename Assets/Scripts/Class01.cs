using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

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
        var uwr = new UnityWebRequest(fileURL, UnityWebRequest.kHttpVerbGET);
        string path = Path.Combine(Application.dataPath, "fakefa.pdf");
        uwr.downloadHandler = new DownloadHandlerFile(path);
        yield return uwr.SendWebRequest();
        if (uwr.result != UnityWebRequest.Result.Success)
            Debug.LogError(uwr.error);
        else
            Debug.Log("File successfully downloaded and saved to " + path);
        
    }
    
}
