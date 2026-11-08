using System;
using UnityEngine;
using UnityEngine.Serialization;
using static AudioManager;


public class Phone : MonoBehaviour
{
    public bool hasNotif;
    public Animator phoneAnimator;
    public string buzzAnimName = "Buzz";
    public GameObject go_notificationScreenIrl;
    public GameObject go_DirectScreen;
    public GameObject go_AppScreen;

    private void Awake() => RemoveNotification();

    public void NewNotification()
    {
        go_DirectScreen.gameObject.SetActive(true);
        go_AppScreen.gameObject.SetActive(false);
        hasNotif = true;
        go_notificationScreenIrl.SetActive(true);
        UseSourceWithClip(SourceTrack.Player, AudioClipType.PhoneBuzz, out var clip)
            .PlayOneShot(clip);
        phoneAnimator.Play(buzzAnimName);
    }

    public void RemoveNotification()
    {
        go_DirectScreen.gameObject.SetActive(false);
        go_AppScreen.gameObject.SetActive(true);
        hasNotif = false;
        go_notificationScreenIrl.SetActive(false);
    }
    
}
