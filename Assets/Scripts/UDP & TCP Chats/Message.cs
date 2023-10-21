using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI userMessageText;
    public Image userAvatarImage;

    private void Awake()
    {
        userAvatarImage = gameObject.transform.Find("MsgUserImage").GetComponent<Image>();
        usernameText = gameObject.transform.Find("MsgUserName").GetComponent<TextMeshProUGUI>();
        userMessageText = gameObject.transform.Find("MsgText").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GetComponent<RectTransform>().SetAsFirstSibling();
    }
}
