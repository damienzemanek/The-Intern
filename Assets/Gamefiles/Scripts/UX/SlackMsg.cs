using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlackMsg : MonoBehaviour
{
    public TextMeshProUGUI txt_MessageName;
    public Image spr_personImage;

    public void InitMessage(string msg, Sprite pic)
    {
        txt_MessageName.text = msg;
        spr_personImage.sprite = pic;
    }
}