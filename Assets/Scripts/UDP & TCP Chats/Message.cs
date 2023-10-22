using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    private void Start()
    {
        //This makes the new message be on top of the other ones.
        GetComponent<RectTransform>().SetAsFirstSibling();
    }
}
