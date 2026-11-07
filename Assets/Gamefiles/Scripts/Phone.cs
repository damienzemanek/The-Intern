using System;
using UnityEngine;
using static AudioManager;


public class Phone : MonoBehaviour
{
    public Animator phoneAnimator;
    public string buzzAnimName = "Buzz";
    public GameObject go_notification;

    private void Awake() => RemoveNotification();

    public void NewNotification()
    {
        go_notification.SetActive(true);
        UseSourceWithClip(SourceTrack.Player, AudioClipType.PhoneBuzz, out var clip)
            .PlayOneShot(clip);
        phoneAnimator.Play(buzzAnimName);
    }

    public void RemoveNotification()
    {
        go_notification.SetActive(false);
    }
    
}
