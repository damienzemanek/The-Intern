using System;
using TMPro;
using UnityEngine;

public class ClockText : MonoBehaviour
{
    public TextMeshProUGUI txt_clock;

    public void FixedUpdate()
    {
        txt_clock.text = DateTime.Now.ToString("h:m");
    }
}
