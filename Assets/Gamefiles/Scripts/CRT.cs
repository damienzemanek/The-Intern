using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CRT : MonoBehaviour
{
    [Required] public GameObject go_emailScreen;
    [Required] public GameObject go_ReceivedEmailScreen;
    [Required] public GameObject go_replyArea;
    [Required] public TextMeshProUGUI txt_replyText;

    private void OnEnable()
    {
        go_replyArea.gameObject.SetActive(false);
        txt_replyText.text = string.Empty;
    }

    public void OpenEmailApp()
    {
        go_emailScreen.SetActive(true);
    }

    public void OpenEmail()
    {
        go_ReceivedEmailScreen.SetActive(true);
    }

    public void AutoReply(string txt)
    {
        go_replyArea.SetActive(true);
        txt_replyText.text = txt;
    }
}
